<?

class Config
{
    const POEM_PARAM_NAME      = 'poem';
    const TENANT_PARAM_NAME    = 'tenant';
    const UID_PARAM_NAME       = 'uid';
    const API_SET_ADDR         = 'http://localhost:8091/api/values';
    const API_GET_ADDR         = 'http://localhost:8092/api/goodpoem';
    const API_GET_STAT_ADDR    = 'http://localhost:8093/api/poemstatistics';
    const LINE_INDEX_DELIMITER = '%%';
    const LINE_SEPARATOR       = '\r\n';

    const HOST            = 'http://circuit.local';
    const URL_INDEX       = self::HOST . '/index.php';
    const URL_RESULT      = self::HOST . '/result.php';
    const URL_FORM_ACTION = self::HOST . '/count_vowels_action.php';
}
