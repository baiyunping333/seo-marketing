<?php
/**
 * Created by JetBrains PhpStorm.
 * User: afei
 * Date: 11-9-4
 * Time: 上午12:25
 * To change this template use File | Settings | File Templates.
 */
class Exec extends WpSetting{
    function index(){
        //echo "I'm index method,please read my source code...";
    }
    //01:在本地生成对应的host文件:C:\WINDOWS\system32\drivers\etc\hosts
    function execCreateWin32etcHost(){
        $this->createSiteNumLocalHosts();
    }
    //02:在本地生成对应的virthHost文件:D:\wamp\Apache2\conf\extra\httpd-vhosts.conf
    function execCreateVitureHost(){
        $this->createSiteNumVirtualHost();
    }
    //03:生成wp-hive插件的URL列表到DB中去:wphive_hosts
    function execCreateWpHiveUrlList(){
        $this->insertSiteNumWpHive();
    }
    //03-1:对应清空wp-hive的URLList表：已经添加默认项了
    function execDelSiteNumWpHive(){
        $this->turnCateHiveHost();
    }
    //04:保存WP主题的相关参数到DB中去:afeios_cfg
    function execInsertAfeiosCfg(){
        $this->insertAfeiosCfg();
    }
    //04-1:清空themeCfg表
    function execDelAfeiosCft(){
        $this->turnCateAfeiosCfg();
    }
    //05：创建1000个站点：
    function execCreateSites(){
        go('/afeios/setup.php?keys=createsites');
    }
    //06:基本设置+plugins__________________
    function execSetBaseSettingAndPlugins(){
        //alert('请先将afeios/index.php参数设置成：$KR->setOptsForAllSites()，并注释Exec.class.php此行代码！');exit;
        go('/afeios/setup.php?keys=setbasecfg');
    }
    //07:startGoogleMap
    function execCreateGoogleMap(){
        //方案一：网页弹出方式
        //go('/afeios/setup.php?keys=creategmap');
        //方案二：采取cmd运行生成bat方式
        $this->createGoogleMapBat();
    }
    //08:插入文章分类等参数：__________________
    function execInsertArticleCates(){
        go('/afeios/setup.php?keys=createcate');
    }
    //08-1:删除所有分类：
    function execDelArticleCates(){
        go('/afeios/setup.php?keys=delallcates');
    }
    //09:向每个Blog中添加文章___________________
    function execWpPosts(){
        go('/afeios/setup.php?keys=addpost');//delallcates
    }
    function execDelAllPosts(){
        go('/afeios/setup.php?keys=delallposts');
    }
    function execAddPostsByCmd(){
        $this->addPostsByCmd();
    }
    //10:导出wordpress表
    function execExportWpDb(){
        $this->exportWpDb();
    }
    //10-1:导出host对应的POST表：
    function execExportSiglePostTable(){
        $this->exportSiglePostTable();
    }
    function execExportPostTables(){
        $this->exportPostTables();
    }
    function execGzipPostTables(){
        $this->gzipPostTables();
    }









}
