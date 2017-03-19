<?

require_once("../include/common.inc.php");

class Storage
{
    /**
     * @param string $key
     * @return string
     */
    public static function ReadValue($key)
    {
        $client = new Predis\Client();
        $value = $client->get($key);
        return $value;
    }
    
    /**
     * @param string $value 
     * @return bool
     */    
    public static function WriteValue($value)
    {
        $ch = curl_init();
        
        curl_setopt($ch, CURLOPT_URL, Config::API_SET_ADDR);
        
        curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1 );
        curl_setopt($ch, CURLOPT_POST, 1 );
        curl_setopt($ch, CURLOPT_POSTFIELDS, self::WrapStringForPost($value));
        curl_setopt($ch, CURLOPT_HTTPHEADER, ['Content-Type: application/json']);

        $server_output = curl_exec ($ch);

        curl_close ($ch);
        
        return $server_output;
    }
    
    private static function WrapStringForPost($value)
    {
        return '\'' . $value . '\'';
    }
    
}
