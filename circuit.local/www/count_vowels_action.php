<?
require_once("../include/common.inc.php");

try
{
    $request = new Request($_REQUEST);
    $value = $request->getRequestParam(Config::POEM_PARAM_NAME);
    validate_string($value);
    $uid = Storage::WriteValue($value);
    redirect(Config::URL_RESULT, ['uid' => $uid]);
}
catch (Exception $exception)
{
    echo $exception->getMessage() . "\n";
}