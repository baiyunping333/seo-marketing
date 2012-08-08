<?php
/**
 * Created by JetBrains PhpStorm.
 * User: feizheng
 * Date: 12-7-11
 * Time: 下午3:17
 * To change this template use File | Settings | File Templates.
 */

class Mysql
{
    public function __construct($host, $user, $pass, $database)
    {
        $this->conn = mysql_connect($host, $user, $pass) or die(mysql_error());
        mysql_select_db($database) or die(mysql_error());
        mysql_query("SET NAMES UTF8");
    }

    /**
     * @param $inSqlString
     * @return resource
     */
    public function query($inSqlString)
    {
        return mysql_query($inSqlString, $this->conn);
    }

    /**
     * @param $inTable
     * @param $inData
     * @return int
     */
    public function insert($inTable, $inData)
    {
        foreach ($inData as $key => $value) {
            $sKeys[] = "`{$key}`";
            $sValues[] = "'{$value}'";
        }
        $sql = sprintf("INSERT INTO `%s` (%s) VALUES (%s)", $inTable, implode(",", $sKeys), implode(",", $sValues));
        if (!$this->query($sql)) exit;
        return mysql_insert_id($this->conn);
    }

    /**
     * @param $inTable
     * @param $inWhere
     * @return int
     */
    public function delete($inTable, $inWhere)
    {
        $sql = sprintf("DELETE FROM `%s` WHERE (%s)", $inTable, $inWhere);
        if (!$this->query($sql)) exit;
        return mysql_affected_rows($this->conn);
    }

    /**
     * @param $inTable
     * @param $inData
     * @param $inWhere
     * @return int
     */
    public function update($inTable, $inData, $inWhere)
    {
        foreach ($inData as $key => $value) {
            $sRes[] = "`{$key}`='{$value}'";
        }
        $sql = sprintf("UPDATE `%s` SET %s WHERE (%s)", $inTable, implode(",", $sRes), $inWhere);
        if (!$this->query($sql)) exit;
        return mysql_affected_rows($this->conn);
    }

    /**
     * @param $inQueryRes
     * @param int $inType
     * @return array
     */
    public function fetchArray($inQueryRes, $inType = MYSQL_NUM)
    {
        while (($row = mysql_fetch_array($inQueryRes, $inType)) != false) {
            $resultArr[] = $row;
        }
        return $resultArr;
    }

    /**
     * @param $inSqlString
     * @param int $inType
     * @return array
     */
    public function fetch($inSqlString, $inType = MYSQL_NUM)
    {
        $query = $this->query($inSqlString);
        return $this->fetchArray($query, $inType);
    }


    public function __destruct()
    {
        mysql_close($this->conn);
    }
}