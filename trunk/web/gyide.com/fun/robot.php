<?php
function get_naps_bot()
{
	$useragent = strtolower($_SERVER['HTTP_USER_AGENT']);
	if (strpos($useragent, 'googlebot') != false){
		return 'Googlebot';
	}
	if (strpos($useragent, 'msnbot') != false){
		return 'MSNbot';
	}
	if (strpos($useragent, 'slurp') != false){
		return 'Yahoobot';
	}
	if (strpos($useragent, 'baiduspider') != false){
		return 'Baiduspider';
	}
	if (strpos($useragent, 'sohu-search') != false){
		return 'Sohubot';
	}
	if (strpos($useragent, 'lycos') != false){
		return 'Lycos';
	}
	if (strpos($useragent, 'robozilla') != false){
		return 'Robozilla';
	}
	if (strpos($useragent, 'sosospider') != false){
		return 'Sosospider';
	}
	return false;
}
function nowtime(){
	$date=date("Y-m-d.G:i:s");
	return $date;
}

function create_Dir($path){
	if (!file_exists($path)){
		create_Dir(dirname($path));
		mkdir($path, 0777);
	}
}

//获取二级域名
$host=$_SERVER["HTTP_HOST"];
$array_temp=explode(".",$host);
$len=count($array_temp);
$domain= $array_temp[$len-2].'.'.$array_temp[$len-1];


$searchbot = get_naps_bot();

if ($searchbot != false) {
	$tlc_thispage = addslashes($_SERVER['HTTP_USER_AGENT']);
	$url=$host.$_SERVER["REQUEST_URI"];
	$file="/var/seo/tk001/".date("Y-m-d")."/".$domain.'.txt';
	create_Dir(dirname($file));
	$time=nowtime();
	$data=fopen($file,"a");
	fwrite($data,"Time:$time	robot:$searchbot	URL:$url	\n");//AGENT:$tlc_thispage\n");
	fclose($data);
}
?>
