using System;
using System.Threading.Tasks;
using MassTransit;
using GreenPipes;
using StackExchange.Redis;
using System.Runtime.Caching;
using PoemUtils;

namespace Circuit
{
    public class GoodPoemConsumer : IConsumer<PoemFilteringCompleted>
    {
        private ConnectionMultiplexer _redis;
        ObjectCache _cache;

        public GoodPoemConsumer()
        {
            _redis = ConnectionMultiplexer.Connect("localhost");
            _cache = MemoryCache.Default;
        }

        public async Task Consume(ConsumeContext<PoemFilteringCompleted> context)
        {
            string corrId = context.Message.corrId;
            string poemGoodLines = context.Message.poemGoodLines;
            int tenant = context.Message.tenant;

            Console.WriteLine(corrId);
            Console.WriteLine(poemGoodLines);
            Console.WriteLine(tenant);

            _cache.Set(new CacheItem(corrId, poemGoodLines), new CacheItemPolicy());

            IDatabase db = _redis.GetDatabase();
            db.StringSet(corrId, poemGoodLines);
        }
    }

    public class GoodPoemSubscriber : Transport
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

                cfg.ReceiveEndpoint(host, "good_poem_queue", e =>
                {
                    e.UseCircuitBreaker(cb =>
                    {
                        cb.TrackingPeriod = TimeSpan.FromMinutes(1);
                        cb.TripThreshold = 15;
                        cb.ActiveThreshold = 10;
                        cb.ResetInterval = TimeSpan.FromMinutes(5);
                    });

                    e.Consumer<GoodPoemConsumer>();
                });
            });
        }
    }

}