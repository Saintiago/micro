<?

require_once("../include/common.inc.php");

class Storage
{
    /**
     * @return array
     */
    public static function ReadStatistics()
    {
        return json_decode(self::MakeGetRequest(Config::API_GET_STAT_ADDR), true);
    }

    /**
     * @param string $key
     * @return string
     */
    public static function ReadValue($key)
    {
        return self::MakeGetRequest(Config::API_GET_ADDR . '/' . $key);
    }
    
    /**
     * @param string $value 
     * @return bool
     */    
    public static function WriteValue($value)
    {
        $ch = curl_init();
        
        curl_setopt($ch, CURLOPT_URL, Config::API_SET_ADDR);

        curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
        curl_setopt($ch, CURLOPT_POST, 1);
        curl_setopt($ch, CURLOPT_POSTFIELDS, self::WrapStringForPost($value));
        curl_setopt($ch, CURLOPT_HTTPHEADER, ['Content-Type: application/json']);

        $server_output = curl_exec ($ch);

        curl_close ($ch);
        
        return $server_output;
    }

    /**
     * @param string $url
     * @return string
     */
    private static function MakeGetRequest($url)
    {
        $ch = curl_init();

        curl_setopt($ch,CURLOPT_URL, $url);
        curl_setopt($ch,CURLOPT_RETURNTRANSFER, true);
        curl_setopt($ch, CURLOPT_TIMEOUT, 60);

        $output = curl_exec($ch);

        curl_close($ch);
        return $output;
    }
    
    private static function WrapStringForPost($value)
    {
        return '\'' . urlencode($value) . '\'';
    }
    
}
