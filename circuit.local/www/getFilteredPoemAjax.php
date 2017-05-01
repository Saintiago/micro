<?
require_once("../include/common.inc.php");

try
{
    $request = new Request($_REQUEST);
    $uid = $request->getRequestParam(Config::UID_PARAM_NAME);

    echo preparePoem(Storage::ReadValue($uid));
}
catch (Exception $exception)
{
    echo $exception->getMessage() . "\n";
}