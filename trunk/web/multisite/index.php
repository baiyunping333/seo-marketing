<?php
include_once('init.php');
include_once('config.php');
include_once(AFEIOS_CORE.'/Common.class.php');
include_once(AFEIOS_CORE.'/Krcj.class.php');
include_once(AFEIOS_CORE.'/Seo.class.php');
include_once(AFEIOS_CORE.'/WpCore.class.php');
include_once(AFEIOS_CORE.'/MultiSite.class.php');
include_once(AFEIOS_CORE.'/WpSetting.class.php');
include_once(AFEIOS_CORE.'/Exec.class.php');

$action=isset($_REQUEST['action'])?$_REQUEST['action']:'index';

//实例化对象
$KR=new Exec();
$KR->{$action}();

?>
<!DOCTYPE HTML>
<html lang="en-US">
<head>
	<meta charset="UTF-8">
	<title>站群v1.0</title>
    <link href="<?php echo STATIC_DIR; ?>/css/style.css"  rel="stylesheet" type="text/css" />
</head>
<body>
    <div class="wrap">
        
    </div>
</body>
</html>
