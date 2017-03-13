<?

/**
 * @param string $url
 * @param array  $params
 */
function redirect($url, $params = [])
{
    if (count($params) > 0)
    {
        $queryString = '?';
        foreach ($params as $key => $value)
        {
            $queryString .= "$key=$value&";
        }
        $url .= rtrim($queryString, '&');
    }

    header("Location: $url");
    die();
}

/**
 * @param $value
 * @return bool
 */
function validate_string($value)
{
    if (!(is_string($value) && (strlen($value) > 0)))
     {
         throw new InvalidArgumentException('Value must be not empty string.');
     }
}

/**
 * @param string $poem
 * @return string
 */
function preparePoem($poem)
{
    $lines = explode("\n", $poem);
    $indexedLines = [];
    foreach ($lines as &$line)
    {
        if (strpos($line, Config::LINE_INDEX_DELIMITER) === false)
        {
            continue;
        }

        $lineElements = explode(Config::LINE_INDEX_DELIMITER, $line);
        $indexedLines[(int)$lineElements[0]] = $lineElements[1];
    }

    ksort($indexedLines);

    return implode ("\n", $indexedLines);
}
