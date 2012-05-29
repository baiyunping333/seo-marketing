?<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<?php
	set_time_limit(0);

if(isset($_GET['pwd'])&&$_GET['pwd'] == "7698"){
	include "./mysql_db.php";
	include "./common.inc.php";

	// 依次为：IP 用户名 密码 数据库
	$db = new dbQuery($cfg_dbhost, $cfg_dbuser, $cfg_dbpwd, $cfg_dbname);

	$file_name="./keys.txt";
	$fp=fopen($file_name,'r');
	$space = array("\r\n", "\n", "\r");
	//移到指定文件行
	$x=isset($_GET['x'])?$_GET['x']:0;
	echo $x;
	for($i=1;$i<$x;$i++){
		fgets($fp,4096);
	}
	$xx=0;
	echo "<br>begin<br>";
	while(!feof($fp)){
		$buffer=fgets($fp,4096);
		$buffer=str_replace("'","\'",$buffer);
		$this_key=str_replace($space,'',$buffer);

		//分割关键词及layer
		$arr_temp=explode('|',$this_key);
		$this_key=$arr_temp[0];
		$this_keys=$arr_temp[1];
		$this_adkey=$arr_temp[2];
		$this_layer=$arr_temp[3];
		//自动跳过空行
		if($this_key==''){continue;}

		//入库
		$this_host=$cfg_arr_host[rand(0,count($cfg_arr_host)-1)];
		//$this_ename=keyToEname($this_key,$cfg_arr_ename);
		$cfg_dbqz=str_replace('.','_',$this_host).'_'; //表前缀
		$this_sql = "SELECT MAX(`cid`) as ide FROM `".$cfg_dbqz."cat`";//查找cid最大值
		$result = $db -> query($this_sql);
		$tt = $db -> fetch_array($result);
		$maxid = $tt['ide'];
		$index = $maxid+1;
		$bn = '0';
		//insert($this_key,$this_keys,$this_ename,$this_adkey,$this_layer);
		insert($this_key,$this_keys,$this_adkey,$this_layer);
		//断点安装，避免php运行时间过长超时
		$xx++;
		echo $x++.':    '.$this_key.'done!<br>';//$x自加
		if($xx>2000){
			$url="./addkey.php?pwd=7698&x=$x";
			echo "<meta http-equiv=refresh content='0; url=$url'>";
			die();
		}
	}

	echo"<H1><font color='red'>Add keys done!($x)</a></H1>";
	//echo"<H1><font color='red'><a href='./autoflinks.php?pwd=7698'>Click here</a> to create links.</H1>";
	$db -> close();
	//rename("keys.txt","keys.bak.php");
	$next_site = 'http://www.ykjhs.com/install.php';
	if($next_site != 'xxx'){
		echo "<meta http-equiv=refresh content='0; url=http://www.ykjhs.com/install.php'>";
	}
	
	/*
	 * 将关键词数及生成日期写入
	 */
	$arr_con=array('maxid'=>$x-1,'create'=>time());
	$con=serialize($arr_con);
	cache_put_contents($con,$cfg_cache_path.'/cfg');
	unset($con);
	unset($arr_con);
}else{
	echo "Please input password.";

}





// 公共函数

function Toutf($str) {
	return iconv("GBK", "utf-8", $str);
}

// 插入关键词
function insert($t_key,$t_keys,$t_adkey,$t_layer){
	global $db;
	global $bn;
	global $index;
	global $cfg_dbqz;
	$epath=get_epath($t_key);
	//生成标题
	$arr_keys=explode(',',$t_keys);
	shuffle($arr_keys);
	$arr_keys=array_slice($arr_keys,0,2);
	$title=$t_key.'_'.implode('_',$arr_keys);
	$sql = "INSERT ignore  INTO `".$cfg_dbqz."cat`(`key`,`title`,`keys`,`epath`, `ad_key`, `layer`) VALUES ('$t_key','$title','$t_keys','$epath', '$t_adkey', $t_layer);";
	$db->query($sql);
	$x=$db->affected_rows();
	$bn++;
	if($bn==5){
		echo "<br>-------------error:$t_key-------------<br>";
		$x=1;
	}//连续5词获取不到唯一id则跳过
	if($x!=1){insert($t_key,$t_keys,$t_adkey,$t_layer);}
}
//得到路径
function get_epath($key){
	global $cfg_arr_mainkey;	//主关键词
	global $cfg_arr_ename;		//二级域名组
	//判断是否主关键词
	if($cfg_arr_mainkey[$key]>0){//是主关键词
		$epath=$cfg_arr_ename[$cfg_arr_mainkey[$key]-1].'/';
		echo "<br>-------<br>".$epath."<br>-------<br>";
	}else{
		$t_ename=keyToEname($key,$cfg_arr_ename);
		$epath=$t_ename.'/'.md5To36(md5($key),8,10).'/';
	}
	return $epath;
}

//16进制到10进制
function toTen($num){
	$arr_key=array('0'=>0,'1'=>1,'2'=>2,'3'=>3,'4'=>4,'5'=>5,'6'=>6,'7'=>7,'8'=>8,'9'=>9,'a'=>10,'b'=>11,'c'=>12,'d'=>13,'e'=>14,'f'=>15);
	$result=0;
	for($i=0;$i<strlen($num);$i++){
		$result+=$arr_key[ substr($num,$i,1)]*pow(16,strlen($num)-$i-1);
	}
	return $result;
}

//按关键词得到二级域名(不同关键词对应不同的二级域名了)
function keyToEname($t_key,$t_arr_ename){
	$n=count($t_arr_ename);
	$n_key=toTen(substr(md5($t_key),0,2));//按关键词获取特征码
	$mod_key=$n_key % $n;//求余，按余数来确定是用二级域名数组中的哪个
	return $t_arr_ename[$mod_key];
}

//10进制转36进制
function md5To36($num,$start_no,$len){
	$arr_o=array('0','1','2','3','4','5','6','7','8','9','a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z');
	$o_n=toTen(substr($num,$start_no,$len));
	$r_no='';
	do{
		$a=ceil($o_n/36);
		$b=fmod(floatval($o_n),36);
		$o_n=$a;
		$r_no=$arr_o[$b].$r_no;
	}while($o_n>36);
	return $arr_o[$o_n].$r_no;
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
?>
