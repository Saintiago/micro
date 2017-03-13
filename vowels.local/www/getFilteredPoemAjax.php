<?
require_once("../include/common.inc.php");

try
{
    $request = new Request($_REQUEST);
    $uid = $request->getRequestParam(Config::UID_PARAM_NAME);

    $filteredPoem = null;
    while (is_null($filteredPoem))
    {
        sleep(2);
        $filteredPoem = Storage::ReadValue($uid);
    }

    echo preparePoem($filteredPoem);
}
catch (Exception $exception)
{
    echo $exception->getMessage() . "\n";
}