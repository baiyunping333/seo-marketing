<?php
/**
 * Created by JetBrains PhpStorm.
 * User: ori
 * Date: 12-8-10
 * Time: 下午9:48
 * To change this template use File | Settings | File Templates.
 */
class baidu{
    var $key;
    var $viewId;
    var $isUp=FALSE;

    //缓存内容
    var $listAllUrls=array();
    var $listNotCrawl=array();
    var $errorTime=0;
    var $title='';
    var $description='';
    var $body='';
    var $links='';      //需要同步

    /**
     * 从数据库读出的序列化缓存数据中获得分析得到内容
     * @param string $dbContent 数据库中取出的序列化数据缓存
     */
    function AnalysisCache($dbContent){
        $arrContent=unserialize($dbContent);
        $this->listAllUrls=$arrContent['listAllUrls'];
        $this->listNotCrawl=$arrContent['listNotCrawl'];
        $this->errorTime=$arrContent['errorTime'];
        $this->title=$arrContent['title'];
        $this->description=$arrContent['description'];
        $this->body=$arrContent['body'];
        $this->links=$arrContent['links'];
    }
    //因为首页一次生成3个页面，按首页的方法没法让这3个页面进行link的实时同步。
    function InsertLinks($links){
        $links.='<br>'.$this->links;
        $arrLinks=explode('<br>',$links);
        $arrLinks=array_unique($arrLinks);
        $links=implode('<br>',$arrLinks);
        if(strlen($links)-strlen($this->links)>10){     //多10个字符肯定新增连接了
            $this->links=$links;
            $this->isUp=TRUE;
        }
    }
    /**
     * 返回序列化后的缓存内容以待写入。
     * @return string
     */
    function GetSerializeCache(){
        $arrCache=array();
        $arrCache['listAllUrls']=$this->listAllUrls;
        $arrCache['listNotCrawl']=$this->listNotCrawl;
        $arrCache['errorTime']=$this->errorTime;
        $arrCache['title']=$this->title;
        $arrCache['description']=$this->description;
        $arrCache['body']=$this->body;
        $arrCache['links']=$this->links;
        $serializeCache=serialize($arrCache);
        return $serializeCache;
    }

    function SetKey($key){
        $this->key=$key;
    }
    function SetViewId($viewId){
        $this->viewId=$viewId;
    }
    /**
     * 抓取目录列表
     * @return mixed
     */
    function CrawlList() {
        $pg=$this->viewId;
        $key=$this->key;
        if ( ($this->errorTime>5) || ($this->description!='') ){   //不采集：错误次数超过5或者已采集成功
            //$this->isUp=FALSE;
            return ;
        }

        $key = iconv("UTF-8", "GBK//IGNORE",$key);
        $data=CurlGet('http://zhidao.baidu.com/search?&rn=10&word='.urlencode($key).'&lm=0&pn='.($pg*10));
        $data = iconv("GBK", "UTF-8", $data);
        //共搜到相关问题[\s]*?([0-9]+?)[\s]*?项
        //从返回结果来判断该页是否存在（百度知道当页数参数大于实际最大页数时会返回最大页数的结果）
        $totalAnswerNumber=0;
        if(preg_match_all('%共搜到相关问题[\s]*?([0-9]+?)[\s]*?项%sim', $data, $arrTemp)){
            $totalAnswerNumber=$arrTemp[1][0];
        }
        if($totalAnswerNumber<=($pg-1)*11){
            $this->errorTime+=1;
            $this->isUp=TRUE;
            return;
        }

        if (preg_match_all('%<a href="http://zhidao.baidu.com/question/([0-9]{5,15})[^>]+>([\s\S]+?)</a>[\s\S]+?<dd class[^>]+?>([\s\S]+?)</dd>%sim', $data, $arrTemp)) { // 站内搜索
            $ids = $arrTemp[1];
            $titles = $arrTemp[2];
            $descriptions = $arrTemp[3];
        }
        $data=null;
        $arrTemp=null;
        unset($data,$arrTemp);

        if(count($ids)<1){      //抓取失败
            $this->errorTime+=1;
            $this->isUp=TRUE;
            return;
        }
        $this->errorTime=0;
        //从所取列表中随机选取一条标题作为缓存中的标题；打乱所有描述后合并取前1000个字作为缓存中的描述
        $this->listAllUrls=$ids;
        $this->listNotCrawl=$ids;

        shuffle($titles);
        $this->title=strip_tags($titles[0]);
        $this->title=str_replace('...','',$this->title);

        shuffle($descriptions);
        $this->description=strip_tags(implode('<br>',$descriptions));
        $this->description=substr_utf8($this->description,0,1000);

        $this->links.='<a href=./view-'.$pg.'.html>'.$this->title.'</a><br>';     //采集成功把链接加入缓存
        $this->isUp=TRUE;
    }
    /**
     * 抓取文章
     */
    function CrawlView(){
        if(count($this->listNotCrawl)<1) {  //全部抓取完毕，不再抓取
            //$this->isUp=FALSE;
            return;
        }
        if($this->errorTime>6){             //错误超过6次，跳过
            array_splice($this->listNotCrawl,0,1);
            $this->errorTime=0;
            $this->isUp=TRUE;
            return;
        }

        $articleId=$this->listNotCrawl[0];
        $data=CurlGet('http://zhidao.baidu.com/question/'.$articleId.'.html');
        $data = iconv("GBK", "UTF-8", $data);
        //标题
        $title='';
        if(preg_match('%<title>([^>]+?)_%sim',$data,$arr)){
            $title=strip_tags($arr[1]);
        }
        if($title==''){
            $this->isUp=TRUE;
            $this->errorTime+=1;
            return;
        }
        /*描述
        $question_description='';
        if(preg_match('%<pre id="question-content">([^<]+)</pre>%sim',$data,$arr)){
            $question_description=trim($arr[1]);
        }*/

        //最佳答案
        $answers='';
        if(preg_match('%<pre id="best-answer-content[^>]+?>([^<]+?)</pre>%sim', $data,$arr)){
            $answers[]=str_replace(array('\r','\n','\r\n'),'<br>',strip_tags(trim($arr[1]))) ;
        }

        //其他答案
        if(preg_match_all('%<pre class="reply[^>]+?>([^<]+?)</pre>%sim', $data,$arr)){
            foreach($arr[1] as $r){
                $answers[]=str_replace(array('\r','\n','\r\n'),'<br>',strip_tags(trim($r)));
            }
        }
        $data=null;
        $arr=null;
        unset($data,$arr);
        $arrBody=explode('<br>',$this->body);
        $arrBody=array_merge($arrBody,$answers);
        if(count($arrBody)<=1){
            $body=$arrBody[0];
        }else{
            shuffle($arrBody);
            $body=implode('<br>',$arrBody);
        }
        $body=str_replace($title,'',$body);
        $arrBody=explode('<br>',$body);
        shuffle($arrBody);
        $body=implode('<br>',$arrBody);
        if($this->title==''){$this->title=$title;}
        $this->errorTime=0;
        if(strlen($body)>20000){
            //body达到20K，不再抓取。
            $body=substr_utf8($body,0,20000);
            $this->body=$body;
            $body=null;
            $this->listNotCrawl=array();
            $this->isUp=TRUE;
            return;
        }
        $this->body=$body;
        $body=null;
        array_splice($this->listNotCrawl,0,1);  //去掉成功的文章
        $this->isUp=TRUE;
    }

}