using System.Collections.Generic;
using System.Web.Http;
using StackExchange.Redis;
using System;

namespace BackEnd
{
    public class ValuesController : ApiController
    {
        ConnectionMultiplexer _redis;

        public ValuesController()
        {
            _redis = ConnectionMultiplexer.Connect("localhost");
        }

        // GET api/values 
        public string Get()
        {
            IDatabase db = _redis.GetDatabase();
            string value = db.StringGet("value");
            Console.WriteLine("Get: " + value);
            return value;
        }

        // POST api/values 
        public void Post([FromBody]string value)
        {
            Console.WriteLine("Post: " + value);
            IDatabase db = _redis.GetDatabase();
            db.StringSet("value", value);
        }
    }
}