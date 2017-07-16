<?
require_once("../include/common.inc.php");

try
{
    $request = new Request($_REQUEST);
    $poem = $request->getRequestParam(Config::POEM_PARAM_NAME);
    $tenant = $request->getRequestParam(Config::TENANT_PARAM_NAME);
    $data = [
        Config::POEM_PARAM_NAME => validate_string($poem),
        Config::TENANT_PARAM_NAME => validate_integer($tenant),
    ];
    $dataString = json_encode($data);
    $uid = Storage::WriteValues($dataString);
    redirect(Config::URL_RESULT, ['uid' => $uid]);
}
catch (Exception $exception)
{
    echo $exception->getMessage() . "\n";
}