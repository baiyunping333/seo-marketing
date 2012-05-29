<?php
set_time_limit(60);
header("Content-type:text/html; charset=utf-8");

/*=============================================
 *
 *	获取基础参数
 *	$domain:当前主域名
 *	$ename：当前二级域名，包含'.'
 *	$path：当前路径
 *	$base_url:当前路径，包含最后一个'/'
 *	$cfg_dbqz：表前缀
 *	$epath:二级域名+路径，用以确定cache位置md5()或者搜索数据库
 *	$maxid:记录数
 *	$create: 创建日期
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
 * 包含库文件
 */
include "./common.inc.php";
include "./mysql_db.php";
include "./fun.php";

/*
 * 获取maxid及首次更新时间
 */
if(file_exists($cfg_cache_path.'/cfg')){
	$con=file_get_contents($cfg_cache_path.'/cfg');
	$arr_con=unserialize($con);
	$maxid=$arr_con['maxid'];
	$create=$arr_con['create'];
}else{
	$db = new dbQuery($cfg_dbhost,$cfg_dbuser,$cfg_dbpwd,$cfg_dbname);
	$sqlx="SELECT MAX(cid) as maxcid FROM `".$cfg_dbqz."cat`;";
	$f_result = $db->query($sqlx);
	$f_row = $db->fetch_array($f_result);
	$maxid=$f_row['maxcid'];
	$db->close();
	$create=time();
	$arr_con=array('maxid'=>$maxid,'create'=>$create);
	$con=serialize($arr_con);
	cache_put_contents($con,$cfg_cache_path.'/cfg');
}
/*==============================
 *
 *cache存在
 *
 ==============================*/
