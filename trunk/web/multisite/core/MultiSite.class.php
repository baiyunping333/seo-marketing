<?php
/**
 * Created by JetBrains PhpStorm.
 * User: afei
 * Date: 11-8-23
 * Time: 上午12:15
 * To change this template use File | Settings | File Templates.
 */
class MultiSite extends WpCore{
    function __construct(){
        parent::__construct();
    }

    /**
     * 获取默认wp_配置参数
     * @return array
     */
    function getDefaultOpts(){
        return get_alloptions();
    }
	//$KR->updateOpts('blogname','me is new blog title');
	function updateOpts( $option_name, $newvalue ){
        if($option_name=="ping_sites"){
            $this->wpdb->query($this->getPingSitesSql());
        }else{
            return update_option( $option_name, $newvalue );
        }

	}

    /**
     * 功能：批量创建win32Host文件
     * @return void
     */
    function createSiteNumLocalHosts(){
        $num=$this->initCfg['siteNum'];
        for($i=1,$hostHTML='';$i<=$num;$i++){
            $nibNum=$this->getNibble($num);
            $siteFix=$this->initCfg['siteKeyFix'].$this->getNibbleNum($i,$nibNum);
            $hostHTML.=sprintf("127.0.0.1\t%s.%s\n",$siteFix,$this->initCfg['domain']);
        }
        $hostHTML=sprintf("#TaoKeOS Test--start \n%s#TaoKeOS Test--end",$hostHTML);
        $this->file->write(OUT_FILES.'/hosts',$hostHTML,'w');
        printf('文件hosts写入成功！目标路径为：%s,本机路径为：%s',OUT_FILES.'/hosts',$this->initCfg['win32etcDir']);
    }

    /**
     * 功能：批量创建VirtualHost文件
     * @return void
     */
    function createSiteNumVirtualHost(){
        $num=$this->initCfg['siteNum'];
        for($i=1,$vhostHTML="";$i<=$num;$i++){
            $nibNum=$this->getNibble($num);
            $siteFix=$this->initCfg['siteKeyFix'].$this->getNibbleNum($i,$nibNum);
            $vhostHTML.=sprintf("<VirtualHost *>\n\tDocumentRoot \"d:/wamp/www/wordpress\" \n\tServerName %s \n</VirtualHost>\n\n",$siteFix.'.'.$this->initCfg['domain']);
        }
        $this->file->write(OUT_FILES.'/vhosts',$vhostHTML,'w');
        printf('文件hosts写入成功！目标路径为：%s,本机路径为：%s',OUT_FILES.'/vhosts',$this->initCfg['vhostDir']);
    }



}
