using System.Collections.Generic;
using System.Web.Http;
using System;
using System.Web.Script.Serialization;
using System.Runtime.Caching;
using PoemUtils;

namespace Circuit
{
    public class PoemStatisticsController : ApiController
    {
        ObjectCache _cache;

        public PoemStatisticsController()
        {
            _cache = MemoryCache.Default;
        }

        // GET api/poemstatistics
        public string Get()
        {
            var statisticsArray = new Dictionary<string, Dictionary<string, string>>();

            CacheItem corrIdListCache = _cache.GetCacheItem(Config.CORR_ID_LIST_CACHE_KEY);
            if (corrIdListCache != null)
            {
                List<string> corrIdList = (List<string>)corrIdListCache.Value;

                foreach (var corrId in corrIdList)
                {
                    CacheItem statisticsCache = _cache.GetCacheItem(corrId);

                    if (statisticsCache == null)
                    {
                        continue;
                    }

                    Statistics statistics = (Statistics)statisticsCache.Value;

                    statisticsArray.Add(corrId, 
                        new Dictionary<string, string>{
                            { "totalLinesCount",  statistics.totalLinesCount.ToString()},
                            { "goodLinesCount",  statistics.goodLinesCount.ToString()}
                        });
                }
            }

            return new JavaScriptSerializer().Serialize(statisticsArray);
        }
    }
}