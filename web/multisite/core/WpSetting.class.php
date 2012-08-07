<?php
/**
 * Created by JetBrains PhpStorm.
 * User: afei
 * Date: 11-8-27
 * Time: 上午2:13
 * To change this template use File | Settings | File Templates.
 */
class WpSetting extends MultiSite{
    function __construct(){
        global $wpdb,$initCfg;
        $this->wpdb=$wpdb;
        $this->initCfg=$initCfg;
        parent::__construct();
    }

    /**
     * 功能：获取Host列表
     * @return mixed
     */
    function getHostList(){
        $sql=sprintf("SELECT host FROM wphive_hosts limit %s",$this->initCfg['siteNum']+1);
        return $this->wpdb->get_results($sql);
    }

    /**
     * 功能：获取Array形式的Hostlist
     * @return mixed
     */
    function getHostListArr(){
        $hostLists=$this->getHostList();
        foreach($hostLists as &$host){
            $host=$host->host;
        }
        return $hostLists;
    }

    function getHostListArrWithHttp(){
        $hostLists=$this->getHostList();
        foreach($hostLists as &$host){
            $host='http://'.$host->host;
        }
        return $hostLists;
    }

    /**
     * 功能：获取URL地址列表，如:http://www.site1.com
     * @return mixed
     */
    function getSiteUrlList($tails=''){
        $hostLists=$this->getHostList();
        foreach($hostLists as &$host){
            $host='http://'.$host->host.$tails;
        }
        return $hostLists;
    }

    function exportBatFile($tail,$filename){
        $cmd="@echo on";
        $siteList=$this->getHostListArrWithHttp();
        $i=isset($this->initCfg['addPostByCmdStart'])?$this->initCfg['addPostByCmdStart']: 0;
        $end=isset($this->initCfg['addPostByCmdEnd'])?$this->initCfg['addPostByCmdEnd']: $this->initCfg['siteNum'];
        for(; $i<=$end; $i++){
            $cmd.=sprintf("\nstart chrome.exe %s/%s\nping 127.1 -n %s 1>nul",$siteList[$i],$tail,$this->initCfg['addPostByCmdTimeOut']*60);
            $cmd.="\ntaskkill /f /t /im chrome.exe";
        }
        $this->file->write(OUT_FILES.'/'.$filename,$cmd,'w');
        printf('BAT文件生成成功！本机路径为：%s 变化时间：%s',OUT_FILES.'/'.$filename,mktime());
    }

    /**
     * 功能：输出添加大量文章的bat文件
     * @return void
     */
    function addPostsByCmd(){
        $this->exportBatFile("afeios/post.php?keys=addpost",'addPost.bat');
    }

    /**
     * 功能：导出大量的Table
     * @return void
     */
    function exportPostTables(){
        $this->exportBatFile("afeios/index.php?action=execExportSiglePostTable",'exportPostTable.bat');
    }

    function gzipPostTables(){
        $cmd="@echo on";
        $siteFixs=$this->getAllSiteFixArr();
        $i=isset($this->initCfg['addPostByCmdStart'])?$this->initCfg['addPostByCmdStart']: 0;
        $end=isset($this->initCfg['addPostByCmdEnd'])?$this->initCfg['addPostByCmdEnd']: $this->initCfg['siteNum'];
        //echo $this->z7Cfg['z7Exe'];exit;
        //echo $siteFixs[20];exit;
        for(; $i<$end; $i++){
            $cmd.=sprintf("\n%s a -tgzip %s_posts.gz %s_posts.sql\nping 127.1 -n %s 1>nul",
                $this->z7Cfg['z7Exe'],
                OUT_FILES."/".$siteFixs[$i],
                OUT_FILES."/".$siteFixs[$i],
                $this->initCfg['addPostByCmdTimeOut']*60);
        }
        $this->file->write(OUT_FILES.'/gzipPostTables.bat',$cmd,'w');
        printf('BAT文件生成成功！本机路径为：%s 变化时间：%s',OUT_FILES.'/gzipPostTables.bat',mktime());
        //print_r($this->getAllSiteFix());
        /*
        $siteList=$this->getHostListArrWithHttp();


        $this->file->write(OUT_FILES.'/'.$filename,$cmd,'w');
        printf('BAT文件生成成功！本机路径为：%s 变化时间：%s',OUT_FILES.'/'.$filename,mktime());
        */
    }

