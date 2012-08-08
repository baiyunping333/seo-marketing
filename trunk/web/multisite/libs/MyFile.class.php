<?php
/**
 * Created by JetBrains PhpStorm.
 * User: feizheng
 * Date: 12-7-10
 * Time: 下午3:00
 * To change this template use File | Settings | File Templates.
 */
class MyFile
{
    /**
     * @param $inFileName
     * @return bool
     */
    public function read($inFileName)
    {
        if (!file_exists($inFileName)) die("File not exist! FileName is:$inFileName,Code line:" . __LINE__);
        $fp = fopen($inFileName, "r");
        $fr = fread($fp, filesize($inFileName));
        fclose($fp);
        return $fr;
    }

    /**
     * @param $inFileName
     * @param $inString
     * @param string $inMode
     * @return int
     */
    public function write($inFileName, $inString, $inMode = "w")
    {
        $fp = fopen($inFileName, $inMode);
        $fw = fwrite($fp, $inString);
        fclose($fp);
        return $fw;
    }

    /**
     * @param $inFileName
     * @return bool
     */
    public function delete($inFileName)
    {
        if (!file_exists($inFileName)) die("File not exist! FileName is:$inFileName,Code line:" . __LINE__);
        return unlink($inFileName);
    }

    /**
     * @param $inSrcFileName
     * @param $inTargetFileName
     * @return bool
     */
    public function move($inSrcFileName, $inTargetFileName)
    {
        if (!file_exists($inSrcFileName)) die("File not exist! FileName is:$inSrcFileName,Code line:" . __LINE__);
        return rename($inSrcFileName, $inTargetFileName);
    }

    /**
     * @param $inFileName
     * @param string $inFormatString
     * @return array
     */
    public function readScanf($inFileName,$inFormatString="%s\n")
    {
        if (!file_exists($inFileName)) die("File not exist! FileName is:$inFileName,Code line:" . __LINE__);
        $handle = fopen($inFileName, "r");
        while ($line = fscanf($handle, $inFormatString)) {
            $res[]=$line;
        }
        fclose($handle);
        return $res;
    }
}

class MyDir extends MyFile
{
    /**
     * @param $inPath
     * @return bool
     */
    public function create($inPath)
    {
        if (file_exists($inPath)) die("Folder exist! The path is:$inPath,Code line:" . __LINE__);
        return mkdir($inPath, 0777, true);
    }

    /**
     * @param $inPath
     * @param bool $self
     * @return bool
     */
    public function delete($inPath, $self = true)
    {
        if (!file_exists($inPath)) die("Path is not exist! The path is:$inPath,Code line:" . __LINE__);
        $dh = opendir($inPath);
        while ($res = readdir($dh)) {
            if ($res == "." || $res == "..") continue;
            $fullpath = $inPath . "/" . $res;
            if (is_dir($fullpath)) {
                $this->delete($fullpath, $self);
            } else {
                unlink($fullpath);
            }
        }
        closedir($dh);
        return $self && rmdir($inPath);
    }

    /**
     * @param $inSrcPath
     * @param $inTargetPath
     * @return void
     */
    public function xCopy($inSrcPath, $inTargetPath)
    {
        if (!file_exists($inSrcPath)) die("Path is not exist! The path is:$inSrcPath,Code line:" . __LINE__);
        $srcPaths = $this->getWinPath($inSrcPath);
        $diffDir = $inTargetPath . $srcPaths["right"];
        $this->create($diffDir);
        $dh = opendir($inSrcPath);
        while ($res = readdir($dh)) {
            if ($res == '.' || $res == '..') continue;
            if (is_dir($inSrcPath . '/' . $res)) {
                $this->xCopy($inSrcPath . '/' . $res, $inTargetPath);
            } else {
                copy($inSrcPath . '/' . $res, $diffDir . '/' . $res);
            }
        }
        closedir($dh);
    }

    /**
     * @param $inPath
     * @return array
     */
    private function  getWinPath($inPath)
    {
        $result = array();
        $result['right'] = strstr($inPath, '/');
        $result['left'] = rtrim($inPath, $result['right']);
        return $result;
    }

}
