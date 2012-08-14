<?php
/**
 * Created by JetBrains PhpStorm.
 * User: ori
 * Date: 12-8-14
 * Time: 上午12:11
 * To change this template use File | Settings | File Templates.
 */
header("Content-type:text/html; charset=utf-8");
error_reporting(E_ALL & ~ E_NOTICE);
ini_set("magic_quotes_runtime",0);
include_once('./class/mysql_db.php');
include_once('./config.php');
include_once('./class/randArticle.php');
include_once('./class/main.php');
include_once('./function/function.php');
$cfg=new config();
$db=new dbQuery($cfg->dbHost,$cfg->dbUser,$cfg->dbPassword,$cfg->dbName);
$indexCache=new indexContent();
$viewID=InStr('read-','.html',$_SERVER["REQUEST_URI"]);	//当前百度问答采集页面ID。范围从1-10。对应baidu1-baidu10
//--------------------从数据库中读取-------------------------------------
$sqlSelect='SELECT `cid`,`title`,`key`,`keys`,`adkey`,`index` FROM `'.$indexCache->dbPrefix."cat` WHERE `epath`='".$indexCache->epath."';";
$result=$db->query($sqlSelect);
$arrResult=$db->fetch_array($result);
//--------------------初始化数据--------------------------------------------
$indexCache->AnalysisData($arrResult);   //将viewid转换成数组
$indexCache->AnalysisCache();
$arrResult=null;
unset($arrResult);
//--------------------获取createTime-------------------------------------
$content='';
if($cfg->isMemcached){
    $memcached=new Memcached();
    $memcached->addServer('127.0.0.1','11211');
    $content=$memcached->get($indexCache->mainDomain);
}
if($content=='' && file_exists('./cfg')){$content=file_get_contents('./cfg');}
if($content==''){
    $sqlSelect='SELECT MAX(cid) as maxcid FROM `'.$indexCache->dbPrefix.'cat`;';
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
//--------------------生成外链--------------------------------------------
//暂时不生成链轮。只生成到上层的返回链接，锚文本插入文章内容中的
//具体生成导出链接待定。这里先将外链置空（必须保留空成员，不然加入链接后文章会变样）
$arrOutLink=array();
$arrOutLink[]='<a href="http://'.$indexCache->baseUrl.'">'.$indexCache->key.'</a>';    //上层链接
$arrOutLink[]='1';
$arrOutLink[]='2';
$arrOutLink[]='3';
$arrOutLink[]='';
$outLink=implode('|',$arrOutLink);
$db->close();
//--------------------生成外链完毕--------------------------------------------
$randArticle=new randArticle($cfg->isMemcached);
$randArticle->SetKey($indexCache->key);
$randArticle->SetKeys($indexCache->keys);
$randArticle->SetStroyPath($cfg->storyPath);
$randArticle->SetOutLink($outLink);
$randArticle->InitFeature($indexCache->baseUrl.$viewID);
$randArticle->GenerateTitle();
$randArticle->GenerateBody();
//--------------------生成上下页--------------------------------------------
$dayAfterCreate=(int)((time()-$createTime)/86400)+1;
$maxRandArticleId=$dayAfterCreate+3;
$nextLink='<p><a href="./read-'.($viewID+1).'.html">下一篇</a></p>';
$lastLink='<p><a href="./read-'.($viewID-1).'.html">上一篇</a></p>';
if($viewID>=$maxRandArticleId){$nextLink='';}
if($viewID<=1){$lastLink='';}
$randLink='<p><a href="./read-'.rand(1,$maxRandArticleId).'.html">推荐文章</a></p>';
$additionLinks=$lastLink.$nextLink.$randLink;
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
$arrReplace['title']=$randArticle->title;
$arrReplace['maintitle']=$indexCache->title;
$arrReplace['adkey']=$indexCache->adKey;
$arrReplace['keyslink']=$keysLink;
$arrReplace['taglink']=$tagLink;
$arrReplace['body']=$randArticle->body.'<p>'.$additionLinks;
$arrReplace['baseurl']=$indexCache->baseUrl;
$arrReplace['description']='';
foreach ($arrReplace as $k => $v) {
    $templets=str_replace('{'.$k.'}',$v,$templets);
}
echo $templets;

