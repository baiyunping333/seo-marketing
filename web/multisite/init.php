<?php
/**
 * Created by JetBrains PhpStorm.
 * User: afei
 * Date: 11-8-27
 * Time: 上午12:30
 * To change this template use File | Settings | File Templates.
 */

/**
 * 文件：config.php
 * 功能：afeios初始化，常用库引入
 */
header('Content-Type:text/html;charset=utf-8');
date_default_timezone_set('Asia/shanghai');
ini_set('max_execution_time', 0);
ini_set('memory_limit', '128M');

/**
 * 常用常量定义
 */
define('AFEIOS_PATH', str_replace('\\', '/', dirname(__FILE__)));
define('AFEIOS_CORE', AFEIOS_PATH . '/core');
define('AFEIOS_LIBS', AFEIOS_PATH . '/libs');

define('WP_PATH', 'D:/wamp/www/wordpress');
define('CHARSET', 'UTF-8');
define('TIMEAREA', 'Asia/shanghai');
define('DB_HOST',"localhost");
define('DB_USER',"root");
define('DB_PASSWORD',"");
define('DB_NAME',"test");

/**
 * 自己的常用类库及函数
 */
include_once(AFEIOS_LIBS . '/Mysql.class.php');
include_once(AFEIOS_LIBS . '/MyFile.class.php');
include_once(AFEIOS_LIBS . '/PinYin.php');
include_once(AFEIOS_LIBS . '/JS.php');


/**
 *常用类的Init初始化
 */
$file = new MyFile();
$fDir = new MyDir();
$db = new Mysql(DB_HOST, DB_USER, DB_PASSWORD, DB_NAME);
