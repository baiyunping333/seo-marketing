<?php 
/**
*@author:afei
*@updata:2010-06-12
*/
class Mysql {
	//初始化
	public function __construct($host,$user,$pass,$database) {
		$this->conn=mysql_connect($host,$user,$pass) or die("Error: ".mysql_error());
		mysql_select_db($database) or die("Error: ".mysql_error());
		mysql_query("set names utf8");
	}

	//直接执行的Query
	public function query($sql) {
		return mysql_query($sql);
	}

	//查询，返回数组
	public function queryArr($sql) {
		$result=mysql_query($sql);
		if(!mysql_num_rows($result)) return 0;
		$resultArr=array();
		while(($row=mysql_fetch_object($result))!=false){
			$resultArr[]=$row;
		}
		return $resultArr;
	}
	
	//给出资源，得到结果
	public function fetchArray($query) {
		$resultArr=array();
		while(($row=mysql_fetch_object($query))!=false){
			$resultArr[]=$row;
		}
		return $resultArr;
	}

	####################
	#@查，添，删，改
	#@相关便捷操作
	####################

	//Select查找：仿Ku6写法
	public function select($table,$filed,$where='',$limit='',$order='',$group='',$force='')
	{
		if($table=='' or $filed==''){
			return false;
		}
		if($where==""){
			$where="1=1";
		}
		$forceindex=$force==''?'':' force index('.$force.') ';
		$where=$where==''?'':' where '.$where;
		$order=$order==''?'':' order by '.$order;
		$group=$group==''?'':' group by '.$group;
		$limit=$limit==''?'':' limit '.$limit;
		$sql='select '.$filed.' from `'.$table.'`'.$forceindex.$where.$group.$order.$limit;
		return $this->queryArr($sql);
	}

	//插入
	public function insert($table,$data)
	{
		if($table=='' or $data==''){
			return false;
		}
		$content=self::dealData($data);
		foreach($content as $k=>$v){
			$sfield.="`$k`,";
			$svalue.=(self::isFun($v))?("$v,"):("'$v',");
		}
		$sfield=rtrim($sfield,',');
		$svalue=rtrim($svalue,',');
		$sql="insert into $table ($sfield) values ($svalue)";
		//echo $sql;exit;
		mysql_query($sql);
		return mysql_insert_id();
	}
	
	//删除
	public function del($table,$where)
	{
		/*
		if($table="" or $where=""){
			return false;
		}
		*/
		$sql=sprintf("delete from %s where %s",$table,$where);
		mysql_query($sql);
		return mysql_affected_rows();
	}
	
	//更新
	public function update($table,$data,$where="")
	{
		if($table=='' or $data==''){
			return false;
		}
		if($where==""){
			$where="1=1";
		}
		$content=self::dealData($data);
		foreach($content as $k=>$v){
			$kv.=$k."="."'$v',";
		}
		//可以直接使用impode(',',$content)实现
		$kv=substr($kv,0,-1);
		$sql="update $table set $kv
		          where ($where)";
		$result=mysql_query($sql);
		return mysql_affected_rows();
	}

	//过滤
	private function filter($value)
	{
		if(!is_array($value)){
			$result=mysql_real_escape_string($value);
		}else{
			foreach($value as $k=>$v){
				$result[$k]=mysql_real_escape_string($v);
			}
		}
		return $result;
	}

	//判断SQL函数;直接以()结束的就是函数
	private function isFun($value)
	{
		$str=substr($value,-2);
		if($str=="()"){
			return true;
		}else{
			return false;
		}
	}
	
	/**
	 * 可以把传进来的两个数组变成一个数组
	 * 如果是一个数组即直接返回
	 * 数据可以这样获取$result = compact("firstname", "lastname", "age");
	 * 1.$field=array('usrename','password');,$value=array('afeiship','1234560');
	 * 2.$field=array('username'=>'afeiship','password'=>'123465');
	 * 3.$field="username=afeiship&&password=123456";
	 */
	private function dealData($data)
	{
		$dealData=array();
		//字段，值分1或2个数组传入
		if(!is_array($data)){
			parse_str($data,$dealData);
		}else{
			$dealData=$data;
		}
		$dealData=$this->filter($dealData);
		return $dealData;
	}

	//关闭连接
	public function __destruct() {
		mysql_close($this->conn);
	}
	//连接数据库并设置编码

}//end Mysql.class
?>