<?php
/**
 * Created by JetBrains PhpStorm.
 * User: afei
 * Date: 11-8-26
 * Time: 下午12:55
 * To change this template use File | Settings | File Templates.
 */
class Config{
    function __construct($initCfg,$krCfg,$seoCfg,$wpCfg){
        global $wpdb;
        $this->wpdb=$wpdb;
        $this->initCfg=$initCfg;
        $this->krCfg=$krCfg;
        $this->seoCfg=$seoCfg;
        $this->wpCfg=$wpCfg;
        $this->siteUrl=$this->getSiteUrl();
    }

    /**
     * 功能：获取Site主关键字
     * @return mixed
     */
    function getSiteKeyWord(){
        $webTitle = $this->getSiteTitle();
        return current(explode($this->initCfg['titleSeparate'],$webTitle));
    }

    /**
     * 功能：取得当前URL
     * @return string
     */
    function getCurSiteUrl(){
        return 'http://'.$this->getHostPreFix().'.'.$this->initCfg['domain'];
    }

    /**
     * 功能：由HOST取得网站前缀
     * @return mixed
     */
    function getHostPreFix(){
        return current(explode('.',$_SERVER["HTTP_HOST"]));
    }

    /**
     * 功能：获取Site的Title
     * @return string
     */
    function getSiteTitle(){
        $webTitleArr=$this->getSiteTitleArr();
        return implode($this->initCfg['titleSeparate'],$webTitleArr);
    }

    /**
     * 功能：网站的唯一主关键字
     * @return string
     */
    function getSiteKeyWords(){
        $webTitleArr=$this->getSiteTitleArr();
        return implode($this->initCfg['keyWordsSeparate'],$webTitleArr);
    }

    /**
     * 功能：取得几个关键词数组
     * @return mixed
     */
    function getSiteTitleArr(){
        $webTitleArr=$this->getNumRandFromArr($this->getLtailsArr(),$this->initCfg['webTitleKeyNum']);
        $this->trimAsr(&$webTitleArr);
        return $webTitleArr;
    }

    /**
     * 功能：文章来源
     * @return mixed
     */
    function getArticleSrc(){
        return $this->getNumRandFromArr(array(
            $this->getSiteKeyWord()."网",
            $this->getSiteKeyWord()."官网"
        ));
    }

    /**
     * 功能：获取基本主题动态参数
     * @return array
     */
    function getThemeCfgArr(){
        $num=$this->initCfg['siteNum'];
        $tempArr=array();
        for($i=1;$i<=$num;$i++){
            //$siteFix=
            $nibNum=$this->getNibble($num);
            $siteFix=$this->initCfg['siteKeyFix'].$this->getNibbleNum($i,$nibNum);
            $tempArr[]=$this->getThemeCfg($siteFix);
            $tempArr['sitePreFix']=$siteFix;
        }
        return $tempArr;
    }

    /**
     * 功能：随机取得一条themeCfg信息
     * @param $siteFix
     * @return array
     */
    function getThemeCfg($siteFix){
        return array(
            'webTitle'=>$this->getSiteTitle(),
            'keyword'=>$this->getSiteKeyWord(),
            'articleSrc'=>$this->getArticleSrc(),
            'metaKeywords'=>$this->getSiteKeyWords(),
            'sitePreFix'=>$siteFix
        );
    }

    /**
     * 功能：添加siteNum条记录到afeios_cfg中去
     * @return void
     */
    function insertAfeiosCfg(){
        $themeCfgArr=$this->getThemeCfgArr();
        for($i=0;$i<$this->initCfg['siteNum'];$i++){
            $this->wpdb->insert($this->initCfg['tb_afeios_cfg'],$themeCfgArr[$i]);
        }
        echo $this->initCfg['siteNum']."条记录基本配置添加成功！当前最后一条记录为：".$this->wpdb->insert_id;
    }

    /**
     * 功能：清空afeios_cfg表
     * @return void
     */
    function turnCateAfeiosCfg(){
        $sql=sprintf("TRUNCATE TABLE `%s`",$this->initCfg['tb_afeios_cfg']);
        $this->wpdb->query($sql);
        echo $this->initCfg['tb_afeios_cfg']."已经被清空！";
    }


    /**
     * 功能：根据hostSiteFix来获取theme的相关参数
     * @return mixed
     */
    function getCfgByHostSiteFix(){
        $sql=sprintf("SELECT * FROM %s where sitePreFix='%s'",$this->initCfg['tb_afeios_cfg'],$this->getHostPreFix());
        return $this->wpdb->get_results($sql);
    }

    function getAllSiteFix(){
        $sql=sprintf("SELECT sitePreFix FROM %s",$this->initCfg['tb_afeios_cfg']);
        return $this->wpdb->get_results($sql);
    }

    function getAllSiteFixArr(){
        $siteFixs=$this->getAllSiteFix();
        foreach($siteFixs as &$siteFix){
            $siteFix=$siteFix->sitePreFix;
        }
        return $siteFixs;
    }


