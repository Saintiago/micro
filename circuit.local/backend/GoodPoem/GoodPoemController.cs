using System.Collections.Generic;
using System.Web.Http;
using System;
using System.Runtime.Caching;
using PoemUtils;


namespace Circuit
{
    public class GoodPoemController : ApiController
    {
        ObjectCache _cache;

        public GoodPoemController()
        {
            _cache = MemoryCache.Default;
        }

        // GET api/goodpoem/?tenant={tenant}&poemKey={poemKey} 
        public string Get(int tenant = 0, string poemKey = null)
        {
            string goodPoem = "";
            CacheItem goodPoemCache = _cache.GetCacheItem(poemKey);
            if (goodPoemCache != null)
            {
                goodPoem = goodPoemCache.Value.ToString();
            }
            else
            {
                int triesCount = 0;
                while (triesCount < (Config.SERVICE_TIMEOUT / 1000))
                {
                    var value = Sharding.GetInstance().Read(tenant, poemKey);
                    if (!String.IsNullOrEmpty(value))
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