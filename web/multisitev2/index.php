<?php
/**
 * Created by JetBrains PhpStorm.
 * User: ori
 * Date: 12-8-12
 * Time: 下午2:24
 * To change this template use File | Settings | File Templates.
 */
header("Content-type:text/html; charset=utf-8");
error_reporting(E_ALL & ~ E_NOTICE);
ini_set("magic_quotes_runtime",0);
include_once('./class/mysql_db.php');
include_once('./config.php');
include_once('./class/baidu.php');
include_once('./class/randArticle.php');
include_once('./class/main.php');
include_once('./function/function.php');
$cfg=new config();
$db=new dbQuery($cfg->dbHost,$cfg->dbUser,$cfg->dbPassword,$cfg->dbName);
$indexContent=new indexContent();

//--------------------获取maxid-------------------------------------
$content='';
if($cfg->isMemcached){
    $memcached=new Memcached();
    $memcached->addServer('127.0.0.1','11211');
    $content=$memcached->get($indexContent->mainDomain);
}
if($content=='' && file_exists('./cfg')){$content=file_get_contents('./cfg');}
if($content==''){
    $sqlSelect='SELECT MAX(cid) as maxcid FROM `'.$indexContent->dbPrefix.'cat`;';
    $result=$db->query($sqlSelect);
    $arrResult=$db->fetch_array($result);
    $maxID=$arrResult['maxcid'];
    $createTime=time();
    $arrCon=array('maxid'=>$maxID,'create'=>$createTime);
    $content=serialize($arrCon);
    CachePutContents($content,'./cfg');
}else{
    $arrResult=unserialize($content);
    $maxID=$arrResult['maxid'];
    $createTime=$arrResult['create'];
}
//--------------------随机3篇百度页面-------------------------------------
for($i=1;$i<=10;$i++){
    $arrBaiduViewId[$i]=$i;
}
shuffle($arrBaiduViewId);
$arrBaiduViewId=array_slice($arrBaiduViewId,0,3);
$sqlBaiduData='';
foreach ($arrBaiduViewId as $v){
    $sqlBaiduData.=',`baidu'.$v.'`';
}
//--------------------从数据库中读取-------------------------------------
$sqlSelect='SELECT `cid`,`title`,`key`,`keys`,`adkey`,`index`'.$sqlBaiduData.'FROM `'.$indexContent->dbPrefix."cat` WHERE `epath`='".$indexContent->epath."';";
$result=$db->query($sqlSelect);
$arrResult=$db->fetch_array($result);

//--------------------错误页面--------------------------------------------
if($db->affected_rows()==0){
    echo '<html><head><script language="JavaScript" src="/templets/head.js" type="text/JavaScript"></script></head><body>';
    for($i=0;$i<25;$i++){
        $sqlSelect='SELECT `key`,`epath` FROM `'.$indexContent->dbPrefix.'cat` WHERE `cid`='.rand(1,$maxID);
        $result=$db->query($sqlSelect);
        $arrResult=$db->fetch_array($result);
        $key=$arrResult['key'];
        $epath=$arrResult['epath'];
        $arrPath=explode('/',$epath);
        echo '<li><a href="http://'.$arrPath[0].$indexContent->mainDomain.'/'.$arrPath[1].'">'.$key.'</a></li>';
    }
    echo '</body></html>';
    $db->close();
    die();
}

//--------------------初始化数据--------------------------------------------
$indexContent->AnalysisData($arrResult,$arrBaiduViewId);
$arrResult=null;
unset($arrResult);

$indexContent->SetCreateTime($createTime);
$indexContent->AnalysisCache();

//--------------------无需更新--------------------------------------------
if(!$indexContent->NeedUp() && $indexContent->conIndex!=''){
    $db->close();
    $indexContent->Show($cfg->isMemcached,$memcached);
    die();
}

$upTimes=$indexContent->cid>500 ? 86400 : 432000;
$indexContent->SetUpTimes($upTimes);
//--------------------更新百度--------------------------------------------
$sqlUpBaidu='';
$indexContent->CleanBaiduArray();
$baiduLinks='';
foreach($arrBaiduViewId as $k=>$v){
    $baidu=new baidu();
    $baidu->AnalysisCache($indexContent->conBaidu[$v]);
    $baidu->SetKey($indexContent->key);
    $baidu->SetViewId($v);
    $baidu->InsertLinks($baiduLinks);
    $baidu->CrawlList();
    $baiduLinks=$baidu->links;
    $indexContent->SetBaidu($v,$baidu->title,$baidu->description);
    if($baidu->isUp){$sqlUpBaidu.=',`baidu'.$v."`='".addslashes($baidu->GetSerializeCache()) ."' ";}
    $baidu=NULL;
    unset($baidu);
}
//--------------------更新随机文章-----------------------------------------
$dayAfterCreate=(int)((time()-$indexContent->createTime)/86400);
$arrRandArticleViewId=array($dayAfterCreate+1,$dayAfterCreate+2,$dayAfterCreate+3);
$indexContent->CleanRandArticleArray();
foreach ($arrRandArticleViewId as $v) {
    $randArticle=new randArticle($cfg->isMemcached);
    $randArticle->SetKey($indexContent->key);
    $randArticle->SetKeys($indexContent->keys);
    $randArticle->SetStroyPath($cfg->storyPath);
    $randArticle->InitFeature($indexContent->baseUrl.$v);
    $randArticle->GenerateTitle();
    $randArticle->GenerateDescription();
    $indexContent->SetRandArticle($v,$randArticle->title,$randArticle->descriptions);
    $randArticle=NULL;
    unset($randArticle);
}
//--------------------更新TAG----------------------------------------------
$indexContent->UpDateTagKey($db);
//--------------------更新数据库-------------------------------------------
$sqlUpdate='UPDATE `'.$indexContent->dbPrefix."cat` SET `index`='".addslashes($indexContent->GetSerializeCache()) ."' ";
$sqlUpdate.=$sqlUpBaidu;
$sqlUpdate.=' WHERE `cid`='.$indexContent->cid;
$db->query($sqlUpdate);
$db->close();

$indexContent->Show($cfg->isMemcached,$memcached);