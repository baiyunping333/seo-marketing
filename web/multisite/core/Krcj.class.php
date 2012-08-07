<?php
/**
 * Created by JetBrains PhpStorm.
 * User: afei
 * Date: 11-8-20
 * Time: 下午3:19
 * To change this template use File | Settings | File Templates.
 */
class Krcj extends Common{
    function __construct(){
        parent::__construct();
    }
    /**
     * 功能：获取KRDownload目录下的文件名列表Array
     * @return array
     */
    function getFileNames(){
        return $this->fDir->listDir($this->krCfg['krDownload']);
    }
    /**
     * 功能：获取KRDownload目录下的文件数量
     * @return int
     */
    function getFileNum(){
        return count($this->getFileNames());
    }

    /**
     * 功能：获取krDownload中所有的文件中的Title content数组
     * @return array
     */
    function getArticle(){
        $articles=array();
        $fileNames=$this->getFileNames();
        foreach($fileNames as $fileName) {
            $article=$this->getSingleArticle($fileName);
            if($article['title'] && $article['content']){
                $articles[]=$article;
            }
        }
        return $articles;
    }

    /**
     * 功能：获取采集文章总数
     * @return int
     */
    function getArticleNum(){
        return count($this->getArticle());
    }

    /**
     * 功能：获取单篇文章的title,content
     * @param $fileName
     * @return array
     */
    function getSingleArticle($fileName){
        $articleSingle=$this->transToUtf8(file_get_contents($fileName));
        //正则取文章标签、内容
        $mode="/D8888D贴子标题-------------------------------------------------------(.+?)D8888D主贴内容-------------------------------------------------------(.+?)/imUs";
        preg_match_all($mode,$articleSingle,$matchArr);
        $articleTit=$this->filterNR($matchArr[1][0]);
        $articleCon=$this->dealArticle($matchArr[2][0]);
        $article=array(
            'title'=>$articleTit,
            'content'=>$articleCon
        );
        return $article;
    }

    /**
     * 功能：处理KR采集器文章中的空格等
     * @param $article
     * @return array
     */
    function dealArticle($article){
        $arr=explode("\n",$article);
        $resArr=array();
        foreach($arr as $val) {
            $val=trim($val,"　　");
            $val=trim($val);
            if($val){
                $resArr[]=$this->wrapTags("p",$val);
            }
        }
        return $resArr;
    }



}
