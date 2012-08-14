<?php
/**
 * Created by JetBrains PhpStorm.
 * User: ori
 * Date: 12-8-13
 * Time: 下午7:50
 * To change this template use File | Settings | File Templates.
 */
header("Content-type:text/html; charset=utf-8");
error_reporting(E_ALL & ~ E_NOTICE);
ini_set("magic_quotes_runtime",0);
include_once('./class/mysql_db.php');
include_once('./config.php');
include_once('./class/baidu.php');
include_once('./class/main.php');
include_once('./function/function.php');
$cfg=new config();
$db=new dbQuery($cfg->dbHost,$cfg->dbUser,$cfg->dbPassword,$cfg->dbName);
$indexCache=new indexContent();
$viewID=InStr('view-','.html',$_SERVER["REQUEST_URI"]);	//当前百度问答采集页面ID。范围从1-10。对应baidu1-baidu10
//--------------------从数据库中读取-------------------------------------
$sqlSelect='SELECT `cid`,`title`,`key`,`keys`,`adkey`,`index`,`baidu'.$viewID.'` FROM `'.$indexCache->dbPrefix."cat` WHERE `epath`='".$indexCache->epath."';";
$result=$db->query($sqlSelect);
$arrResult=$db->fetch_array($result);
//--------------------初始化数据--------------------------------------------
$indexCache->AnalysisData($arrResult,(array)$viewID);   //将viewid转换成数组
$indexCache->AnalysisCache();
$arrResult=null;
unset($arrResult);
$baidu=new baidu();
$baidu->AnalysisCache($indexCache->conBaidu[$viewID]);
$baidu->SetKey($indexCache->key);
$baidu->SetViewId($viewID);
$baidu->CrawlView();
//--------------------更新数据库--------------------------------------------
if($baidu->isUp){
    $sqlUpdate='UPDATE `'.$indexCache->dbPrefix.'cat` SET `baidu'.$viewID.'`=\''.addslashes($baidu->GetSerializeCache()) .'\' WHERE `cid`='.$indexCache->cid;
    $db->query($sqlUpdate);
}
$db->close();
//--------------------输出--------------------------------------------
if($cfg->isMemcached){
    $memcached=new Memcached();
    $memcached->addServer('127.0.0.1','11211');
}
$templets=GetContent($indexCache->mainDomain.'view.html','./templets/view.html',$memcached,86400,$cfg->isMemcached);
//生成相关链接：
$arrKeys=explode(',',$indexCache->keys);
$keysLink='';
foreach($arrKeys as $v){
    $keyEPath=GetEPath($v);
    $arr=explode('/',$keyEPath);
    $keysLink.='<li><a href="http://'.$arr[0].$indexCache->mainDomain.'/'.$arr[1].'">'.$v.'</a></li>';
}
//生成taglink
$tagLink='';
$arrTagKey=explode(',',$indexCache->tagKey);
foreach($arrTagKey as $v){
    $tagEPath=GetEPath($v);
    $arr=explode('/',$tagEPath);
    $tagLink.='<li><a href="http://'.$arr[0].$indexCache->mainDomain.'/'.$arr[1].'">'.$v.'</li>';
}

//替换数组
$arrReplace=array();
$arrReplace['key']=$indexCache->key;
$arrReplace['keys']=$indexCache->keys;
$arrReplace['title']=$baidu->title;
$arrReplace['maintitle']=$indexCache->title;
$arrReplace['adkey']=$indexCache->adKey;
$arrReplace['keyslink']=$keysLink;
$arrReplace['taglink']=$tagLink;
if($baidu->body==''){
    $arrReplace['body']='暂无数据，请等待。错误编码：'.md5($indexCache->baseUrl.$viewID);
}else{
    $arrReplace['body']=$baidu->body;
}
$arrReplace['body'].='您可能感兴趣：<br>'.$baidu->links;
$arrReplace['baseurl']=$indexCache->baseUrl;
$description=preg_replace("%<[^>]+>%",',',$baidu->description);
$description=substr_utf8($description, 0, 50);
$arrReplace['description']=$description;
foreach ($arrReplace as $k => $v) {
    $templets=str_replace('{'.$k.'}',$v,$templets);
}
echo $templets;
