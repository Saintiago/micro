<?

class Request
{
    private $params = [];

    /**
     * @param [] $rawRequest
     */
    public function __construct($rawRequest)
    {
        $this->params = $rawRequest;
    }

    /**
     * @param $key
     * @return mixed
     */
    public function getRequestParam($key)
    {
        if (!isset($this->params[$key]))
        {
            throw new InvalidArgumentException('There is no "' . $key . '" key in request.');
        }

        return $this->params[$key];
    }
}