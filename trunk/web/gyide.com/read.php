<?php
set_time_limit(60);
header("Content-type:text/html; charset=utf-8");
/*
 * 包含库文件
 */
include "./common.inc.php";
include "./mysql_db.php";
include "./fun.php";

/*=============================================
 *
 *	获取基础参数
 *	$domain:当前主域名
 *	$ename：当前二级域名，包含'.'
 *	$path：当前路径
 *	$base_url:当前路径，包含最后一个'/'
 *	$cfg_dbqz：表前缀
 *	$epath:二级域名+路径，用以确定cache位置md5()或者搜索数据库
 *	$rnid:read-(.*).html
 *
 =============================================*/

$host=$_SERVER["HTTP_HOST"];
$array_temp=explode(".",$host);
$len=count($array_temp);
$domain= $array_temp[$len-2].'.'.$array_temp[$len-1];
if($len==2){
	$ename='';//主域名，无二级域名
}else{
	$ename=$array_temp[0].'.';
}
$rnid=in_str('read-','.html',$_SERVER["REQUEST_URI"]);
$array_temp = explode ('/',$_SERVER["REQUEST_URI"]);	//$_SERVER["REQUEST_URI"] 获取URL，以‘/’开头 '/'     '/xxx.zzz'       '/xxx/yyy.zzz'      三种格式
$path= $array_temp[1];
	/*
	 *	看path的值，
	 *	如果为空，说明在根目录下“/”
	 *	如果不包含“.“，说明是目录
	 *	如果包含“.”，说明是文件
	 */
if(strpos($path,".")>0){	//有'.'说明是文件，必定在根目录
	$path='/';
}else{						//没有'.'说明可能是'/'或者是'/xxx/yyy.zzz'；对应path的值为''，'xxx'
	$path='/'.$path."/";
}
if($path=='//'){$path='/';}	//确定$path的值
$base_url=$ename.$domain.$path;
$epath=$ename.$path;
$cfg_dbqz = str_replace('.','_',$domain).'_';//数据表前缀
ini_set("magic_quotes_runtime",0);
/*
 * 随机文章--分词打乱部分
 * cache路径：$base_url.'read-'.$rnid
 * url路径:/path/read-$rnid.html
 * $rnid:随机1-9999999的数字，用以确定采用哪条数据,cache*
 * $rn_url:由$rnid获取cache路径
 * rn_title:输出标题*
 * rn_des:输出描述*
 */
$cache_index=cache_get_path($epath,$cfg_cache_path.'index/');		//主页的缓存，必须要有，需要读取
if(!file_exists($cache_index)){
	//缓存不存在。说明出错了。
	echo "参数错误或当前页不可用。url：".$_SERVER["HTTP_HOST"].$_SERVER["REQUEST_URI"].'请稍候再试'.md5($rnid);
	die();
}
/*
 * 读取参数
 * arr_index:主页的参数
 */
$con_index=file_get_contents($cache_index);
$arr_index=unserialize($con_index);
/*
 * 生成标题，正文部分
 */
$arr_body=readart($arr_index['key'],$arr_index['keys'],$rnid,$base_url);
//输出
out_view($arr_index,$arr_body,$domain,$base_url);

function out_view($arr,$arr_view,$domain,$base_url){
	global $cfg_tk_pid;
	$con=file_get_contents("./templets/view.html");
	/* =================
	 * 生成链接
	 * =================
	 *
	 * 生成相关链接
	 * $key_link:由关键词列表生成关键词连接集
	 */
	$arr_keys=explode(",",$arr['keys']);	//生成相关关键词链接
	$key_link='';					//由关键词动态生成，不记录在cache，但要输出
	foreach($arr_keys as $kk){
		$key_epath=get_epath($kk);
		$arr_temp=explode("/",$key_epath);
		$key_link.='<li><a href="http://'.$arr_temp[0].$domain.'/'.$arr_temp[1].'">'.$kk.'</a></li>';
	}
	/*
	 * $tag_link:生成tag链接，包含随机部分及互连部分
	 */
	$tag_link=$arr['flink'];					//生成tag链接，包含随机部分及互连部分（数据库给出）
	$arr_randkey=explode(',',$arr['tag_key']);
	foreach($arr_randkey as $kk){
		$key_epath=get_epath($kk);
		$arr_temp=explode("/",$key_epath);
		$tag_link.='<li><a href="http://'.$arr_temp[0].$domain.'/'.$arr_temp[1].'">'.$kk.'</a></li>';
	}
	$base=str_replace('/x','',$base_url.'x');
	/*
	 * 替换数组
	 */
	$arr_rep=array();
	$arr_rep['key']=$arr['key'];
	$arr_rep['title']=$arr_view['title'];
	$arr_rep['maintitle']=$arr['title'];
	$arr_rep['miaoshu']=$arr_view['miaoshu'];
	$arr_rep['base_url']=$base;
	$arr_rep['body']=$arr_view['body'];
	$arr_rep['adkey']=$arr['adkey'];
	$arr_rep['adkey_code']=urlencode($arr['adkey']);
	$arr_rep['adkey_iconv']=urlencode(iconv("UTF-8", "GBK//IGNORE", $arr['adkey']));
	$arr_rep['pid']=$cfg_tk_pid;
	$arr_rep['keys_link']=$key_link;
	$arr_rep['tag_link']=$tag_link;
	/*
	 * 替换
	 */
	foreach($arr_rep as $k=>$v){
		$con=str_replace('{'.$k.'}',$v,$con);
	}
	echo $con;
	include('./fun/robot.php');
}

