<?php
/**
 * Created by JetBrains PhpStorm.
 * User: afei
 * Date: 11-8-20
 * Time: 下午11:40
 * To change this template use File | Settings | File Templates.
 */
class Common extends Config{
    function __construct(){
        global $initCfg,$config,$z7Cfg,$krCfg,$fDir,$file,$seoCfg,$wpCfg,$wpOptsCfg,$wpdb;
        $this->initCfg=$initCfg;
        $this->config=$config;
        $this->z7Cfg=$z7Cfg;
        $this->krCfg=$krCfg;
        $this->fDir=$fDir;
        $this->file=$file;
        $this->seoCfg=$seoCfg;
        $this->wpCfg=$wpCfg;
        $this->wpOptsCfg=$wpOptsCfg;
        $this->wpdb=$wpdb;

        //repeat config.
        $this->siteUrl=$this->getSiteUrl();


        //wordpress常用
        $this->wpCateIds=get_all_category_ids();
        $this->wpCates=get_categories();
        $this->wpTags=get_tags();
    }

    /**
     * 功能：包裹标签
     * @param $tagName
     * @param $str
     * @return string
     */
    function wrapTags($tagName,$str) {
        return "<$tagName>$str</$tagName>";
    }

    /**
     * 功能：链接Wrap
     * @param $link
     * @param $str
     * @return string
     */
    function wrapLink($link,$str){
        return $this->filterNR(sprintf('<a href="%1$s" title="%2$s">%2$s</a> ',$link,$str));
    }

    /**
     * 功能：获取1个随机分类ID
     * @return int
     */
    function wp_getRandCatId() {
        return $this->getNumRandFromArr($this->wpCateIds);
    }



    /**
     * 功能：在String后面加后缀，$mode=1时，即加在前面
     * @param $str
     * @param $fix
     * @param int $mode
     * @return string
     */
    function addFix($str,$fix,$mode=0) {
        //前缀
        if($mode) {
            $str="{$str}_{$fix}";
        }else {
            $str="{$fix}_{$str}";
        }
        return $str;
    }//end_func


    /**
     * 功能：添加页眉、页脚
     * @param $content
     * @param $tipArr
     * @param string $mode
     * @return array
     */
    function addPageTip($content,$tipArr,$mode="head") {
        $tipStr=$this->getNumRandFromArr($tipArr);
        if($mode=="head"){
            array_unshift($content,$tipStr);
        }else{
            array_push($content,$tipStr);
        }
        return $content;
    }

    /**
     * 功能：返回版权信息数组
     * @return array
     */
    function getTipArr(){
        return array(
            '<p>此文章来源于网络，原文出自：<a class="cRed" href="'.$this->siteUrl.'" title="'.$this->config['webKeyWord'].'">'.$this->siteUrl.'</a></p>',
            '<p>欢迎转载，转载请注明出处：<a class="cRed" href="'.$this->siteUrl.'" title="'.$this->config['webKeyWord'].'">'.$this->siteUrl.'</a></p>',
            '<p>这里是一些关于<a class="cRed" href="'.$this->siteUrl.'" title="'.$this->config['webKeyWord'].'">'.$this->config['webKeyWord'].'</a>的文章，来源互联网，希望大家喜欢！',
            '<p>文章出自：<a class="cRed" href="'.$this->siteUrl.'" title="'.$this->config['webKeyWord'].'">'.$this->config['webKeyWord'].'</a>版权所有。本站文章除注明出处外，皆为作者原创文章，可自由引用，但请注明来源。 禁止全文转载。</p>'
        );
    }


    /*----------------------------生成随机时间 start-----------------------------------*/

    function getSeoDate($articlesNum=1000,$articesPerDay=5,$date="2011-01-03") {
        $sumDays=ceil($articlesNum/$articesPerDay);
        $randDate=array();
        $dateArr=$this->getDateArr($date);
        for($i=0; $i<$sumDays; $i++) {
            for($j=0; $j<$articesPerDay; $j++) {
                $randDate[]['pubdate']=$this->getFutureTime($dateArr);
            }
            $dateArr[2]+=1;//下个月
            $dateArr=$this->dealPubDate($dateArr);
        }
        //array_multisort($randDate);
        return array_slice($randDate,0,$articlesNum);
    }

    function dealPubDate($date="2011-01-13"){
        $dateArr=$this->getDateArr($date);
        $pub_month=$dateArr[1];
        $pub_day=$dateArr[2];

        $sumMonth=$this->getSumDays($pub_month);

        if($pub_day>$sumMonth) {
            $pub_day=1;
            $pub_month++;
            //此处跨年
            if($pub_month>12) {
                $dateArr[0]++;
                $pub_month=1;
                $pub_day=1;
            }
        }

        return array(	$dateArr[0],$pub_month,$pub_day);
    }

    /**
     * 功能：一个月共有多少天，有Bug
     * @param $month
     * @return int
     */
    function getSumDays($month){
        if($month<9){
            $sumMonth=($month%2==0)?30:31;
        }else{
            $sumMonth=($month%2==0)?31:30;
        }
        //特殊情况：28天
        if($month==2){
            $sumMonth=28;
        }
        return $sumMonth;
    }

    function getDateArr($date="2011-01-13"){
        if(is_array($date)) {
            $dateArr=$date;
        }else {
            $dateArr=explode("-",$date);
        }
        return $dateArr;
    }

    /**
     * 功能：产生一个随机的SEO时间
     * @param string $date
     * @return string
     */
    function getFutureTime($date) {
        $dateArr=$this->getDateArr($date);
        return sprintf(
            "%s-%s-%s %s:%s:%s",
            $dateArr[0],
            $dateArr[1],
            $dateArr[2],
            $this->rand2Time($this->seoCfg['timeArea'][0],$this->seoCfg['timeArea'][1]),
            $this->rand2Time(),
            $this->rand2Time());
    }//end_func

    /**
     * 功能：产生00-60的时间数
     * @param int $start
     * @param int $end
     * @return string
     */
    function rand2Time($start=1,$end=59) {
        return str_pad(rand($start,$end),2,"0",STR_PAD_LEFT);
    }
    /*----------------------------生成随机时间 end-----------------------------------*/

    /**
     * 功能：备份Wordpress数据库
     * @return void
     */
    function exportWpDb(){
        $cmd=sprintf("%s -u%s -p%s %s>%s",$this->initCfg['mysqlDump'],DB_USER,DB_PASSWORD,$this->initCfg['wpdbTbName'],$this->initCfg['wpdbBakName']);
        system($cmd,$ret);
        if(!$ret){
             printf("数据库备份成功，路径为：%s",$this->initCfg['wpdbBakName']);
        }
    }

    /**
     * 功能：备份单个的Post表
     * @return void
     */
    function exportSiglePostTable(){
        $hostFix=$this->getHostPreFix();
        $cmd=sprintf(
            "%s -u%s -p%s %s %s_posts>%s/%s_posts.sql",
            $this->initCfg['mysqlDump'],
            DB_USER,
            DB_PASSWORD,
            $this->initCfg['wpdbTbName'],
            $hostFix,
            OUT_FILES,
            $hostFix
        );
        system($cmd,$ret);
        if(!$ret){
             printf("数据表%s_post备份成功，路径为：%s/%s_posts.sql",$hostFix,OUT_FILES,$hostFix);
        }
    }


}
