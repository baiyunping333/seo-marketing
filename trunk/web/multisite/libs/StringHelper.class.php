<?php
/**
 * Created by JetBrains PhpStorm.
 * User: feizheng
 * Date: 12-7-10
 * Time: 下午3:08
 * To change this template use File | Settings | File Templates.
 */

class StringHelper
{
    /**
     * @static
     * @param $inString
     * @return string
     */
    public function filterNR($inString)
    {
        return str_replace("/n/r", "", $inString);
    }

    public function nl2Array($inString, $inFilter = true)
    {
        $tempArr = str_split($inString, "/n/r");
        exit;
        if ($inFilter) {
            array_filter(&$tempArr);
        }
        return $tempArr;
    }

    /**
     * @static
     * @return string
     */
    public function getPath()
    {
        return str_replace('\\', '/', dirname(__FILE__));
    }

    /**
     * 功能：包裹标签
     * @param $tagName
     * @param $str
     * @return string
     */
    function wrapTags($tagName, $str)
    {
        return "<$tagName>$str</$tagName>";
    }

    /**
     * 功能：链接Wrap
     * @param $link
     * @param $str
     * @return string
     */
    function wrapLink($link, $str)
    {
        return $this->filterNR(sprintf('<a href="%1$s" title="%2$s">%2$s</a> ', $link, $str));
    }

    /**
     * 功能：在String后面加后缀，$mode=1时，即加在前面
     * @param $str
     * @param $fix
     * @param int $mode
     * @return string
     */
    function addFix($str, $fix, $mode = 0)
    {
        //前缀
        if ($mode) {
            $str = "{$str}_{$fix}";
        } else {
            $str = "{$fix}_{$str}";
        }
        return $str;
    }
}