$cache_path = cache_get_path($epath,$cfg_cache_path.'index/');
if(file_exists($cache_path)){
	/*
	 * 读取参数
	 * $arr_cache['cid']
	 * $arr_cache['key']
	 * $arr_cache['keys']
	 * $arr_cache['title']
	 * $arr_cache['flink']		*
	 * $arr_cache['adkey']
	 * $arr_cache['layer']
	 * $arr_cache['l_up']
	 * $arr_cache['uptimes']
	 * $arr_cache['tag_key']	*
	 * $arr_cache['rid']		*
	 * $arr_cache['b_pg']		*
	 * $arr_cache['ov_title']	*
	 * $arr_cache['ov_des']		*
	 * $arr_cache['rnid']		*
	 * $arr_cache['rn_title']	*
	 * $arr_cache['rn_des']		*
	 * $arr_cache['riid']		*
	 * $arr_cache['ri_title']	*
	 * $arr_cache['ri_des']		*
	 */
	$cache_con=file_get_contents($cache_path);
	$arr_cache=unserialize($cache_con);
	if(time()-$arr_cache['l_up']>=$arr_cache['uptimes']){
		/*========================================
		 * 更新
		 *=======================================*/

		/*
		 * 生成TAG
		 * 包含
		 * $tag_key:随机生成的关键词列表，需要写入cache
		 * $max_id：tag中出现最大ID。一般以一级关键词数量为准.
		 */
		$max_id = 2000;
		$dayup=(int)((time()-$create)/86400*3)+1;
		//每3日增加ID一次
		//前4个关键词正常更新（12天）
		$this_maxid = $dayup;
		//5倍更新	$dayup 5-10 15-30天	 34个关键词
		if ($dayup>4){
			$this_maxid=4+($dayup-4)*5;
		}
		//40倍更新	$dayup 11-20 33-60天  434个关键词
		if ($dayup>10){
			$this_maxid= 34+($dayup-10)*40;
		}
		//100倍更新 $dayup 20+ 63天+ 剩余关键词 46+60=106天左右达到最大值
		if ($dayup>20){
			$this_max_id=434+($dayup-20)*100;
		}
		if ($this_maxid<=$max_id){
			$randmax=$this_maxid;
		}else{
			$randmax=$max_id;
		}
		$db = new dbQuery($cfg_dbhost,$cfg_dbuser,$cfg_dbpwd,$cfg_dbname);
		$sql="SELECT `flink` FROM `".$cfg_dbqz."cat` WHERE `cid`=".$arr_cache['cid'];
		$f_result = $db->query($sql);
		$f_row = $db->fetch_array($f_result);
		$arr_cache['flink']=$f_row['flink'];
		//确定tag个数。超过10个只显示10个
		if( $randmax >=10 ){
			$this_max = 10;
		}else{
			$this_max = $randmax;
		}
		for($ii=0;$ii<$this_max;$ii++){
			$sql = "SELECT `key` FROM `".$cfg_dbqz."cat` WHERE `cid`=".rand(1,$randmax);
			$f_result = $db->query($sql);
			$f_row = $db->fetch_array($f_result);
			$e_key=$f_row['key'];
			$arr_randkey[$ii]=$e_key;
		}
		$db->close();
		$tag_key=implode(",",$arr_randkey);	//随机tag的key*

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
		$rid=rand(1,10);
		$r_url=cache_get_path($epath.$rid,$cfg_cache_path.'view/');
		if(file_exists($r_url)){
			/*
			 * cache存在，直接读取
			 */
			$view_con=file_get_contents($r_url);
			$arr_view_con=unserialize($view_con);
			$ov_title=$arr_view_con['atitle'];
			$ov_des=$arr_view_con['description'];
		}elseif($error_view<=10){//错误次数超过10次，不再采集
			/*
			 * cache不存在，触发采集
			 * $error_view:错误次数,cache
			 */
			$bid = get_hot($key,$rid-1); //返回结果集
			$bids = $bid['ids'];
			$titles = $bid['titles'];
			$descriptions = $bid['descriptions'];
			/*
			 * 输出到缓存
			 */
			if(count($bids)>0){
				$this_bid=implode(',',$bids);
				$this_atitle=strip_tags($titles[0]);
				shuffle($descriptions);
				$this_description=strip_tags(implode('<br>',$descriptions));
				$this_description=substr_utf8($this_description,0,500);
				/*
				 * 写入
				 */
				$arr_view_con=array();
				$arr_view_con['bid']=$this_bid;
				$arr_view_con['notyet']=$bids;
				$arr_view_con['error']=0;
				$arr_view_con['atitle']=$this_atitle;
				$arr_view_con['description']=$this_description;
				$view_con=serialize($arr_view_con);
				cache_put_contents($view_con,cache_get_path($epath.$rid,$cfg_cache_path.'view/'));
				$ov_title=$arr_view_con['atitle'];
				$ov_des=$arr_view_con['description'];
			}else{
				$error_view+=1;
			}
		}

		/*
		 * 随机文章--分词打乱部分
		 * cache路径：$base_url.'read-'.$rnid
		 * url路径:/path/read-$rnid.html
		 * $rnid:随机1-9999999的数字，用以确定采用哪条数据,cache*
		 * $rn_url:由$rnid获取cache路径
		 * rn_title:输出标题*
		 * rn_des:输出描述*
		 */
		$rnid=rand(1,9999999);
		$arr_temp=array();
		$arr_temp=randart($key,$keys,$rnid,$base_url);
		$rn_title=$arr_temp['title'];
		$rn_des=$arr_temp['des'];

		/*
		 * 随机文章--分词不打乱随机插入部分
		 * cache路径：$base_url.'reads-'.$rnid
		 * url路径:/path/reads-$rnid.html
		 * $riid:随机1-9999999的数字，用以确定采用哪条数据,cache*
		 * $ri_url:由$rnid获取cache路径
		 * ri_title:输出标题*
		 * ri_des:输出描述*
		 */
		$riid=rand(1,9999999);
		$arr_temp=array();
		$arr_temp=insertart($key,$keys,$riid,$base_url);
		$ri_title=$arr_temp['title'];
		$ri_des=$arr_temp['des'];
		/*
		 * 更新数据
		 */
		$arr_cache['tag_key']=$tag_key;
		$arr_cache['rid']=$rid;
		$arr_cache['b_pg']=(int)$b_pg;
		$arr_cache['ov_title']=$ov_title;
		$arr_cache['ov_des']=$ov_des;
		$arr_cache['rnid']=$rnid;
		$arr_cache['rn_title']=$rn_title;
		$arr_cache['rn_des']=$rn_des;
		$arr_cache['riid']=$riid;
		$arr_cache['ri_title']=$ri_title;
		$arr_cache['ri_des']=$ri_des;
		$cache_con=serialize($arr_cache);
		cache_put_contents($cache_con,$cache_path);
	}
	out_index($arr_cache,$domain,$base_url);
	die();
}


/*===============================
 *
 * cache不存在，初始化生成
 *
 ===============================*/

$db = new dbQuery($cfg_dbhost,$cfg_dbuser,$cfg_dbpwd,$cfg_dbname);
$sql = "select * from `".$cfg_dbqz."cat` where `epath`= '$epath'" ;
$result = $db->query($sql);
$row = $db->fetch_array($result);
/*
 * 错误页面
 */