    function createGoogleMapBat(){
        $cmd="@echo on";
        $siteList=$this->getHostListArrWithHttp();
        $tails="wp-admin/options-general.php?page=google-sitemap-generator%%2Fsitemap.php&sm_wpv=3.1.3&sm_pv=3.2.4&sm_rebuild=true&noheader=true&_wpnonce={$this->initCfg['wpnonce']}";
        $i=isset($this->initCfg['addPostByCmdStart'])?$this->initCfg['addPostByCmdStart']: 0;
        $end=isset($this->initCfg['addPostByCmdEnd'])?$this->initCfg['addPostByCmdEnd']: $this->initCfg['siteNum'];
        for(; $i<=$end; $i++){
            $cmd.=sprintf("\nstart chrome.exe \"%s/%s\"",$siteList[$i],$tails);
            if($i % $this->initCfg['cmdPerOpen']==0){
                $cmd.=sprintf("\nping 127.1 -n %s 1>nul",$this->initCfg['addPostByCmdTimeOut']*60);
                $cmd.="\ntaskkill /f /t /im chrome.exe";
            }
        }
        $this->file->write(OUT_FILES.'/createGoogleMap.bat',$cmd,'w');
        printf('BAT文件生成成功！本机路径为：%s 变化时间：%s',OUT_FILES.'/createGoogleMap.bat',mktime());
    }


    /**
     * 功能：php数组转化为javascript数组
     * @param $arr
     * @return string
     */
    function arrPhpToJs($arr){
        if(is_array($arr)){
            foreach($arr as &$ar){
                $ar="\"".$ar."\"";
            }
            return '['.implode(',',$arr).']';
        }
    }

    /**
     * 功能：输入JS的URL数组
     * @return string
     */
    function getJsUrlList($keys="createsites"){
        switch($keys){
            //安装1000个网站
            case 'createsites':
                $tails='';
            break;

            //生成googleSitemap
            case 'creategmap':
                $tails="/wp-admin/options-general.php?page=google-sitemap-generator%2Fsitemap.php&sm_wpv=3.1.3&sm_pv=3.2.4&sm_rebuild=true&noheader=true&_wpnonce={$this->initCfg['wpnonce']}";
            break;

            //基本设置+plugins
            case 'setbasecfg':
                $tails="/afeios/post.php?keys=setbasecfg";
            break;

            //创建分类
            case 'createcate':
                $tails="/afeios/post.php?keys=createcate";
            break;

            //清除所有分类
            case 'delallcates':
                $tails="/afeios/post.php?keys=delallcates";
            break;

            //发布文章：
            case 'addpost':
                $tails="/afeios/post.php?keys=addpost";
            break;

            //清空post表：
            case 'delallposts':
                $tails="/afeios/post.php?keys=delallposts";
            break;


        }
        $siteUrlList=$this->getSiteUrlList($tails);
        return $this->arrPhpToJs($siteUrlList);
    }


    function setUpForAllSites(){
        $setupUrl="http://wp.".$this->initCfg['domain']."/afeios/setup.php";
        printf('请手动访问这个地址：<a href="%1$s" target="_blank">%1$s</a> ',$setupUrl);
    }


    /**
     * 功能：一次性设置好所有的Opts+plugins
     * @return void
     */
    function setOptsForAllSites(){
        foreach($this->wpOptsCfg as $optsName => $optsVal){
            $this->updateOpts($optsName,$optsVal);
        }
        printf('当前站：<span class="cRed">%s</span>基本设置已经完成！',$this->getCurSiteUrl())."<br>";
    }


}
