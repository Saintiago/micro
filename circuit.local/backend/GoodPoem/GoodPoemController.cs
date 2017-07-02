using System.Collections.Generic;
using System.Web.Http;
using StackExchange.Redis;
using System;
using System.Runtime.Caching;
using PoemUtils;


namespace Circuit
{
    public class GoodPoemController : ApiController
    {
        private ConnectionMultiplexer _redis;
        ObjectCache _cache;

        public GoodPoemController()
        {
            _redis = ConnectionMultiplexer.Connect("localhost");
            _cache = MemoryCache.Default;
        }
        // GET api/goodpoem/{poemKey} 
        public string Get(string poemKey)
        {
            string goodPoem = "";
            CacheItem goodPoemCache = _cache.GetCacheItem(poemKey);
            if (goodPoemCache != null)
            {
                goodPoem = goodPoemCache.Value.ToString();
            }
            else
            {
                IDatabase db = _redis.GetDatabase();
                int triesCount = 0;
                while (triesCount < (Config.SERVICE_TIMEOUT / 1000))
                {
                    var value = db.StringGet(poemKey);
                    if (!value.IsNull)
                    {
                        goodPoem = value;
                        goodPoemCache = new CacheItem(poemKey, value);
                        _cache.Set(goodPoemCache, new CacheItemPolicy());
                        break;
                    }

                    System.Threading.Thread.Sleep(1000);
                    triesCount++;
                }
            }

            Console.WriteLine("Get: " + goodPoem);
            return goodPoem;
        }
    }
}