if($db -> affected_rows()==0){
	for($ii=0;$ii<50;$ii++){
		$sql = "SELECT `key`,`epath` FROM `".$cfg_dbqz."cat` WHERE `cid`=".rand(1,$maxid);
		$f_result = $db->query($sql);
		$f_row = $db->fetch_array($f_result);
		$e_key=$f_row['key'];
		$e_epath=$f_row['epath'];// --- /xxx/格式或者/
		$arr_temp=explode("/",$e_epath);
		$e_ename=$arr_temp[0];
		$e_path=$arr_temp[1];
		echo '<li><a href="http://'. $e_ename.$domain.'/'.$e_path.'">'. $e_key .'</a></li> ';
	}
	$db->close();
	die();
}
/*
 * 错误页面结束
 *
 * 生成内容，带*的要记录在cache内已备读取
 */

$cid = $row['cid'];				//商品ID
$key = $row['key'];				//商品关键词*
$keys = $row['keys'];			//相关关键词*
$title = $row['title'];			//标题*
$flink = $row['flink'];			//互连部分*
$adkey = $row['ad_key'];		//广告关键词*
$layer = $row['layer'];			//层数*
$l_up = time();					//上次更新日期（其实就是这次更新日期)*
switch($layer){					//更新频率*
	case 0:
		$up_times=86400;
		break;
	case 1:
		$up_times=86400*3;
		break;
	case 2:
		$up_times=86400*5;
	case 3:
		$up_times=86400*7;
}

/*
 * 生成TAG
 * 包含
 * $tag_key:随机生成的关键词列表，需要写入cache
 * $max_id：tag中出现最大ID。一般以一级关键词数量为准.
 */
$max_id = 2000;
$this_maxid=(int)((time()-$create)/86400*3)+1;//每3日增加ID一次
if ($this_maxid<=$max_id){
	$randmax=$this_maxid;
}else{
	$randmax=$max_id;
}
if( $randmax >= 10 ){
	$this_max = 10;
}else{
	$this_max = $randmax;
}
for($ii=0;$ii<$this_max;$ii++){
	$sql = "SELECT `key` FROM `".$cfg_dbqz."cat` WHERE `cid`=".rand(1,$randmax);
	$f_result = $db->query($sql);
	$f_row = $db->fetch_array($f_result);
	$e_key=$f_row['key'];
	$arr_randkey[$ii]=$e_key;
}
$tag_key=implode(",",$arr_randkey);	//随机tag的key*
$db->close();

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
$rid=rand(1,10);
$r_url=cache_get_path($epath.$rid,$cfg_cache_path.'view/');
if(file_exists($r_url)){
	/*
	 * cache存在，直接读取
	 */
	$view_con=file_get_contents($r_url);
	$arr_view_con=unserialize($view_con);
	$ov_title=$arr_view_con['atitle'];
	$ov_des=$arr_view_con['description'];
}elseif($error_view<=10){//错误次数超过10次，不再采集
	/*
	 * cache不存在，触发采集
	 * $error_view:错误次数,cache
	 */
	$bid = get_hot($key,$rid-1); //返回结果集
	$bids = $bid['ids'];
	$titles = $bid['titles'];
	$descriptions = $bid['descriptions'];
	/*
	 * 输出到缓存
	 */
	if(count($bids)>0){
		$this_bid=implode(',',$bids);
		$this_atitle=strip_tags($titles[0]);
		shuffle($descriptions);
		$this_description=strip_tags(implode('<br>',$descriptions));
		$this_description=substr_utf8($this_description,0,500);
		/*
		 * 写入
		 */
		$arr_view_con=array();
		$arr_view_con['bid']=$this_bid;
		$arr_view_con['notyet']=$bids;
		$arr_view_con['atitle']=$this_atitle;
		$arr_view_con['description']=$this_description;
		$view_con=serialize($arr_view_con);
		cache_put_contents($view_con,cache_get_path($epath.$rid,$cfg_cache_path.'view/'));
		$ov_title=$arr_view_con['atitle'];
		$ov_des=$arr_view_con['description'];
	}else{
		$error_view+=1;
	}
}

/*
 * 随机文章--分词打乱部分
 * cache路径：$base_url.'read-'.$rnid
 * url路径:/path/read-$rnid.html
 * $rnid:随机1-9999999的数字，用以确定采用哪条数据,cache*
 * $rn_url:由$rnid获取cache路径
 * rn_title:输出标题*
 * rn_des:输出描述*
 */
