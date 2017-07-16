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
 * @param $poem
 * @return string
 */
function validate_string($poem)
{
    if (!(is_string($poem) && (strlen($poem) > 0)))
    {
        throw new InvalidArgumentException('Poem must not be empty string.');
    }
    return $poem;
}

/**
 * @param $tenant
 * @return int
 */
function validate_integer($tenant)
{
    $tenant = (int)$tenant;
    if ($tenant <= 0)
    {
        throw new InvalidArgumentException('Tenant must be integer > 0.');
    }
    return $tenant;
}

/**
 * @param string $poem
 * @return string
 */
function preparePoem($poem)
{
    $lines = explode(Config::LINE_SEPARATOR, $poem);
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
