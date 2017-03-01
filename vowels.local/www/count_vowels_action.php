<?

require_once("../include/common.inc.php");

// TODO make some kind Request class with escaping and stuff
if (Storage::WriteValue($_POST[Config::PARAM_NAME]))
{
    redirect(Config::URL_RESULT);
}
else
{
    echo 'Can\'t store value.' . "\n";
}