$rnid=rand(1,9999999);
$arr_temp=array();
$arr_temp=randart($key,$keys,$rnid,$base_url);
$rn_title=$arr_temp['title'];
$rn_des=$arr_temp['des'];

/*
 * 随机文章--分词不打乱随机插入部分
 * cache路径：$base_url.'reads-'.$rnid
 * url路径:/path/reads-$rnid.html
 * $riid:随机1-9999999的数字，用以确定采用哪条数据,cache*
 * $ri_url:由$rnid获取cache路径
 * ri_title:输出标题*
 * ri_des:输出描述*
 */
$riid=rand(1,9999999);
$arr_temp=array();
$arr_temp=insertart($key,$keys,$riid,$base_url);
$ri_title=$arr_temp['title'];
$ri_des=$arr_temp['des'];

/*
 * 保存cache
 */
$arr_cache=array();
$arr_cache['cid']=$cid;
$arr_cache['key']=$key;
$arr_cache['keys']=$keys;
$arr_cache['title']=$title;
$arr_cache['flink']=$flink;
$arr_cache['adkey']=$adkey;
$arr_cache['layer']=$layer;
$arr_cache['l_up']=$l_up;
$arr_cache['uptimes']=$up_times;
$arr_cache['tag_key']=$tag_key;
$arr_cache['rid']=$rid;
$arr_cache['b_pg']=(int)$b_pg;
$arr_cache['ov_title']=$ov_title;
$arr_cache['ov_des']=$ov_des;
$arr_cache['rnid']=$rnid;
$arr_cache['rn_title']=$rn_title;
$arr_cache['rn_des']=$rn_des;
$arr_cache['riid']=$riid;
$arr_cache['ri_title']=$ri_title;
$arr_cache['ri_des']=$ri_des;
$cache_con=serialize($arr_cache);
cache_put_contents($cache_con,$cache_path);
out_index($arr_cache,$domain,$base_url);
/*===============================
 *
 * 主函数结束
 *
 ===============================*/



/*
 * 随机文章--分词不打乱随机插入部分
 * $keys：相关关键词
 * $a_no：1-99999999中的一个随机数，确定路径
 * $base_url:url路径
 * 返回$arr_res[]包含title，des。
 */
