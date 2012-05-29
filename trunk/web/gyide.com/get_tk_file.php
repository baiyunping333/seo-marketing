<?php
header("Content-type:text/html; charset=utf-8");

$taoke_pid='mm_15557716_0_0';		//淘客pid
$cache_time = 3600*24*7 ;   //缓存时间 单位:秒
$tnum = 12 ;				//商品数量
$cfg_ad_path = '/var/www/api/' ;	//1，目录以/结尾 2，目录要存在 3，目录要在php读写路径内 4，如果是linux系统要有写权限
/**
 * 调用方式 <script src='/get_tk.php?id=xxxx'></script>
 * BY：SeoLei 未经允许 请勿公开提供下载
*/


if(isset($_GET['id'])){$id = $_GET['id'];}elseif(isset($_GET['k'])){$id = $_GET['k'];}
$key=urldecode($id);
if($key==''){$key='丰胸';}
$arr=explode('_',$key);
$key=$arr[0];
$filename=cache_get_path($key,$cfg_ad_path);
if(file_exists($filename)){
	include($filename);
	die();
}else{
	$taobaoke=get_taoke($key,$taoke_pid,$tnum);
	$data=get_content($taobaoke);
	$data=str_replace('/','\/',$data);
	$data=str_replace("'","\\'",$data);
	$data="document.writeln('$data');";
	echo $data;
	$data.="<?php if(time()-".time().">$cache_time){unlink('$cache_path$filename');}?>";
	cache_put_contents($data,$filename);
}
//根据模板输出格式
function get_content($str){
	//echo $str;
	$arr=unserialize($str);
	$data='';
	for($i=0;$i<count($arr);$i++){
		$data.='<li><div class="pros"><ul><li class="imgs"><a href="'.$arr[$i]['click_url'].'"><img src="'.$arr[$i]['pic_url'].'_310x310.jpg"/></a></li><li class="tits"><a href="'.$arr[$i]['click_url'].'">'.$arr[$i]['title'].'</a> </li><li class="pris"><font class="n">'.$arr[$i]['price'].'</font>元</li></ul></div></li>';
	}
	return '<ul>'.$data.'</ul>';
}
//淘客内容
function get_taoke($key,$taoke_id,$tnum){
	$key = iconv("UTF-8", "GBK//IGNORE", $key);
	$url = 'http://s8.taobao.com/search?q='.urlencode($key).'&style=grid&unid=0&mode=63&pid='.$taoke_id.'&s=0&sort=sale-desc';
	if(function_exists("curl_init")) {
		$ch = curl_init();
		curl_setopt($ch, CURLOPT_URL, $url);
		curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
		curl_setopt($ch, CURLOPT_HEADER, 0);
		$data = curl_exec($ch);
		curl_close($ch);
	} else {
		$data=file_get_contents($url);
	}

	$data = iconv("GBK", "UTF-8",$data);
	$data_arr=array();
	//'%<h3 class="summary"><a[^>]+href="([^\"]+?)"[\s\S]+?title="([\s\S]+?)"[\s\S]+?<img data-ks-lazyload="([^>]+?)_[0-9xb]+\.jpg"[\s\S]+?<li class="price"><em>([0-9\.]+?)</em> <span>([^<]*)</span></li>%'
	if(preg_match_all('%<h3 class="summary"><a[^>]+href="([^\"]+?)"[\s\S]+?title="([\s\S]+?)"[\s\S]+?<img data-ks-lazyload="([^>]+?)_[0-9xb]+\.jpg"[\s\S]+?<li class="price"><em>([0-9\.]+?)</em> <span[ ]*>([^<]*)</span></li>%',$data,$arrTemp)){
		$all=count($arrTemp[1]);
		$len=($all<$tnum)?$all:$tnum;
		$rand_array=range(0,$all-1);
		shuffle($rand_array);
		$temp=array_slice($rand_array,0,$len);
			for($i=0;$i<$len;$i++){
				$j=$temp[$i];
				$data_arr[$i]['click_url']=$arrTemp[1][$j];
				$data_arr[$i]['title']=$arrTemp[2][$j];
				$data_arr[$i]['pic_url']=$arrTemp[3][$j];
				$data_arr[$i]['price']=$arrTemp[4][$j];
				$data_arr[$i]['sell']=$arrTemp[5][$j]!=''?$arrTemp[5][$j]:'最近成交1笔';
			}
			unset($arrTemp,$data);
	}
	//print_r($data_arr);die();
	return serialize($data_arr);
}
//存储
function cache_put_contents($body,$path){
	createDir(dirname($path));
	$handle=fopen($path,'w');
	fwrite($handle,$body);
	fclose($handle);
}
function createDir($path){
	if (!file_exists($path)){
		createDir(dirname($path));
		mkdir($path, 0777);
	}
}
//存储目录
function cache_get_path($str,$path){
	$str=MD5($str);
	$path.=substr($str,0,2).'/';
	$path.=substr($str,2,2).'/';
	$path.=substr($str,4,2).'/';
	$path.=substr($str,6,28);
	return $path;
}
?>