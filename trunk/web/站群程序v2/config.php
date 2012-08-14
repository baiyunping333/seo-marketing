<?php
/**
 * Created by JetBrains PhpStorm.
 * User: ori
 * Date: 12-8-9
 * Time: 下午11:08
 * To change this template use File | Settings | File Templates.
 */
class config{
    var $dbHost='localhost';
    var $dbName='soso1';
    var $dbUser='root';
    var $dbPassword='';
    var $storyPath='E:/xampp/xs/';
    var $cachePath='E:/xampp/htdocs/cache';
    var $isMemcached=FALSE;
    var $subDomains=array('www.','');
    var $mainKeys=array('www.xxooyy.tk'=>0,'xxooyy.com'=>1);
    var $nextDomain='a2.com';
    var $linkOut=array('a2.com','a3.com');
}

