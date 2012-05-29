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
 *	$rid:view-(.*).html
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
$rid=in_str('view-','.html',$_SERVER["REQUEST_URI"]);
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
ini_set("magic_quotes_runtime",0);

/*
 *百度问答采集部分
 *cache路径：$epath.$rid
 *url路径:/path/view-$rid.html
 *每个关键词采集10篇
 *$rid:随机1-10的数字，用以确定采用哪条数据,cache*
 *$r_url:由$rid获取cache路径
 *$b_pg:页码,需要cache*
 *ov_title:输出标题*
 *ov_des:输出描述*
 */
$cache_view=cache_get_path($epath.$rid,$cfg_cache_path.'view/');	//问答页的缓存
$cache_index=cache_get_path($epath,$cfg_cache_path.'index/');		//主页的缓存，必须要有，需要读取
if(!file_exists($cache_view)){
	//缓存不存在。说明出错了。
	echo "参数错误或当前页不可用。url：".$_SERVER["HTTP_HOST"].$_SERVER["REQUEST_URI"].'请稍候再试'.md5($rid);
	die();
}
if(!file_exists($cache_index)){
	//缓存不存在。说明出错了。
	echo "参数错误或当前页不可用。url：".$_SERVER["HTTP_HOST"].$_SERVER["REQUEST_URI"].'请稍候再试'.md5($rid);
	die();
}

/*
 * 读取参数
 * arr_index:主页的参数
 * arr_view:问答页的参数
 */
$con_index=file_get_contents($cache_index);
$arr_index=unserialize($con_index);
$con_view=file_get_contents($cache_view);
$arr_view=unserialize($con_view);

/*
 * 相关问答页
 * rid的范围是1-10，查看各问答页的cache文件是否存在即可
 * 由$x_path给出
 * $x_link:相关链接
 */
if($arr_view['x_no']<9){
	$x_link='';
	$x_no=0;
	for($i=1;$i<=10;$i++){
		if($i==$rid){continue;}	//本页就不判定了
		$x_path=cache_get_path($epath.$i,$cfg_cache_path.'view/');	//缓存路径
		if(file_exists($x_path)){
			$con_x=file_get_contents($x_path);
			$arr_x=unserialize($con_x);
			$x_link.='<a href="http://'.$base_url.'view-'.$i.'.html">'.$arr_x['atitle'].'</a><br>';	//相关问题
			$x_no++;
		}
	}
	$arr_view['x_link']=$x_link;
	$arr_view['x_no']=$x_no;
}


/*
 * 还有内容未完善，抓取。每次抓取一篇文章。
 * 每篇文章错误5次跳过
 * error:文章错误数
 * notyet:未抓取的文章
 * body:文章正文
 */
$up=0;
if($arr_view['error']<5 && count($arr_view['notyet'])>0){
	$arr_view['title']=str_replace('...','',$arr_view['atitle']);
	$bid=$arr_view['notyet'][0];
	//抓取
	$con=get_view($bid);
	//处理抓取的数据
	if($con['title']==''){
		//抓取失败
		$arr_view['error']+=1;
		$up=1;
	}else{
		//抓取成功
		$arr_answer=$con['answers'];
		if(count($arr_answer)>1){shuffle($arr_answer);}//打乱答案
		$body=implode('<br>',$arr_answer).'<br>'.$arr_view['body'];
		$arr_body=explode('<br>',$body);
		if(count($arr_body)>1){shuffle($arr_body);}//打乱正文
		$body=implode('<br>',$arr_body);
		$con['title']=str_replace('...','',$con['title']);
		$body=str_replace($con['title'],'',$body);
		$body=str_replace($arr_view['title'],'',$body);
		$arr_view['error']=0;						//重置错误次数
		$arr_view['body']=$body;
		//更新notyet
		array_splice($arr_view['notyet'],0,1);//去掉成功的文章
		$up=1;
	}
}else{
	//不更新
	if($arr_view['error']>=5){
		//跳过错误文章ID
		array_splice($arr_view['notyet'],0,1);
		$up=1;
	}
}
if($up==1){
	$con_view=serialize($arr_view);
	cache_put_contents($con_view,$cache_view);
}


//输出
out_view($arr_index,$arr_view,$domain,$base_url);

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
	$description=preg_replace("%<[^>]+>%",',',$arr_view['description']);
	$description=substr_utf8($description, 0, 50);
	$arr_rep['miaoshu']=$description;
	$arr_rep['base_url']=$base;
	$arr_rep['body']=$arr_view['body']."<br>"."您可能感兴趣：<br>".$arr_view['x_link'];
	if($arr_rep['body']==''){
		//如果尚未生成文章内容
		$arr_rep['body']='暂无数据，请等待。错误编码：'.md5($base_url);
	}
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
?>
