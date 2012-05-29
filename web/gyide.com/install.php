<?php
include "./common.inc.php";
include './mysql_db.php';

$db = new dbQuery($cfg_dbhost,$cfg_dbuser,$cfg_dbpwd,$cfg_dbname);

//$arr_host
foreach($cfg_arr_host as $host){
	$qz=str_replace('.','_',$host);
	$qz = $qz.'_';
	$sql1="drop table if exists `".$qz."cat`";
	$sql2="
		CREATE TABLE `".$qz."cat` (
		  `cid` int(9) NOT NULL auto_increment,
		  `title` varchar(200) NOT NULL,
		  `key` varchar(50) NOT NULL,
		  `keys` varchar(255) default NULL,
		  `epath` varchar(30) default NULL,
		  `flink` text default NULL,
		  `layer` tinyint(4) NOT NULL default '0',
		  `ad_key` varchar(50) NOT NULL,
		  PRIMARY KEY  (`cid`),
		  UNIQUE KEY `key` (`key`),
		  UNIQUE KEY `epath` (`epath`),
		  KEY `layer` (`layer`)
		) ENGINE=MyISAM DEFAULT CHARSET=utf8;";
		$db->query($sql1);
		$db->query($sql2);
		echo "create database -- $qz -- success.<br>";
}
//

$db->close();
//rename("install.php","install.php.bak");

echo "Create database success,<a href='./addkey.php?pwd=7698'>click here</a>to add keys!";
echo "<meta http-equiv=refresh content='0; url=./addkey.php?pwd=7698'>";
?>
