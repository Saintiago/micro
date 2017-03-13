using System.Collections.Generic;
using System.Web.Http;
using StackExchange.Redis;
using System;

namespace Vowels
{
    public class ValuesController : ApiController
    {
        private ConnectionMultiplexer _redis;
        private Publisher _transport;

        public ValuesController()
        {
            _redis = ConnectionMultiplexer.Connect("localhost");
            _transport = new Publisher();
        }

        // GET api/values 
        public string Get()
        {
            IDatabase db = _redis.GetDatabase();
            string value = db.StringGet("poem");
            Console.WriteLine("Get: " + value);
            return value;
        }

        // POST api/values 
        public string Post([FromBody]string poem)
        {
            string[] lines = poem.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            string poemId = Guid.NewGuid().ToString();
            CharCounter VowelCounter = new CharCounter(Config.VOWELS);
            int lineIndex = 1;

            foreach (string line in lines)
            {
                Message msg = new Message();
                msg.corrId = poemId;
                msg.Line = lineIndex + Config.LINE_INDEX_DELIMITER + line;
                msg.VowelsCount = VowelCounter.Count(line);
                msg.Target = MessageTarget.Consonants;

                Console.WriteLine(msg.corrId);
                Console.WriteLine(msg.Line);
                Console.WriteLine(msg.VowelsCount);
                Console.WriteLine(msg.ConsonantsCount);
                Console.WriteLine(msg.Target);

                _transport.Publish(msg);
                ++lineIndex;
            }

            return poemId;
        }
    }
}