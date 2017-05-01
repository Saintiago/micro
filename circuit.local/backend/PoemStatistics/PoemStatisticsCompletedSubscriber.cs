using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using GreenPipes;
using System.Runtime.Caching;
using PoemUtils;

namespace Circuit
{
    public class PoemStatisticsCompletedConsumer : IConsumer<PoemFilteringCompleted>
    {
        ObjectCache _cache;

        public PoemStatisticsCompletedConsumer()
        {
            _cache = MemoryCache.Default;
        }

        public async Task Consume(ConsumeContext<PoemFilteringCompleted> context)
        {
            string corrId = context.Message.corrId;
            string poemGoodLines = context.Message.poemGoodLines;

            Console.WriteLine(corrId);
            Console.WriteLine(poemGoodLines);

            lock (PoemStatisticsStartedConsumer.statLock)
            {
                CacheItem statisticsCache = _cache.GetCacheItem(corrId);
                if (statisticsCache == null)
                {
                    _cache.Set(new CacheItem(corrId, new Statistics()), new CacheItemPolicy());

                    CacheItem corrIdListCache = _cache.GetCacheItem(Config.CORR_ID_LIST_CACHE_KEY);
                    List<string> corrIdList = (List<string>)corrIdListCache.Value;
                    corrIdList.Add(corrId);
                    corrIdListCache = new CacheItem(Config.CORR_ID_LIST_CACHE_KEY, corrIdList);
                    _cache.Set(corrIdListCache, new CacheItemPolicy());
                }

                Statistics statistics = (statisticsCache != null) ? (Statistics)statisticsCache.Value : new Statistics();
                statistics.goodLinesCount = Utils.GetLinesCount(poemGoodLines);
                statisticsCache = new CacheItem(corrId, statistics);
                _cache.Set(statisticsCache, new CacheItemPolicy());
            }
        }
    }

    public class PoemStatisticsCompletedSubscriber : Transport
    {
        protected override IBusControl ConfigureBus()
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://localhost/"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint(host, "statistics_completed_queue", e =>
                {
                    e.UseCircuitBreaker(cb =>
                    {
                        cb.TrackingPeriod = TimeSpan.FromMinutes(1);
                        cb.TripThreshold = 15;
                        cb.ActiveThreshold = 10;
                        cb.ResetInterval = TimeSpan.FromMinutes(5);
                    });

                    e.Consumer<PoemStatisticsCompletedConsumer>();
                });
            });
        }
    }

}