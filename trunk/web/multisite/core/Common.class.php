<?php
/**
 * Created by JetBrains PhpStorm.
 * User: afei
 * Date: 11-8-20
 * Time: 下午11:40
 * To change this template use File | Settings | File Templates.
 */
class Common {
    function __construct(){
        global $initCfg;
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


}
