<?php
//远程抓取
function file_get_contents_s($url){
	$ch = curl_init();
	curl_setopt($ch, CURLOPT_URL, $url);
	curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
	curl_setopt($ch, CURLOPT_HEADER, 0);
	$data = curl_exec($ch);
	curl_close($ch);
	//$data=file_get_contents($url);
	return $data;
}

//百度知道列表
function get_hot($key,$pg) {
	$key = iconv("UTF-8", "GBK//IGNORE",$key);
	$data=file_get_contents_s('http://zhidao.baidu.com/q?ct=17&tn=ikaslist&rn=10&word='.urlencode($key).'&lm=0&pn='.($pg*10));
	$data = iconv("GBK", "UTF-8", $data);
	$s_arr = array();
	if (preg_match_all('%<a href="/question/([0-9]{5,15})[^>]+>([\s\S]+?)</a>[\s\S]+?<span class="search_content">([\s\S]+?)<br>%sim', $data, $arrTemp)) { // 站内搜索
		$ids = $arrTemp[1];
		$titles = $arrTemp[2];
		$descriptions = $arrTemp[3];
	}
	//是否包含下一页
	$next_page=0;
	if(!strpos($data,'下一页')===false){
		$next_page=1;
	}

	unset($data,$arrTemp);
	$content = array();
	$content['ids'] = $ids; //列表
	$content['titles'] = $titles; //标题
	$content['descriptions'] = $descriptions; //描述
	$content['next_pg'] = $next_page; //下一页
	//$content['keys'] = $k_arr; //关键词组
	return $content;
}

//百度知道内容
function get_view($id){
	$contents=array();
	$data=file_get_contents_s('http://zhidao.baidu.com/question/'.$id.'.html');
	$data = iconv("GBK", "UTF-8", $data);

	//标题
	$title='';
	if(preg_match('%<title>([^>]+?)_%sim',$data,$arr)){
		$title=$arr[1];
	}

	//描述
	$question_description='';
	if(preg_match('%<pre id="question-content">([^<]+)</pre>%sim',$data,$arr)){
		$question_description=trim($arr[1]);
	}

	//最佳答案
	$answers='';
	if(preg_match('%<pre id="best-answer-content[^>]+?>([^<]+?)</pre>%sim', $data,$arr)){
		$answers[]=trim($arr[1]);
	}

	//其他答案
	if(preg_match_all('%<pre class="reply[^>]+?>([^<]+?)</pre>%sim', $data,$arr)){
		foreach($arr[1] as $r){
			$answers[]=trim($r);
		}
	}

	$contents['title'] = $title;
	$contents['miaoshu'] = $question_description;
	$contents['answers'] = $answers;
	unset($arr,$data);
	return($contents);
}

//存储
function cache_put_contents($body,$path){
	createDir(dirname($path));
	$handle=fopen($path,'w');
	fwrite($handle,$body);
	fclose($handle);
}
function createDir($path){
	if (!file_exists($path)){
		createDir(dirname($path));
		mkdir($path, 0777);
	}
}
//存储目录
function cache_get_path($str,$path){
	$str=MD5($str);
	$path.=substr($str,0,2).'/';
	$path.=substr($str,2,2).'/';
	$path.=substr($str,4,2).'/';
	$path.=substr($str,6,28);
	return $path;
}

//替换标点
function replace_db($text,$newdb){
	$arr_db=explode("|","，|。|！|～|《|》|（|）|“|”|‘|’|？|【|】|、|…|-|：|,|.|~|<|>|(|)|'|?|[|]|!|^");
	return str_replace($arr_db,$newdb,$text);
}
//连续标点替换
function replace_dbs($text,$newdb){
	$bd="，|。|！|～|《|》|（|）|“|”|‘|’|？|【|】|、|…|\-|：|,|\.|~|\(|\)|'|\?|\[|\]|\!|\^";
	return preg_replace('/('.$bd.'){2,}/i',$newdb,$text);
}

//URL转换
function Toabc($number){
	$n = array('7','9','3','1','2','4','5','6','0','8');
	$w = array('o','a','n','m','u','t','v','s','i','e');
	return str_replace($n, $w, $number);
}

function To123($abc){
	$n = array('7','9','3','1','2','4','5','6','0','8');
	$w = array('o','a','n','m','u','t','v','s','i','e');
	return str_replace($w, $n, $abc);
}

//中英文字符串截取
 function substr_utf8($string,$start,$length){
	$chars = $string;
	$m=$n=0;
	$i=0;
	do{
		if (preg_match ("/[0-9a-zA-Z]/", $chars[$i])){//纯英文
			$m++;
			$i++;
		}else {
			$n++;
		}//非英文字节,
		$k = $n/3+$m/2;
		$l = $n/3+$m;//最终截取长度；$l = $n/3+$m*2？

	} while($k < $length);

	$str1 = mb_substr($string,$start,$l,'utf-8');//保证不会出现乱码
	return $str1;
}

//提取中间字符
function in_str($firStr,$lastStr,$obStr) {
	$arr_temp=explode($firStr,$obStr);
	$arr_temp1=explode($lastStr,$arr_temp[1]);
	return $arr_temp1[0];
}


function array_remove_empty(& $arr, $trim = true){
    foreach ($arr as $key => $value) {
        if (is_array($value)) {
            array_remove_empty($arr[$key]);
        } else {
            $value = trim($value);
            if ($value == '') {
                unset($arr[$key]);
            } elseif ($trim) {
                $arr[$key] = $value;
            }
        }
    }
}

//按关键词得到路径
function get_epath($key){
	global $cfg_arr_mainkey;	//主关键词
	global $cfg_arr_ename;		//二级域名组
	//判断是否主关键词
	if($cfg_arr_mainkey[$key]>0){//是主关键词
		$epath=$cfg_arr_ename[$cfg_arr_mainkey[$key]-1].'/';
	}else{
		$t_ename=keyToEname($key,$cfg_arr_ename);
		$epath=$t_ename.'/'.md5To36(md5($key),8,10).'/';
	}
	return $epath;
}

//16进制到10进制
function toTen($num){
	$arr_key=array('0'=>0,'1'=>1,'2'=>2,'3'=>3,'4'=>4,'5'=>5,'6'=>6,'7'=>7,'8'=>8,'9'=>9,'a'=>10,'b'=>11,'c'=>12,'d'=>13,'e'=>14,'f'=>15);
	$result=0;
	for($i=0;$i<strlen($num);$i++){
		$result+=$arr_key[ substr($num,$i,1)]*pow(16,strlen($num)-$i-1);
	}
	return $result;
}

//按关键词得到二级域名
function keyToEname($t_key,$t_arr_ename){
	$n=count($t_arr_ename);
	$n_key=toTen(substr(md5($t_key),0,2));//按关键词获取特征码
	$mod_key=$n_key % $n;//求余，按余数来确定是用二级域名数组中的哪个
	return $t_arr_ename[$mod_key];
}

//10进制转36进制
function md5To36($num,$start_no,$len){
	$arr_o=array('0','1','2','3','4','5','6','7','8','9','a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z');
	$o_n=toTen(substr($num,$start_no,$len));
	$r_no='';
	do{
		$a=ceil($o_n/36);
		$b=fmod(floatval($o_n),36);
		$o_n=$a;
		$r_no=$arr_o[$b].$r_no;
	}while($o_n>36);
	return $arr_o[$o_n].$r_no;
}
function clear(){
	echo '<br>';
}
?>
