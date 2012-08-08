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
    'setupTimeOut'=>1
);

/**
 * 美化程序(Staitc)的相关路径
 */
define('BASE_URI',$C->getCurSiteUrl());
define('STATIC_DIR',BASE_URI.'/afeios/static');
