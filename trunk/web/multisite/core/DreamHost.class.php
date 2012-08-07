<?php
/**
 * Created by JetBrains PhpStorm.
 * User: afei
 * Date: 11-9-22
 * Time: 下午11:15
 * To change this template use File | Settings | File Templates.
 */

class DreamHost extends WpSetting{
    function __construct(){
        global $wpdb,$initCfg;
        $this->wpdb=$wpdb;
        $this->initCfg=$initCfg;
        parent::__construct();
    }

    function getSubDomainJsArr(){
        return $this->arrPhpToJs($this->getHostListArr());
    }
}