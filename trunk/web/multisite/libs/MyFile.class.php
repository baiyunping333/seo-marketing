<?php
/**
 * @CreateTime:2010-5-8
 * @FileName:MyFile.class.php
 * @Author:afei
 */

/**
 * ###############################
 * File文件操作部分
 * $filename，文件名，如：c:/afei/afei.txt
 * ###############################
 */
class MyFile
{
	private $error=null;
	protected $e_config=array(
		'err_file_exist'=>'文件或目录不存在!',
		'err_file_readable'=>'文件或目录不可读!',
		'err_file_writeable'=>'文件或目录不可写!',
		'err_dir_exist'=>'目录已存在!'
	);
	public $dirs=array();
	###错误信息处理##############################
	protected function setError($err)
	{
		return $this->error=$err;
	}

	//外部获取错误信息
	public function getError()
	{
		if($this->error){
			return sprintf("%s",self::setError($this->error));
		}
	}

	####错误detection##############################
	protected function detecError($filename,$code)
	{
		switch($code){
			###File操作部分################################
			//1=>exist
			case '1':
				if(!file_exists($filename)){
					$this->setError($filename.$this->e_config['err_file_exist']);
					return false;
				}else{
					return true;
				}
			break;

			//2=>writable
			case '2':
				if(!is_writable($filename)){
					$this->setError($filename.$this->e_config['err_file_writeable']);
					return false;
				}else{
					return true;
				}
			break;

			//3=>readable
			case '3':
				if(!is_readable($filename)){
					$this->setError($filename.$this->e_config['err_file_readable']);
					return false;
				}else{
					return true;
				}
			break;

			###Dir操作部分################################
			//4=>exist已存在此目录
			case '4':
				if(file_exists($filename)){
					$this->setError($filename.$this->e_config['err_dir_exist']);
					return false;
				}else{
					return true;
				}
			break;
		}
	}

	//读
	public function read($filename,$mode='r')
	{
		if(self::detecError($filename,1)&&self::detecError($filename,3)){
			$fp=fopen($filename,$mode);
			$fr=fread($fp,filesize($filename));

			fclose($fp);
			return $fr;
		}
	} //end read();


	//写
	public function write($filename,$string,$mode)
	{
		if(self::detecError($filename,1)&&self::detecError($filename,2)){
			$fp=fopen($filename,$mode);
			$fw=fwrite($fp,$string);
			fclose($fp);
			return $fw;
		}
	} //end write();


	//删
	public function delete($filename)
	{
		if(self::detecError($filename,1)&&self::detecError($filename,2)){
			return @unlink($filename);
		}
	} //end delete();


	//移动[重命名]
	public function move($filename,$newname)
	{
		if(self::detecError($filename,1)){
			return @rename($filename,$newname);
		}
	} //end move();


} //end MyFile2.class;

/**
 * ###############################
 * Dir目录操作部分
 * $filename:目录名，如：c:/afei/
 * ###############################
 */

class MyDir extends MyFile
{

	private function get2Str($str)
	{
		$result=array();
		$result['right']=strstr($str,'/');
		$result['left']=rtrim($str,$result['right']);
		return $result;
	}
	//创建createDir
	public function createDir($filename)
	{
		if(self::detecError($filename,4)){
			return mkdir($filename,0777,true);
		}
	} //end createDir();


	//删除deleteDir
	public function deleteDir($filename,$self=true)
	{
		if(!self::detecError($filename,1)){
			return false;
		}

		$result=self::listDir($filename);
		foreach(array_reverse($result) as $tmp){
			if(is_dir($tmp)){
				@rmdir($tmp);
			}else{
				$this->delete($tmp);
			}
		}
		if($self){
			rmdir($filename);
		}
		return true;
	} //end deleteDir();


	//复制copyDir
	public function copyDir($filename,$newpath)
	{
		if(!self::detecError($filename,1)){
			return false;
		}
		//1.listDir[如：c:/afei/到d:/test/]
		//2.在$filename里对应循环建立目录$newpath.ltrim(strstr($dir,'/'),'/');
		//3.循环执行copy('c:/afei/afei.txt','d:/test/afei/afei.txt');
		//self::createDir($newpath);
		$oldpath=self::get2Str($filename);
		self::createDir($newpath.$oldpath['right']);
		$rnewpath=rtrim($newpath,'/');
		$result=self::listDir($filename);
		foreach($result as $tmp){
			$tmpstr=self::get2Str($tmp);
			if(is_dir($tmp)){
				//创建目录
				self::createDir($rnewpath.$tmpstr['right']);
			}else{
				copy($tmp,$rnewpath.$tmpstr['right']);
			}
		}
		return true;
	}

	//移动[重命名]moveDir
	public function moveDir($filename,$newpath)
	{
		self::copyDir($filename,$newpath);
		self::deleteDir($filename);
		return true;
	}

	//列目录listDir[如果列多个没有关闭就会出问题]
	private function listData($filename)
	{
		$sd=scandir($filename);
		$sd=array_slice($sd,2);
		foreach($sd as $rs){
			if(is_dir($filename.$rs)){
				array_push($this->dirs,$filename.$rs."/");
				self::listData($filename.$rs."/");
			}else{
				array_push($this->dirs,$filename.$rs);
			}
		}
	}

	public function listDir($filename)
	{
		if(!self::detecError($filename,1)){
			return false;
		}
		self::listData($filename);
		$tmp=$this->dirs;
		$this->dirs=array();
		return $tmp;
	}

} //end MyFolder.class;


/**
 *
##文件操作#########################
//读
$filename='c:/afei/afei.txt';
$myfile=new MyFile();
if(($fr=$myfile->read($filename,'rb'))==true){
	echo "$fr";
}else{
	echo $myfile->getError();
}

//写
$filename='c:/afei/afei.txt';
$myfile=new MyFile();
$fr=$myfile->write($filename,'Test a String','a');
if($fr){
	echo "ok";
}else{
	echo $myfile->getError();
}

//删除：
$filename='c:/afei/afei.txt';
$myfile=new MyFile();
$fr=$myfile->delete($filename);
echo $myfile->getError();

//重命名：
$filename='c:/afei/afei.txt';
$myfile=new MyFile();
$fr=$myfile->move($filename,'c:/afei/afei2.txt');
if($fr){
	echo "ok";
}else{
	echo $myfile->getError();
}

##目录操作#########################
//创建
$filename='c:/123/122t/';
$fd=new MyDir();
if ($fd->createDir($filename)){
	echo "ok";
}else {
	echo $fd->getError();
}

//删除
$filename='c:/123/';
$fd=new MyDir();
	$ar=$fd->deleteDir($filename);
	if ($ar){
		echo "ok";
	}else {
		echo "failed";
	}
	//print_r($ar);

 //复制
$filename='c:/afei/';
$fd=new MyDir();
	$ar=$fd->copyDir($filename,'d:/test/');
	if ($ar){
		echo "ok";
	}else {
		echo "failed";
	}

//移动
$filename='c:/123/';
$fd=new MyDir();
	$ar=$fd->moveDir($filename,'d:/afei0/');
	if ($ar){
		echo "ok";
	}else {
		echo "failed";
	}
//列目录
$filename='c:/test/';
$fd=new MyDir();
$arr=$fd->listDir($filename);
print_r($arr);
echo "<hr>";


$filename='c:/Youku/';
$arr2=$fd->listDir($filename);
print_r($arr2);
 */
 /*
$filename='c:/test/';
$fd=new MyDir();
	$ar=$fd->copyDir($filename,'d:/test2/');
	if ($ar){
		echo "ok";
	}else {
		echo "failed";
	}
	 */
