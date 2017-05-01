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
    public class RateCheckerConsumer : IConsumer<RateCheckerMessage>
    {
        ObjectCache _cache;

        public RateCheckerConsumer()
        {
            _cache = MemoryCache.Default;
        }


        public async Task Consume(ConsumeContext<RateCheckerMessage> context)
        {
            string corrId = context.Message.corrId;
            string line = context.Message.Line;
            double vowelsCount = context.Message.VowelsCount;
            double consonantsCount = context.Message.ConsonantsCount;
            int linesCount = context.Message.linesCount;

            Console.WriteLine(corrId);
            Console.WriteLine(line);
            Console.WriteLine(vowelsCount);
            Console.WriteLine(consonantsCount);
            Console.WriteLine(linesCount);

            string corrIdTmpKey = corrId + Config.GOOD_POEM_TEMP_CACHE_KEY_POSTFIX;
            if (consonantsCount > 0)
            {
                double rate = vowelsCount / consonantsCount;
                double difference = Math.Abs(Config.VOWELS_CONSONANTS_GOOD_RATE * .001);
                if (Math.Abs(rate - Config.VOWELS_CONSONANTS_GOOD_RATE) <= difference)
                {
                    CacheItem goodPoemCache = _cache.GetCacheItem(corrIdTmpKey);
                    string goodLines = (goodPoemCache != null) ? goodPoemCache.Value.ToString() : "";
                    goodLines += line + Environment.NewLine;
                    _cache.Set(new CacheItem(corrIdTmpKey, goodLines), new CacheItemPolicy());
                }
            }

            string corrIdLineCountKey = corrId + Config.LINE_COUNT_CACHE_KEY_POSTFIX;
            CacheItem linesCountCache = _cache.GetCacheItem(corrIdLineCountKey);
            int currentLinesCount = (linesCountCache != null) ? (int)linesCountCache.Value : 0;           
            linesCountCache = new CacheItem(corrIdLineCountKey, ++currentLinesCount);
            _cache.Set(linesCountCache, new CacheItemPolicy());

            if (currentLinesCount == linesCount)
            {
                PoemFilteringCompleted msg = new PoemFilteringCompleted();
                msg.corrId = corrId;
                msg.poemGoodLines = _cache.GetCacheItem(corrIdTmpKey).Value.ToString();
                Publisher publisher = new Publisher();
                publisher.GetBus().Publish<PoemFilteringCompleted>(msg);
            }

        }
    }

    public class RateCheckerSubscriber : Transport
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

                cfg.ReceiveEndpoint(host, "rate_check_queue", e =>
                {
                    e.UseCircuitBreaker(cb =>
                    {
                        cb.TrackingPeriod = TimeSpan.FromMinutes(1);
                        cb.TripThreshold = 15;
                        cb.ActiveThreshold = 10;
                        cb.ResetInterval = TimeSpan.FromMinutes(5);
                    });

                    e.Consumer<RateCheckerConsumer>();
                });
            });
        }
    }

}
