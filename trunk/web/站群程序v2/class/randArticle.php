<?php
/**
 * Created by JetBrains PhpStorm.
 * User: ori
 * Date: 12-8-11
 * Time: 下午2:32
 * To change this template use File | Settings | File Templates.
 */
//include_once(dirname(__FILE__)."/../function/function.php");

class randArticle{
    var $isMemcached;
    var $memcached;
    var $storyPath;
    var $outLink='';
    var $key;
    var $arrKeys;
    var $md5Url='';
    var $arrContent=array();        //前5个做为标题文章库，全部28个作为内容文章库
    var $arrFeature=array();        //特征数组
    var $arrLibFeature=array();     //文章库特征数组
    var $title='';
    var $descriptions='';
    var $body='';

    /**
     * 初始化，确定是否采用memcached方式读取小说库
     * @param $isMemcached
     */
    function randArticle($isMemcached){
        $this->isMemcached=$isMemcached;
        if($this->isMemcached){
            $this->memcached=new Memcached();
            $this->memcached->addServer('127.0.0.1','11211');
        }
    }
    /**
     * 注意：outlink必须是完整html格式！如果多个连接用"|"分开
     * @param $outLink
     */
    function SetOutLink($outLink){$this->outLink=$outLink;}
    function SetKey($key){$this->key=$key;}
    function SetKeys($keys){$this->arrKeys=explode(',',$keys);}
    function SetStroyPath($storyPath){
        $this->storyPath=$storyPath;
    }
    /**
     * 生成特征值
     * @param $url  $subdomain.$maindomain.$path.$readid
     */
    function InitFeature($url){
        $md5Url=md5($url);
        $this->md5Url=$md5Url;
        $this->arrFeature=str_split(toTen(substr($md5Url,0,8)).toTen(substr($md5Url,8,8)).toTen(substr($md5Url,16,8)).toTen(substr($md5Url,24,8)),1);
        $arrLibFeature=array();
        for($i=0;$i<7;$i++){
            $arrLibFeature[$i]=ToTen(substr($md5Url,$i*4,4))+1;
            $arrLibFeature[$i+7]=ToTen(substr($md5Url,$i*4+1,4))+1;
            $arrLibFeature[$i+14]=ToTen(substr($md5Url,$i*4+2,4))+1;
            $arrLibFeature[$i+21]=ToTen(substr($md5Url,$i*4+3,4))+1;
        }
        $this->arrLibFeature=$arrLibFeature;
    }
    /**
     * 生成标题
     */
    function GenerateTitle(){
        $arrKeys=$this->arrKeys;
        $arrKeys=$this->JumbleArrayUseFeature($arrKeys);
        $exKey=$arrKeys[0];
        for($i=0;$i<5;$i++){
            $indexLib=$this->arrLibFeature[$i].'.txt';
            $this->arrContent[$i]=GetContent($indexLib,$this->storyPath.$indexLib,$this->memcached,0,$this->isMemcached);
        }
        $arrTitle=array_slice($this->arrContent,0,5);
        $title=$this->key.'|'.implode('|',$arrTitle).'|'.$exKey;
        $title=preg_replace("%<[^>]+>%",',',$title);//防止网址中字母被匹配
        $title=str_replace(array(',','。','：','”','“','！','……','？','；',' ','，'),'',$title);
        $arrTitle=explode('|',$title);
        $arrTitle=$this->ShuffleByFeature($arrTitle);
        $title=implode('',$arrTitle);
        $title=replace_db($title,'');
        $title=substr_utf8($title, 0, 30);
        $this->title=$title;
    }
    /**
     * 生成描述，注意：必须先生成标题！
     */
    function GenerateDescription(){
        $arrKeys=$this->arrKeys;
        $arrKeys=$this->JumbleArrayUseFeature($arrKeys);
        $exKey=$arrKeys[0];
        $arrDescription=array_slice($this->arrContent,0,5);
        $Description=implode('|',$arrDescription).'|'.$this->key.'|'.$exKey;
        $arrDescription=explode('|',$Description);
        //描述部分用随机就行了。没必要伪随机
        shuffle($arrDescription);
        $Description=implode('',$arrDescription);
        $Description=replace_dbs($Description,'.');
        $this->descriptions=substr_utf8($Description,0,1000);
    }
    /**
     * 生成文章内容，注意：必须先生成标题！
     */
    function GenerateBody(){
        for($i=5;$i<28;$i++){
            $indexLib=$this->arrLibFeature[$i].'.txt';
            $this->arrContent[$i]=GetContent($indexLib,$this->storyPath.$indexLib,$this->memcached,0,$this->isMemcached);
        }
        $arrBody=$this->arrContent;
        //初始化插入一些数据
        $lenArrKeys=count($this->arrKeys);
        $arrOutLinks=explode('|',$this->outLink);
        $arrOutLinks[]='';      //比较方便的保证在foreach里面插入所有的外链。
        $lenArrOutLinks=count($arrOutLinks);
        foreach ($arrBody as $k=>&$v) {
            if($k==0){continue;}
            if($k%3==0){$v.='|'.$this->key; }
            if($k%4==0){$v.='|。<br>';}
            if($k%$lenArrKeys==0){$v.='|'.$this->arrKeys[(int)($k/$lenArrKeys)-1];}
            if($k%$lenArrOutLinks==0){$v.='|'.$arrOutLinks[(int)($k/$lenArrOutLinks)-1];}
        }
        $body=implode('|',$arrBody);
        $arrBody=explode('|',$body);
        for($i=0;$i<6;$i++){
            $arrBody=$this->JumbleArrayUseFeature($arrBody);
        }
        $body=implode('',$arrBody).'。';
        $body=replace_dbs($body,'。');
        $this->body=$body;
    }

    /**
     * ！！！！！！！！！！！慎用！！！！！！！！！！！！！
     * 洗牌算法，
     * 先生成一组洗牌序列数组
     * 然后按照从后往前的顺序抽取成员按洗牌序列插入数组
     * 完成洗牌
     * 但是效率及乱序度都不行。慎用
     * @param $array
     * @return array
     */
    function ShuffleByFeature($array){
        //生成洗牌序列 $shuffle
        $lenArray=count($array);
        for($i=0;$i<$lenArray;$i++){
            $shuffle[$i]=$i;
        }
        $shuffle=$this->JumbleArrayUseFeature($shuffle);
        //洗牌
        for($i=0;$i<$lenArray;$i++){
            $temp=array_slice($array,$lenArray-$i-1,1);
            array_splice($array,$lenArray-$i-1,1);  //移出组员
            array_splice($array,$shuffle[$i],0,$temp);  //插入组员
        }
        return $array;
    }
    function JumbleArrayUseFeature($array){
        $lenArray=count($array);
        $lenFeature=count($this->arrFeature);
        $indexFeature=0;
        for($i=0;$i<$lenArray;$i++){
            $feature=$this->arrFeature[$indexFeature++];

            if($indexFeature>=$lenFeature){$indexFeature=0;}
            if($feature>=$lenArray){$feature=$lenArray-1;}

            $this->ExChange($array[$lenArray-$i-1],$array[$feature]);
        }
        return $array;
    }
    function ExChange(&$a,&$b){
        $x=$a;
        $a=$b;
        $b=$x;
    }

}