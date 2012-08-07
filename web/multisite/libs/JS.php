<?php
/**
 * @CreateTime:2010-07-18
 * @FileName:index.php
 * @Author:afei
 * @Site:http://www.afeiship.cn/
 * @E-mail:88603982@qq.com
 */

//提示框
function alert($msg) {
	printf('<script type="text/javascript">alert("%s");</script>',$msg);
}
function go($url) {
	printf('<script type="text/javascript">location.href="%s";</script>',$url);
}
//提示并跳转
function alertGo($msg,$url) {
	printf('<script type="text/javascript">alert("%s");location.href="%s"</script>',$msg,$url);
}

//返回
function goBack() {
	printf('<script type="text/javascript">history.go(-1)</script>');
}


//提示并返回
function alertGoBack($msg) {
	printf('<script type="text/javascript">alert("%s");history.go(-1)</script>',$msg);
}
//提示并刷新显示
function alertRefresh($msg) {
	printf('<script type="text/javascript">alert("%s");location.href = document.referrer;</script>',$msg);
}

//再次确认
function confirm($msg) {
	printf('<script type="text/javascript">confirm("%s");</script>',$msg);
}

//再次确认
function confirmGo($msg,$url) {
	printf('<script type="text/javascript">confirm("%s");location.href="%s"</script>',$msg,$url);
}
//提示并刷新显示
function confirmRefresh($msg) {
	printf('<script type="text/javascript">confirm("%s");location.href = document.referrer;</script>',$msg);
}