function insertart($key,$keys,$a_no,$base_url){
	global $cfg_xs_url;
	$url='http://'.$base_url.'reads-'.$a_no.'html';
	$md5_url=md5($url);
	/*
	 * 生成指纹序列
	 * $art_sp:文章指纹序列
	 * $choas_sp:乱序指纹序列
	 */
	for($i=0;$i<5;$i++){//文章指纹序列
		$art_sp[$i]=toTen(substr($md5_url,$i*4,4))+1;
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
	 * 选取第一个文章。
	 * 按指纹插入关键词。
	 */
	$title=file_get_contents($cfg_xs_url.$art_sp[0].".txt");
	$arr_title=explode("|",$title);
	$arr_key=array($key,$key2);
	$title=arr_insertkey($arr_title,$choas_sp,$arr_key,1);
	$title=preg_replace("%<[^>]+>%",',',$title);//防止网址中字母被匹配
	$title=str_replace(array(',','。','：','”','“','！','……','？','；',' ','，'),'',$title);
	$title=replace_db($title,'');
	$title = substr_utf8($title, 0, 20);
	/*
	 * 确定描述
	 * 选取5个文章。
	 * 按指纹插入关键词。
	 */
	$des='';
	for($i=0;$i<5;$i++){
		$temp=file_get_contents($cfg_xs_url.$art_sp[$i].".txt");
		$des.=$temp;
	}
	$arr_des=explode('|',$des);
	$des=arr_insertkey($arr_des,$choas_sp,$arr_key,3);
	$des=preg_replace("%<[^>]+>%",',',$des);//防止网址中字母被匹配
	$des=replace_dbs($des,'.');
	$des=str_replace(' ','',$des);
	$des=substr_utf8($des,0,500);
	/*
	 * 返回
	 * $arr_res[]
	 */
	$arr_res=array();
	$arr_res['title']=$title;
	$arr_res['des']=$des;
	return $arr_res;
}

/*
 * 随机文章--分词打乱部分
 * $key:关键词
 * $keys：相关关键词
 * $a_no：1-99999999中的一个随机数，确定路径
 * $base_url:url路径
 * 返回$arr_res[]包含title，des。
 */
function randart($key,$keys,$a_no,$base_url){
	global $cfg_xs_url;
	$url='http://'.$base_url.'read-'.$a_no.'html';
	$md5_url=md5($url);
	/*
	 * 生成指纹序列
	 * $art_sp:文章指纹序列
	 * $choas_sp:乱序指纹序列
	 */
	for($i=0;$i<5;$i++){//标题指纹序列
		$art_sp[$i]=toTen(substr($md5_url,$i*4,4))+1;
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
		$des.=$temp;
	}
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
	 *  确定描述
	 */
	if(rand(0,10)>7){
		$arr_des=explode('|',$des.'|'.$key2.'|'.$key);
	}else{
		$arr_des=explode('|',$des.'|'.$key);
	}
	shuffle($arr_des);
	$des=implode('',$arr_des);
	$des=replace_dbs($des,'.');
	$des=substr_utf8($des,0,500);

	/*
	 * 返回
	 * $arr_res[]
	 */
	$arr_res=array();
	$arr_res['title']=$title;
	$arr_res['des']=$des;
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
/*
 * 按指纹序列插入关键词组合成文章
 * $arr_str:待插入数组
 * $choas_sp:指纹序列
 * $arr_key:关键词数组
 * $res:返回文本
 * $quan:权值
 */
function arr_insertkey($arr_str,$choas_sp,$arr_key,$quan){
	$arr=$arr_str;
	$arrlen=count($arr);
	$len=count($choas_sp);
	$len_key=count($arr_key);
	$x=0;//用以记录指纹序列当前位置
	$res='';
	$no=$quan*$choas_sp[$x++]+5;				//间隔多少个词组
	$sp=0;
	for($i=0;$i<$arrlen;$i++){			//指纹序列用光，循环取用
		if($x>=$len){
			$x=0;
			$no=$quan*$choas_sp[$x++]+5;
		}
		if($sp==$no){					//到达间隔标准
			$res.=$arr_key[($no+$quan*$len_key)%$len_key].$arr_str[$i];	//插入关键词
			$no=$quan*$choas_sp[$x++]+5;		//下一个指纹
			$sp=0;
		}else{
			$res.=$arr_str[$i];
			$sp++;
		}
	}
	return $res;
}
/*
 * 输出函数
 * $arr:参数
 * $domain:域名
 * $arr_cache['cid']
 * $arr_cache['key']
 * $arr_cache['keys']
 * $arr_cache['title']
 * $arr_cache['flink']
 * $arr_cache['adkey']
 * $arr_cache['layer']
 * $arr_cache['l_up']
 * $arr_cache['uptimes']
 * $arr_cache['tag_key']	*
 * $arr_cache['rid']		*
 * $arr_cache['b_pg']		*
 * $arr_cache['ov_title']	*
 * $arr_cache['ov_des']		*
 * $arr_cache['rnid']		*
 * $arr_cache['rn_title']	*
 * $arr_cache['rn_des']		*
 * $arr_cache['riid']		*
 * $arr_cache['ri_title']	*
 * $arr_cache['ri_des']		*
 */
function out_index($arr,$domain,$base_url){
	global $cfg_tk_pid;
	$con=file_get_contents("./templets/index.html");
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
	/*
	 * 替换数组
	 */
	$arr_rep=array();
	$arr_rep['key']=$arr['key'];
	$arr_rep['keys']=$arr['keys'];
	$arr_rep['title']=$arr['title'];
	$arr_rep['adkey']=$arr['adkey'];
	$arr_rep['adkey_code']=urlencode($arr['adkey']);
	$arr_rep['adkey_iconv']=urlencode(iconv("UTF-8", "GBK//IGNORE", $arr['adkey']));
	$arr_rep['pid']=$cfg_tk_pid;
	$arr_rep['keys_link']=$key_link;
	$arr_rep['tag_link']=$tag_link;
	$arr_rep['ov_url']='http://'.$base_url.'view-'.$arr['rid'].'.html';
	$arr_rep['ov_title']=$arr['ov_title'];
	$arr_rep['ov_des']=$arr['ov_des'];
	$arr_rep['rn_url']='http://'.$base_url.'read-'.$arr['rnid'].'.html';
	$arr_rep['rn_title']=$arr['rn_title'];
	$arr_rep['rn_des']=$arr['rn_des'];
	$arr_rep['ri_url']='http://'.$base_url.'reads-'.$arr['riid'].'.html';
	$arr_rep['ri_title']=$arr['ri_title'];
	$arr_rep['ri_des']=$arr['ri_des'];
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
