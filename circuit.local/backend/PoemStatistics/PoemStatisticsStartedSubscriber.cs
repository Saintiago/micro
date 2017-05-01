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
    public class PoemStatisticsStartedConsumer : IConsumer<PoemFilteringStarted>
    {
        ObjectCache _cache;
        public static Object statLock = new Object();

        public PoemStatisticsStartedConsumer()
        {
            _cache = MemoryCache.Default;
        }

        public async Task Consume(ConsumeContext<PoemFilteringStarted> context)
        {
            string corrId = context.Message.corrId;
            string poem = context.Message.poem;

            Console.WriteLine(corrId);
            Console.WriteLine(poem);

            lock (statLock)
            {
                CacheItem statCache = _cache.GetCacheItem(corrId);
                if (statCache == null)
                {
                    _cache.Set(new CacheItem(corrId, new Statistics()), new CacheItemPolicy());

                    CacheItem corrIdListCache = _cache.GetCacheItem(Config.CORR_ID_LIST_CACHE_KEY);
                    List<string> corrIdList = (List<string>)corrIdListCache.Value;
                    corrIdList.Add(corrId);
                    corrIdListCache = new CacheItem(Config.CORR_ID_LIST_CACHE_KEY, corrIdList);
                    _cache.Set(corrIdListCache, new CacheItemPolicy());

                }

                Statistics statistics = (statCache != null) ? (Statistics)statCache.Value : new Statistics();
                statistics.totalLinesCount = Utils.GetLinesCount(poem);
                statCache = new CacheItem(corrId, statistics);
                _cache.Set(statCache, new CacheItemPolicy());
            }
        }
    }

    public class PoemStatisticsStartedSubscriber : Transport
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

                cfg.ReceiveEndpoint(host, "statistics_started_queue", e =>
                {
                    e.UseCircuitBreaker(cb =>
                    {
                        cb.TrackingPeriod = TimeSpan.FromMinutes(1);
                        cb.TripThreshold = 15;
                        cb.ActiveThreshold = 10;
                        cb.ResetInterval = TimeSpan.FromMinutes(5);
                    });

                    e.Consumer<PoemStatisticsStartedConsumer>();
                });
            });
        }
    }

}