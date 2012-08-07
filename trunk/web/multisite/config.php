<?php
/**
 * Created by JetBrains PhpStorm.
 * User: afei
 * Date: 11-8-23
 * Time: 下午10:36
 * To change this template use File | Settings | File Templates.
 */

//初始化配置类：
$initCfg=array(
    'titleSeparate'=>'_',
    'domain'=>'qiaojoe.com',
    'setupTimeOut'=>1,
    'addPostByCmdTimeOut'=>0.4,                  //结合CMD命令添加文章，单位:分钟
    'addPostByCmdStart'=>1,                     //开始文章，默认从fix.qiaojoe.com,fix01.qiaojoe...
    'addPostByCmdEnd'=>1001,                     //此处默认参数为siteNum+1
    'cmdPerOpen'=>5,
    'setupStart'=>1,
    'setupEnd'=>1001,
    'keyWordsSeparate'=>',',
    'tb_afeios_cfg'=>'afeios_cfg',
    'webTitleKeyNum'=>rand(3,6),
    'siteNum'=>1000,
    'siteKeyFix'=>'jf',
    'win32etcDir'=>'C:/WINDOWS/system32/drivers/etc/hosts',
    'vhostDir'=>'D:/wamp/Apache2/conf/extra/httpd-vhosts.conf',
    'mysqlDump'=>'D:/wamp/mysql/bin/mysqldump.exe',
    'wpdbTbName'=>'wordpress',
    'wpnonce'=>'2853e92f29',
    'wpdbBakName'=>ABSPATH.'afeios/outfiles/wpbak.sql',
    'quotesDir'=>ABSPATH.'afeios/data/quotes.txt',       //随机名人名言文件
    'ltailsDir'=>ABSPATH.'afeios/data/ltails.txt',       //长尾词文件
    'pingSites'=>ABSPATH.'afeios/data/pings.txt'        //长尾词文件
);

/**
 *C:伪原创相关参数
 */
$seoCfg=array(
    'randArrNum'=>rand(2,6),                        //文章打乱次数
    'quotesDir'=>$initCfg['quotesDir'],           //随机名人名言文件
    'ltailsDir'=>$initCfg['ltailsDir'],           //长尾词文件
    'keyPerContent'=>rand(2,6),                    //每篇中插入多少个长尾Keylinks
    'timeArea'=>array(7,10)                        //SEO文章发布时间段
);

/**
 * 7z压缩程序相关参数
 */
$z7Cfg=array(
    'z7Exe'=>'D:/7-Zip/7zG.exe'
);

/**
 * B:采集处理部分CJ
 */
$krCfg=array(
    'krDownload'=>'E:/seoArea/dny-wordpress/download/'
);

/**
 * D:wordpress相关参数
 */
$wpCfg=array(
    'moreTag'=>'<!--more-->',                   //wordpress 的more标签
    'nextTag'=>'<!--nextpage-->',              //wordpress 的nextpage标签
    'numMoreTag'=>2,                             //在第N段之后添加more标签
    'numNextTag'=>rand(8,10),                     //每N段之后添加nextpage标签
    'tagPerArticle'=>rand(2,6),                 //每篇文章中添加N个Tag
    'catesPerSite'=>rand(5,10),                  //每个站设置多少个分类
    'relaLinkNum'=>0,                           //相关文章数目
    'articlePerDay'=>10,
    'startPubDate'=>'2011-09-30'
);

include_once(ABSPATH.'afeios/libs/config.class.php');
$C=new Config($initCfg,$krCfg,$seoCfg,$wpCfg);
//echo $C->getSiteTitle();
/**
 * A:全局配置config
 */
$config=array(
    'domain'=>$initCfg['domain'],
    'domainFix'=>$initCfg['siteKeyFix'],
    'charset'=>DB_CHARSET,
    'timearea'=>TIMEAREA,
    'webKeyWord'=>$C->getSiteKeyWord(),
    'webTitle'=>$C->getSiteTitle()
);

/**
 * F:wordpress主题相关参数
 */

$wpOptsCfg=array(
    'blogname'=>$config['webTitle'],
    'blogdescription'=>$C->getBlogDescption(),
    'permalink_structure'=>'/%post_id%',
    'ping_sites'=>null,
    'posts_per_page'=>10,
    'template'=>'twentyten',
    'stylesheet'=>'twentyten',
    'default_link_category'=>2,
    'gmt_offset'=>8,
    'active_plugins'=>array(
        'wp-pagenavi/wp-pagenavi.php',
        'google-sitemap-generator/sitemap.php',
        'wp-postviews-plus/postviews_plus.php'
    )
);

$themeCfg=$C->getCfgByHostSiteFix();

/**
 * 美化程序(Staitc)的相关路径
 */
define('BASE_URI',$C->getCurSiteUrl());
define('STATIC_DIR',BASE_URI.'/afeios/static');