<?php
//数据库连接信息
$cfg_dbhost = 'localhost';		//数据库地址
$cfg_dbname = 'tk001';		//数据库名
$cfg_dbuser = 'root';			//用户名
$cfg_dbpwd = 'wbxdiablouo235';		//数据库密码
$cfg_db_language = 'utf8';		//编码 勿改
$cfg_xs_url = '/dev/shm/xiaoshuo/';	//小说路径
$cfg_tk_pid = 'mm_15557716_0_0';		//淘宝客PID

//缓存目录
$cfg_cache_path = '/var/www/tk001/cache/gyide.com/';

//站群二级域名（包括www,非空二级域名必须包含‘.’）
$cfg_arr_ename=array('www.','','bbs.','home.','forum.','blog.','i.','game.','club.','im.');
//站群主关键词，仅包括关键词就可以了
//$cfg_arr_mainkey=array('儿童帽 男童 冬'=>1,'男童冬帽'=>2,'男童毛线帽'=>3);
$cfg_arr_mainkey=array('毛绒短裤'=>1,'熊猫短裤'=>2,'时尚短裤'=>3,'新款短裤'=>4,'韩版外套'=>5,'女孩外套'=>6,'男士韩版外套'=>7,'风衣外套'=>8,'亮片短裤'=>9,'连衣裙'=>10);
//站群主域名,暂定支持单域名。多域名后期再说
$cfg_arr_host=array(
	'gyide.com'
);
?>