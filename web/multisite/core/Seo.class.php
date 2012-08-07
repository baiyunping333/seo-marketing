<?php
/**
 * Created by JetBrains PhpStorm.
 * User: afei
 * Date: 11-8-20
 * Time: 下午4:07
 * To change this template use File | Settings | File Templates.
 */
class Seo extends Krcj{
    function __construct(){
        parent::__construct();
    }

    /**
     * 功能：随机文章次序
     * @param $arr
     * @return bool
     */
    function randArr($arr){
        for($i=0;$i<$this->seoCfg['randArrNum'];$i++){
            shuffle($arr);
        }
        return $arr;
    }

    /**
     * 功能：获取名人名言Array
     * @return array
     */
    function getQuote() {
        $quotesArr=$this->dirExpendArr($this->seoCfg['quotesDir']);
        return $this->transToUtf8($quotesArr);
    }




    /**
     * 功能：获取带链接的长尾词Array
     * @return array
     */
    function getKeyLinks(){
        $ltailArr=$this->getLtailsArr();
        foreach($ltailArr as &$v){
            $v=$this->wrapLink($this->siteUrl,$v);
        }
        return $ltailArr;
    }

    /**
     * 功能：Str中随机位置插入随机Keylink
     * @param $str
     * @param $keyLinks
     * @return string
     */
    function insertKeyLinks($str,$keyLinks) {
        $charset=$this->config['charset'];
        $str=strip_tags($str);//防止HTML的P标记被插乱码
        $len=mb_strlen($str,$charset);
        //产生一个大于0而小于$len的随机数
        $randNum=rand(0,$len);
        //字符串随机分割成两部分
        $strStart=mb_substr($str,3,$randNum,$charset);
        $strEnd=mb_substr($str,$randNum,$len-4,$charset);
        //从keyLinks中随机取出一个关键词
        $randKeys=$this->getNumRandFromArr($keyLinks);
        return $this->wrapTags("p",$strStart.$randKeys.$strEnd);
    }

    /**
     * 功能：单篇文章当中随机插入几个关键词
     * @param $content
     * @return array
     */
    function insertNumKeysToContent($content) {
        $num=$this->seoCfg['keyPerContent'];
        if(count($content)<$num){
            $num=count($content);
        }
        $randConKey=array_rand($content,$num);
        foreach($randConKey as $key) {
            $content[$key]=$this->insertKeyLinks($content[$key],$this->getKeyLinks());
        }
        return $content;
    }

    /**
     * 功能：添加nextpage标签
     * @param $content
     * @return array
     */
    function addNextTag($content) {
        $newAriticle=array_chunk($content,$this->wpCfg['numNextTag']);
        $myArr=array();
        $lastP=array_pop($newAriticle);
        foreach($newAriticle as $article) {
            array_push($article,$this->wpCfg['nextTag']);
            $myArr=array_merge($myArr,$article);
        }
        $myArr=array_merge($myArr,$lastP);
        return $myArr;
    }

    /**
     * 功能：添加more标签
     * @param $content
     * @return array
     */
    function addMoreTag($content) {
        array_splice($content,$this->wpCfg['numMoreTag'],0,$this->wpCfg['moreTag']);
        return $content;
    }

    /**
     * 功能：单篇博文"Content"伪原创
     * @param $content
     * @return array|bool
     */
    function wycContent($content) {
        //1.在content数组中 随机插入N个关键词
        $content=$this->insertNumKeysToContent($content);
        //2.段落随机排列
        $content=$this->randArr($content);
        //3.添加页头，名人名言，页尾
        $content=$this->addPageTip($content,$this->getTipArr(),"head");
        $content=$this->addPageTip($content,$this->getNumRandFromArr($this->getQuote(),5),"foot");
        $content=$this->addPageTip($content,$this->getTipArr(),"foot");
        //4：添加nextpage标签
        $content=$this->addNextTag($content);
        //5.添加more标签
        //$content=$this->addMoreTag($content);
        return $content;
    }

    /**
     * 功能：功能：获得最终我们想要的样博文即我们要插入数据库的博文
     * @param $articles
     * @param bool $conStr
     * @return array
     */
    function wycArticles($articles,$conStr=false) {
        $seoArticles=array();
        foreach($articles as $article) {
            //标题
            $randCateName=get_cat_name($this->wp_getRandCatId());
            $article['title']=$this->addFix($article['title'],$randCateName);
            //echo $article['title'];
            if($conStr){
                $article['content']=$this->wycContentStr($article['content']);
            }else{
                $article['content']=$this->wycContent($article['content']);
            }
            $seoArticles[]=array(
                'title'=>$article['title'],
                'content'=>$article['content']
            );
        }//end_foreach
        return $seoArticles;
    }//end_func

    /**
     * 功能：内容的String
     * @param $content
     * @return string
     */
    function wycContentStr($content) {
        $content=$this->wycContent($content);
        return implode("\n",$content);
    }


}
