<?php

require_once("../include/common.inc.php");

$request = new Request($_REQUEST);
$uid = $request->getRequestParam(Config::UID_PARAM_NAME);

?>
<!DOCTYPE html>
<html>
    <head>
        <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.0/jquery.min.js"></script>
        <script src="js/result_loader.js"></script>
    </head>
    <body>
        <div class="result_container" id="result"></div>
        <input type="hidden" id="uid" value="<?= trim($uid, '"'); ?>" />
    </body>
</html>