    /**
     * 功能：获取Int数位
     * @param $num
     * @return float
     */
    function getNibble($num){
		return count(str_split($num))-1;
    }

    /**
     * 功能：如：12，3，得到：012
     * @param $num
     * @param $nib
     * @return string
     */
    function getNibbleNum($num=12,$nib=3){
		return str_pad($num, $nib,"0",STR_PAD_LEFT);
    }

    /**
     * 功能：去字符串两边空白
     * @param $asr
     * @return string
     */
    function trimAsr($asr){
        if(is_array($asr)){
            foreach($asr as &$v){
                $v=trim($v);
            }
            return $asr;
        }else{
            return trim($asr);
        }
    }


    /**
     * 功能：将TXT里每一行词转化为Array元素
     * @param $fileName
     * @return array
     */
    function dirExpendArr($fileName){
        return explode("\n",file_get_contents($fileName));
    }

     /**
     * 功能：将String/Array转化为Utf8格式，防止乱码
     * @param $asr:string/array
     * @return array
     */
    function transToUtf8($asr){
        if(is_array($asr)){
            foreach($asr as &$v){
                $v=iconv("gb2312","UTF-8", $v);
            }
            return $asr;
        }else{
            return iconv("gb2312", "UTF-8", $asr);
        }
    }

    /**
     * 功能：将换行替换为空
     * @param $str
     * @return mixed
     */
    function filterNR($asr){
        $nr=array("\r\n", "\n", "\r");
        if(is_array($asr)){
            foreach($asr as &$str){
                $str=str_replace($nr,"",$str);
            }
            return $asr;
        }else{
            return str_replace($nr,"",$asr);
        }
    }

    /**
     * 功能：获取长尾词Array
     * @return array
     */
    function getLtailsArr(){
        $keyArr=$this->dirExpendArr($this->initCfg['ltailsDir']);
        return $this->transToUtf8($keyArr);
    }


    function getBlogDescption(){
        $webTitle=$this->getSiteKeyWords();
        $webArticleSrc=$this->getArticleSrc();
        return $webTitle.'【'.$webArticleSrc.'】';
    }

    /**
     * 功能：从TXT中得到pingSites
     * @return array
     */
    function getPinSitesArr(){
        $pingSites=$this->dirExpendArr($this->initCfg['pingSites']);
        return $this->transToUtf8($pingSites);
    }


    function getPingSitesSql(){
        $urls='\''.implode('\r',$this->filterNR($this->getPinSitesArr())).'\'';
        $table=$this->getHostPreFix().'_options`';
        $sql='UPDATE `'.$table.' SET `option_value`='.$urls.' WHERE (`option_name`=\'ping_sites\')';
        return $sql;
    }

    /**
     * 功能：数组中随机取出N个元素
     * @param $arr
     * @param int $num
     * @return mixed
     */
    function getNumRandFromArr($arr,$num=1){
        if($num!=1){
            if(count($arr)<$num){
                $num=count($arr);
            }
            $keys=array_rand($arr,$num);
            foreach($keys as &$key){
                $key=$arr[$key];
            }
            return $keys;
        }else{
            return $arr[array_rand($arr,1)];
        }
    }

    /**
     * 功能：获取网站的URL主地址
     * @return string
     */
    function getSiteUrl(){
        return sprintf('http://%s.%s',$this->getHostPreFix(),$this->initCfg['domain']);
    }


    /**
     * 功能：向wp-hive中添加对应域名列表
     * @return void
     */
    function insertSiteNumWpHive(){
        $num=$this->initCfg['siteNum'];
        $site=array();
        for($i=1;$i<=$num;$i++){
            //$siteFix=
            $nibNum=$this->getNibble($num);
            $siteFix=$this->initCfg['siteKeyFix'].$this->getNibbleNum($i,$nibNum);
            $site['host']=$siteFix.'.'.$this->initCfg['domain'];
            $site['path']='/';
            $site['prefix']=$siteFix."_";
            $this->wpdb->insert('wphive_hosts', $site);
        }
        echo $this->initCfg['siteNum']."条记录wpHive域名表已经添加成功！当前最后一条记录为：".$this->wpdb->insert_id;
    }

    /**
     * 功能：添加默认wp前缀表
     * @return void
     */
    function insertDefaultWpHive(){
        $defaultSite=array(
            'host'=>$this->initCfg['siteKeyFix'].'.'.$this->initCfg['domain'],
            'path'=>'/',
            'prefix'=>$this->initCfg['siteKeyFix'].'_'
        );
        $this->wpdb->insert('wphive_hosts', $defaultSite);
    }

    /**
     * 功能：清空wphive_hosts表
     * @return void
     */
    function turnCateHiveHost(){
        $sql=sprintf("TRUNCATE TABLE `wphive_hosts`");
        $this->wpdb->query($sql);
        //以防意外，添加一个默认可操作程序！
        $this->insertDefaultWpHive();
        echo "wphive_hosts已经被清空！您可以继续访问：http://wp.".$this->initCfg['domain'];
    }


}

