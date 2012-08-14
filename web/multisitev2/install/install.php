<?php
include_once "../config.php";
include_once '../class/mysql_db.php';
include_once '../class/main.php';

$cfg=new config();
$db=new dbQuery($cfg->dbHost,$cfg->dbUser,$cfg->dbPassword,$cfg->dbName);
$info=new urlInfo();

$db->query("drop table if exists `".$info->dbPrefix."cat`");

$sqlCreate="
		CREATE TABLE `".$info->dbPrefix."cat` (
		  `cid` int(9) NOT NULL auto_increment,
		  `title` varchar(200) NOT NULL,
		  `key` varchar(50) NOT NULL,
		  `keys` varchar(255) default NULL,
		  `epath` varchar(30) default NULL,
		  `adkey` varchar(50) default NULL,
		  `index` TEXT default NULL,
		  `baidu1` TEXT default NULL,
		  `baidu2` TEXT default NULL,
		  `baidu3` TEXT default NULL,
		  `baidu4` TEXT default NULL,
		  `baidu5` TEXT default NULL,
		  `baidu6` TEXT default NULL,
		  `baidu7` TEXT default NULL,
		  `baidu8` TEXT default NULL,
		  `baidu9` TEXT default NULL,
		  `baidu10` TEXT default NULL,
		  PRIMARY KEY  (`cid`),
		  UNIQUE KEY `key` (`key`),
		  UNIQUE KEY `epath` (`epath`)
		) ENGINE=myisam DEFAULT CHARSET=utf8;";
$db->query($sqlCreate);
$db->close();
echo "Create database success,<a href='./addkey.php?pwd=7698'>click here</a>to add keys!";
echo "<meta http-equiv=refresh content='0; url=./addkey.php?pwd=7698'>";
?>
