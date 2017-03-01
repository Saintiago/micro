<?

require_once("../include/common.inc.php");

class Storage
{
    /**
     * @return string
     */
    public static function ReadValue()
    {
        $client = new Predis\Client();
        $value = $client->get(Config::PARAM_NAME);
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
        
        curl_setopt($ch, CURLOPT_PROXY, '127.0.0.1:8888');
        
        curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1 );
        curl_setopt($ch, CURLOPT_POST, 1 );
        curl_setopt($ch, CURLOPT_POSTFIELDS, self::WrapStringForPost($value));
        curl_setopt($ch, CURLOPT_HTTPHEADER, ['Content-Type: application/json']); 
        
        curl_exec($ch);
        
        return true;
    }
    
    private static function WrapStringForPost($value)
    {
        return '\'' . $value . '\'';
    }
    
}
