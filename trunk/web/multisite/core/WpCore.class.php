<?php
/**
 * Created by JetBrains PhpStorm.
 * User: afei
 * Date: 11-8-20
 * Time: 下午4:08
 * To change this template use File | Settings | File Templates.
 */
class WpCore extends Seo{
    function __construct(){
        parent::__construct();
    }

    /**
     * 功能：随机获取标签Array
     * @return Array
     */
    function wp_getRandTags(){
        $keyNoLinks=$this->getLtailsArr();
        return $this->getNumRandFromArr($keyNoLinks,$this->wpCfg['tagPerArticle']);
    }

    /**
     * 功能：获取wordpress随机a链接：<a href="http://localhost/wordpress/59.html" title="祛痘产品排行榜_怎么消除痘印">祛痘产品排行榜_怎么消除痘印</a>
     * @return array|bool
     */
    function wp_getRandLinks() {
        $num=$this->wpCfg['relaLinkNum'];
        $rand_posts = get_posts("numberposts=$num&orderby=rand");
        if(is_array($rand_posts)){
            foreach( $rand_posts as &$post ){
                $post=sprintf('<a href="%1$s" title="%2$s" class="cRed">%2$s</a>',$post->guid,$post->post_title);
            }
            return $rand_posts;
        }else{
            return false;
        }
    }


    /**
     * 功能：提取标题中的分类
     * @param $title
     * @return array
     */
    function wp_getCateIdFromTitle($title){
        return array(get_cat_ID(current(explode("_",$title))));
    }//end_func

    /**
     * 功能：删除所有分类
     * @return void
     */
    function wp_deleteAllCategories() {
        foreach($this->wpCateIds as $id) {
            wp_delete_term( $id, 'category' );
        }
    }

    /**
     * 功能：从长尾词中随机获取几个分类
     * @return mixed
     */
    function wp_getRandCate(){
        return $this->getNumRandFromArr($this->getLtailsArr(),$this->wpCfg['catesPerSite']);
    }

    //添加分类
    /**
     * 功能：随机向wordpress中添
     * @return void
     */
    function wp_insertCategories() {
        $catesArr=$this->wp_getRandCate();
        foreach($catesArr as $cate) {
            $my_cate=array(
                'cat_name' =>$cate,
                'category_description'=>$this->config['domainTitKey']."_".$cate,
                'category_nicename' =>Pinyin($cate,'utf8')
            );
            //print_r($my_cate);exit;
            $id=wp_insert_category($my_cate);
            if($id) {
                echo "添加成功，分类名为：".$cate."<br>";
            }
        }
        printf('当前站：<span class="cRed">%s</span>分类全部添加完成！',$this->getCurSiteUrl());
    }

    function getSeoArticles(){
        $seoArticles=$this->wycArticles($this->getArticle(),true);
        $articlesNum=$this->getArticleNum();
        $articlePerDay=$this->wpCfg['articlePerDay'];
        $startPubDate=$this->wpCfg['startPubDate'];
        $seoDate=$this->getSeoDate($articlesNum,$articlePerDay,$startPubDate);
        return array_map("array_merge",$seoArticles,$seoDate);
    }

    /**
     * 功能：获取Wp表名
     * @param int $index
     * @return string
     */
    function wp_getTable($index=0){
        return $this->wpdb->prefix.$this->wpdb->tables[$index];
    }

    /**
     * 功能：清空WP的文章表
     * @return bool
     */
    function emptyPostTable(){
        $sql=sprintf("TRUNCATE TABLE `%s`",$this->wp_getTable());
        $isTruncate=$this->wpdb->query($sql);
        if(!$isTruncate){
            echo $this->getCurSiteUrl()."的Post数据库已经全部清空！";
        }
    }

    /**
     * 功能：删除post中的title重复的文章
     * @return void
     */
    function deleteSamePost(){
        $sql=sprintf('delete %1$s from %1$s , (select id from %1$s group by post_title having count(*)>1 ) as t2 where %1$s.id=t2.id;',$this->wp_getTable());
        $this->wpdb->query($sql);
    }

    /**
     * 功能：添加文章Wordpress入库
     * @return void
     */
    function wp_addPost() {
        $wpArticles=$this->getSeoArticles();
        $new_post=array();
        foreach($wpArticles as $article) {
            $new_post['post_title']=$article['title'];
            $new_post['post_content']=$article['content'];
            $new_post['post_date']=$article['pubdate'];
            $new_post['tags_input']=$this->wp_getRandTags();
            $new_post['post_category']=$this->wp_getCateIdFromTitle($article['title']);

            //一些固定参数
            $new_post['post_status']='future';
            $new_post['post_author']='1';
            $new_post['post_type']='post';
            $post_id=wp_insert_post($new_post);
            if($post_id) {
                printf("文章标题为：%s<br />",$article['title']);
            }
        }//end_foreach
        printf('当前站点为：<span class="cRed">%s</span>，此次共成功添加：<span class="cRed">%s</span>篇文章<br>',$this->getCurSiteUrl(),$this->getArticleNum());
        printf('对应导出文章表的URL为：<a href="http://%s.qiaojoe.com/afeios/index.php?action=execExportSiglePostTable">导出地址点这里</a>',$this->getHostPreFix());;

    }//end_func



}
