<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<?php
	set_time_limit(0);

if(isset($_GET['pwd'])&&$_GET['pwd'] == "7698"){
    include_once "../config.php";
    include_once '../class/mysql_db.php';
    include_once '../class/main.php';
    include_once '../function/function.php';

    $cfg=new config();
    $db=new dbQuery($cfg->dbHost,$cfg->dbUser,$cfg->dbPassword,$cfg->dbName);
    $info=new urlInfo();

	$file_name="../keys.txt";
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
	echo "<br>锁表<br>";
	$db->query('LOCK TABLES `'.$info->dbPrefix.'cat` WRITE;');//锁表
	$insertToDB_sql='INSERT ignore  INTO `'.$info->dbPrefix.'cat`(`key`,`title`,`keys`,`epath`, `adkey`) VALUES ';

	while(!feof($fp)){
		$buffer=fgets($fp,4096);
		$buffer=str_replace("'","\'",$buffer);
        $buffer=str_replace($space,'',$buffer);

		//分割关键词
        //自动跳过空行
        if($buffer==''){continue;}
		$arr=explode('|',$buffer);
		$key=$arr[0];
		$keys=$arr[1];
		$adKey=$arr[2];
		//自动跳过空行
		if($key==''){continue;}

        $insertToDB_sql.=Insert($key,$keys,$adKey);
		//断点安装，避免php运行时间过长超时

		$xx++;
		$x++;

		if($xx>1000){
            $insertToDB_sql=str_replace(',xbakfffeeee','',$insertToDB_sql.'xbakfffeeee');
            $insertToDB_sql=str_replace('xbakfffeeee','',$insertToDB_sql).';';
			$db->query($insertToDB_sql);
			echo "<br>解锁<br>";
			$db->query('UNLOCK TABLES;');//解锁

			$url="./addkey.php?pwd=7698&x=$x";
			echo "<meta http-equiv=refresh content='0; url=$url'>";
            die();
		}
	}

	$insertToDB_sql=str_replace(',xbakfffeeee','',$insertToDB_sql.'xbakfffeeee');
    $insertToDB_sql=str_replace('xbakfffeeee','',$insertToDB_sql).';';
	$db->query($insertToDB_sql);
	echo "<br>解锁<br>";
	$db->query('UNLOCK TABLES;');//解锁
	echo"<H1><font color='red'>Add keys done!($x)</a></H1>";

	$db -> close();

	$next_site = $cfg->nextDomain;
	if($next_site != ''){
		echo "<meta http-equiv=refresh content='0; url=http://www.".$cfg->nextDomain."/install/install.php'>";
	}

	//将关键词数及生成日期写入
	$arrCreate=array('maxid'=>$x-1,'create'=>time());
	$con=serialize($arrCreate);
	CachePutContents($con,'../cfg');
	unset($con);
	unset($arrCreate);

}else{
	echo "Please input password.";

}

/**
 * 返回插入语句
 * @param $key
 * @param $keys
 * @param $adKey
 * @return string
 */
function Insert($key,$keys,$adKey){
    $epath=GetEPath($key);
    //生成标题
    $arr_keys=explode(',',$key);
    shuffle($arr_keys);
    $arrKeys=array_slice($arr_keys,0,2);
    $title=$key.'_'.implode('_',$arrKeys);
    $sql="('$key','$title','$keys','$epath', $adKey),";
    return $sql;
}

?>
