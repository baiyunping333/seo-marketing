<?php
/**
 * Created by JetBrains PhpStorm.
 * User: ori
 * Date: 12-8-10
 * Time: 上午10:00
 * To change this template use File | Settings | File Templates.
 */

/**
 * 从内存数据库或者本地获取数据
 * @param $key
 * @param $valuePath
 * @param Memcached $mem
 * @param int $time
 * @param bool $isMemcached
 * @return mixed
 */
function GetContent($key,$valuePath,$mem,$time=0,$isMemcached){
    if($isMemcached){
        $v=$mem->get($key);
        if($v==''){
            $v=file_get_contents($valuePath);
            $mem->set($key,$v,$time);
        }
    }else{
        $v=file_get_contents($valuePath);
    }
    return $v;
}

/**
 * 得到路径epath
 * @param $key
 * @return string
 */
function GetEPath($key){
    global $cfg;
    //判断是否主关键词
    if(isset( $cfg->mainKeys[$key])){
        $no=$cfg->mainKeys[$key];
        $epath=$cfg->subDomains[ $no ].'/';
        //echo "<br>-------<br>".$epath."<br>-------<br>";
    }else{
        $ename=KeyToEname($key,$cfg->subDomains);
        $epath=$ename.'/'.Md5To36(md5($key),8,10).'/';
    }
    return $epath;
}

/**
 * 按关键词得到二级域名(不同关键词对应不同的二级域名了)
 * @param $key
 * @param $subDomains
 * @return mixed
 */
function KeyToEname($key,$subDomains){
    $noOfDomains=count($subDomains);
    $toTenKey=ToTen(substr(md5($key),0,2));//按关键词获取特征码
    $indexOfDomains=$toTenKey % $noOfDomains;//求余，按余数来确定是用二级域名数组中的哪个
    return $subDomains[$indexOfDomains];
}

/**
 * 16进制到10进制
 * @param $num
 * @return int
 */
function ToTen($num){
    $arrTrans=array('0'=>0,'1'=>1,'2'=>2,'3'=>3,'4'=>4,'5'=>5,'6'=>6,'7'=>7,'8'=>8,'9'=>9,'a'=>10,'b'=>11,'c'=>12,'d'=>13,'e'=>14,'f'=>15);
    $result=0;
    for($i=0;$i<strlen($num);$i++){
        $result+=$arrTrans[ substr($num,$i,1)]*pow(16,strlen($num)-$i-1);
    }
    return $result;
}

/**
 * 16进制转36进制
 * @param $num
 * @param $start_no
 * @param $len
 * @return string
 */
function Md5To36($num,$start_no,$len){
    $arrTrans=array('0','1','2','3','4','5','6','7','8','9','a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z');
    $tenNo=ToTen(substr($num,$start_no,$len));
    $resultNo='';
    do{
        $a=ceil($tenNo/36);
        $b=fmod(floatval($tenNo),36);
        $tenNo=$a;
        $resultNo=$arrTrans[$b].$resultNo;
    }while($tenNo>=36);
    $resultNo=$arrTrans[$tenNo].$resultNo;
    return $resultNo;
}

/**
 * 储存
 * @param $body
 * @param $path
 */
function CachePutContents($body,$path){
    CreateDir(dirname($path));
    $handle=fopen($path,'w');
    fwrite($handle,$body);
    fclose($handle);
}

/**
 * 建立目录
 * @param $path
 */
function CreateDir($path){
    if (!file_exists($path)){
        createDir(dirname($path));
        mkdir($path, 0777);
    }
}

/**
 * curl方式抓取远程页面
 * @param $url
 * @return mixed
 */
function CurlGet($url){
    $ch = curl_init();
    curl_setopt($ch, CURLOPT_URL, $url);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
    curl_setopt($ch, CURLOPT_HEADER, 0);
    $data = curl_exec($ch);
    curl_close($ch);
    return $data;
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
function InStr($firStr,$lastStr,$obStr) {
    $arr_temp=explode($firStr,$obStr);
    $arr_temp1=explode($lastStr,$arr_temp[1]);
    return $arr_temp1[0];
}
