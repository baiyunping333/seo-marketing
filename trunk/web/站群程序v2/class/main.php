<?php
/**
 * Created by JetBrains PhpStorm.
 * User: ori
 * Date: 12-8-12
 * Time: 下午2:38
 * To change this template use File | Settings | File Templates.
 */
//include_once('./urlInfo.php');
//include_once('../function/function.php');
class urlInfo{
    var $subDomain;
    var $mainDomain;
    var $path;
    var $epath;
    var $dbPrefix;
    var $baseUrl;

    function urlInfo(){
        //得到二级域名及主域名
        $host=$_SERVER["HTTP_HOST"];
        $array=explode('.',$host);
        $len=count($array);
        $this->mainDomain=$array[$len-2].'.'.$array[$len-1];
        if($len==2){
            $this->subDomain='';
        }else{
            $this->subDomain=$array[0].'.';
        }

        //得到二级目录 $_SERVER["REQUEST_URI"] 获取URL，以‘/’开头 '/'     '/xxx.zzz'       '/xxx/yyy.zzz'      三种格式
        //path的值 如果为空，说明在根目录下“/”  如果不包含“.“，说明是目录  如果包含“.”，说明是文件
        $array=explode('/',$_SERVER["REQUEST_URI"]);
        $path=$array[1];

        if(strpos($path,'.')>0){
            $path='/';
        }else{
            $path='/'.$path.'/';
        }
        $path=str_replace('//','/',$path);
        $this->path=$path;

        $this->epath=$this->subDomain.$this->path;
        $this->baseUrl=$this->subDomain.$this->mainDomain.$this->path;
        $this->dbPrefix=str_replace('.','_',$this->mainDomain).'_';
        $this->dbPrefix=str_replace('-','_',$this->dbPrefix);

    }
}
class headInfo extends urlInfo {
    var $cid;
    var $key;
    var $keys;
    var $title;
    var $adKey;
    var $conIndex;              //只做读取，不做更新
    var $conBaidu=array();      //只做读取，不做更新

    function index(){
        $this->urlInfo();
    }
    /**
     * 分析数据库查询结果
     * @param array $arrDataFromMysql
     * @param array $baiduViewIds
     */
    function AnalysisData($arrDataFromMysql, $baiduViewIds=array()){
        $this->cid=$arrDataFromMysql['cid'];
        $this->key=$arrDataFromMysql['key'];
        $this->keys=$arrDataFromMysql['keys'];
        $this->title=$arrDataFromMysql['title'];
        $this->adKey=$arrDataFromMysql['adkey'];
        $this->conIndex=$arrDataFromMysql['index'];
        foreach ($baiduViewIds as $v) {
            $this->conBaidu[$v]=$arrDataFromMysql['baidu'.$v];
        }
    }
}

class indexContent extends headInfo{
    var $upTimes=86400;
    var $isUp=FALSE;
    var $maxTagID=10000; //做tag链接的关键词最大数量，按cid的顺序
    var $createTime;

    //缓存内容
    var $lastUpTime;
    var $tagKey;
    var $viewBaidu=array();         //array( $id=>array('title'=>$title,'descriptions'=>$descriptions)......)
    var $viewRandArticle=array();   //同上

    function indexContent(){$this->index();}    //构造函数
    function NeedUp(){
        return(time()-$this->lastUpTime>$this->upTimes);
    }
    function SetUpTimes($upTimes){
        $this->upTimes=$upTimes;
    }
    function SetCreateTime($createTime){
        $this->createTime=$createTime;
    }

    /**
     * 解析数据库读出的主页缓存
     */
    function AnalysisCache(){
        $arrCache=unserialize($this->conIndex);
        $this->upTimes=$arrCache['upTimes'];
        $this->lastUpTime=$arrCache['lastUpTime'];
        $this->tagKey=$arrCache['tagKey'];
        $this->viewBaidu=$arrCache['viewBaidu'];
        $this->viewRandArticle=$arrCache['viewRandArticle'];
    }