/*================================
 * 随机文章--分词打乱部分
 * $key:关键词
 * $keys：相关关键词
 * $a_no：1-99999999中的一个随机数，确定路径
 * $base_url:url路径
 * 返回$arr_res[]包含title，body。
 ================================*/
function readart($key,$keys,$a_no,$base_url){
	global $cfg_xs_url;
	$url='http://'.$base_url.'read-'.$a_no.'html';
	$md5_url=md5($url);
	/*
	 * 生成指纹序列
	 * $art_sp:文章指纹序列
	 * $body_sp:正文指纹序列
	 * $choas_sp:乱序指纹序列
	 */
	for($i=0;$i<7;$i++){//标题5个,文章28个指纹序列
		$body_sp[$i]=toTen(substr($md5_url,$i*4,4))+1;
		$body_sp[$i+7]=toTen(substr($md5_url,$i*4+1,4))+1;
		$body_sp[$i+14]=toTen(substr($md5_url,$i*4+2,4))+1;
		$body_sp[$i+21]=toTen(substr($md5_url,$i*4+3,4))+1;
		if($i<5){$art_sp[$i]=$body_sp[$i];}//标题文章序列
	}
	$choas_sp=str_split(toTen(substr($md5_url,0,8)).toTen(substr($md5_url,8,8)).toTen(substr($md5_url,16,8)).toTen(substr($md5_url,24,8)),1);
	/*
	 * 副关键词
	 */
	$arr_keys=explode(",",$keys);
	$arr_keys=arr_choas($arr_keys,$choas_sp);
	$key2=$arr_keys[0];
	/*
	 * 确定标题
	 * 先从5个文本组合副关键词组合打乱一次
	 * 再提取20字符，加入主关键词打乱一次
	 */
	$title='';
	$des='';
	for($i=0;$i<5;$i++){
		$temp=file_get_contents($cfg_xs_url.$art_sp[$i].".txt");
		$title.=$temp.'|'.$key2;
	}
	$body=$title;
	$arr_title=explode("|",$title);
	$arr_title=arr_choas($arr_title,$choas_sp);//按指纹乱序标题
	$title = implode('|',$arr_title);
	$title=preg_replace("%<[^>]+>%",',',$title);//防止网址中字母被匹配
	$title=str_replace(array(',','。','：','”','“','！','……','？','；',' ','，'),'',$title);
	$title=replace_db($title,'');
	$title = substr_utf8($title, 0, 20);
	$arr_title=explode("|",$title.'|'.$key);
	$arr_title=arr_choas($arr_title,$choas_sp);
	$title=implode('',$arr_title);
	/*
	 *  确定正文
	 */
	for($i=5;$i<28;$i++){
		$temp=file_get_contents($cfg_xs_url.$body_sp[$i].".txt");
		$body.='|'.$temp;
		if($i<16){$body.='|'.$key.'|'.'。<br>';}
	}
	$arr_body=explode("|",$body);
	$arr_body=arr_choas($arr_body,$choas_sp);
	$body=implode('',$arr_body).'。';
	$body=replace_dbs($body,'。');

	/*
	 * 返回
	 * $arr_res[]
	 */
	$arr_res=array();
	$arr_res['title']=$title;
	$arr_res['body']=$body;
	return $arr_res;
}

/*================================
 * 按指纹序列打乱数组
 * $arr_str:待打乱数组
 * $choas_sp:指纹序列
 ================================*/
 function arr_choas($arr_str,$choas_sp){
	$arr=$arr_str;
	$arrlen=count($arr);
	$len=count($choas_sp);
	$x=0;//用以记录指纹序列当前位置
	for($i=0;$i<$arrlen;$i++){
		if($x>=$len){$x=0;}					//指纹序列用光，循环取用
		$no=$choas_sp[$x++];
		if($no>=$arrlen){$no=$arrlen-1;}	//如果指纹序列当前值超过数组最大值，取数组的最后组员
		$temp=$arr[$arrlen-$i-1];			//交换
		$arr[$arrlen-$i-1]=$arr[$no];
		$arr[$no]=$temp;
	}
	return $arr;
 }
?>
