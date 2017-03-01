<?

function redirect($url)
{
    header("Location: $url");
    die();
}