    /**
     * 返回序列化的主页缓存以待写入数据库
     * @return string
     */
    function GetSerializeCache(){
        $arrCache['upTimes']=$this->upTimes;
        $arrCache['lastUpTime']=time();
        $arrCache['tagKey']=$this->tagKey;
        $arrCache['viewBaidu']=$this->viewBaidu;
        $arrCache['viewRandArticle']=$this->viewRandArticle;
        $conIndex=serialize($arrCache);
        $this->conIndex=$conIndex;
        return $conIndex;
    }
    /**
     * 更新tagkey
     * @param dbQuery $db   传址 数据库连接
     * @return mixed
     */
    function UpDateTagKey(dbQuery &$db){
        $cycles=(int)((time()-$this->createTime)/259200)+1;   //以3天为周期更新tagKey
        if($cycles>20 && rand(1,100)>10 && $this->tagKey !=''){ return;}           //几率更新
        switch($cycles){
            case $cycles>20:
                $nowMaxId=434+($cycles-10)*100;
                break;
            case $cycles>10:
                $nowMaxId=34+($cycles-10)*40;
                break;
            case $cycles>4:
                $nowMaxId=4+($cycles-4)*5;
                break;
            default:
                $nowMaxId=$cycles;
        }
        $nowMaxId = $nowMaxId < $this->maxTagID ? $nowMaxId : $this->maxTagID;
        $nowMaxId = $nowMaxId < 25 ? 25 : $nowMaxId;
        $totalTagNumber = $nowMaxId >= 10 ? 10 : $nowMaxId;
        $arrTagKey=array();
        for($i=0;$i<$totalTagNumber;$i++){
            $sqlSelect='SELECT `key` FROM `'.$this->dbPrefix.'cat` WHERE `cid`='.rand(1,$nowMaxId);
            $result=$db->query($sqlSelect);
            $arrResult= $db->fetch_array($result);
            $arrTagKey[]=$arrResult['key'];
        }
        $this->tagKey=implode(',',$arrTagKey);
        $this->isUp=TRUE;
    }
    /**
     * 设置主页缓存中百度内容的更新
     * 如果要更新主页缓存中的百度内容，请先调用CleanBaiduArray()方法将百度内容数组置空
     * @param $indexOfBaidu
     * @param $title
     * @param $description
     */
    function SetBaidu($indexOfBaidu,$title,$description){
        $this->viewBaidu[$indexOfBaidu]['title']=$title;
        $this->viewBaidu[$indexOfBaidu]['description']=$description;
        $this->isUp=TRUE;
    }
    function CleanBaiduArray(){
        $this->viewBaidu=array();
    }
    /**
     * 设置主页缓存中随机文章内容的更新
     * 如果要更新主页缓存中的随机文章内容，请先调用CleanRandArticleArray()方法将随机文章内容数组置空
     * @param $indexOfRandArticle
     * @param $title
     * @param $description
     */
    function SetRandArticle($indexOfRandArticle,$title,$description){
        $this->viewRandArticle[$indexOfRandArticle]['title']=$title;
        $this->viewRandArticle[$indexOfRandArticle]['description']=$description;

    }
    function CleanRandArticleArray(){
        $this->viewRandArticle=array();
    }
    /**
     * 输出正文
     * @param $isMemcached
     * @param $memcached
     */
    function Show($isMemcached, $memcached){
        $templets=GetContent($this->mainDomain.'index.html','./templets/index.html',$memcached,86400,$isMemcached);
        //生成相关链接：
        $arrKeys=explode(',',$this->keys);
        $keysLink='';
        foreach($arrKeys as $v){
            $keyEPath=GetEPath($v);
            $arr=explode('/',$keyEPath);
            $keysLink.='<li><a href="http://'.$arr[0].$this->mainDomain.'/'.$arr[1].'">'.$v.'</a></li>';
        }
        //生成taglink
        $tagLink='';
        $arrTagKey=explode(',',$this->tagKey);
        foreach($arrTagKey as $v){
            $tagEPath=GetEPath($v);
            $arr=explode('/',$tagEPath);
            $tagLink.='<li><a href="http://'.$arr[0].$this->mainDomain.'/'.$arr[1].'">'.$v.'</li>';
        }
        //循环内容
        //百度
        $templetsBaidu=InStr('{baidu:}','{/baidu}',$templets);
        $conBaidu='';
        foreach($this->viewBaidu as $k=>$v){
            if($v['title']==''){continue;}
            $temp=str_replace('{url}','http://'.$this->baseUrl.'view-'.$k.'.html',$templetsBaidu);
            $temp=str_replace('{title}',$v['title'],$temp);
            $temp=str_replace('{description}',$v['description'],$temp);
            $conBaidu.=$temp;
        }
        $templets=str_replace('{baidu:}'.$templetsBaidu.'{/baidu}',$conBaidu,$templets);
        $templetsBaidu=null;
        $conBaidu=null;
        $temp=null;
        unset ($templetsBaidu,$conBaidu,$temp);
        //随机小说库生成文章
        $templetsRandArticle=InStr('{randarticle:}','{/randarticle}',$templets);
        $conRandArticle='';
        foreach ($this->viewRandArticle as $k => $v) {
            $temp=str_replace('{url}','http://'.$this->baseUrl.'read-'.$k.'.html',$templetsRandArticle);
            $temp=str_replace('{title}',$v['title'],$temp);
            $temp=str_replace('{description}',$v['description'],$temp);
            $conRandArticle.=$temp;
        }
        $templets=str_replace('{randarticle:}'.$templetsRandArticle.'{/randarticle}',$conRandArticle,$templets);
        $templetsRandArticle=null;
        $conRandArticle=null;
        $temp=null;
        unset($templetsRandArticle,$conRandArticle,$temp);
        //替换数组
        $arrReplace=array();
        $arrReplace['key']=$this->key;
        $arrReplace['keys']=$this->keys;
        $arrReplace['title']=$this->title;
        $arrReplace['adkey']=$this->adKey;
        $arrReplace['keyslink']=$keysLink;
        $arrReplace['taglink']=$tagLink;
        foreach ($arrReplace as $k => $v) {
            $templets=str_replace('{'.$k.'}',$v,$templets);
        }
        echo $templets;
        $templets=null;
    }
}
