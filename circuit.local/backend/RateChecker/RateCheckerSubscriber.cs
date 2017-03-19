using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using GreenPipes;
using StackExchange.Redis;

namespace Circuit
{
    public class RateCheckerConsumer : IConsumer<Message>
    {
        private ConnectionMultiplexer _redis;

        public RateCheckerConsumer()
        {
            _redis = ConnectionMultiplexer.Connect("localhost");
        }


        public async Task Consume(ConsumeContext<Message> context)
        {
            if (context.Message.Target != MessageTarget.RateChecker)
            {
                return;
            }

            string corrId = context.Message.corrId;
            string line = context.Message.Line;
            double vowelsCount = context.Message.VowelsCount;
            double consonantsCount = context.Message.ConsonantsCount;

            Console.WriteLine(corrId);
            Console.WriteLine(line);
            Console.WriteLine(vowelsCount);
            Console.WriteLine(consonantsCount);

            IDatabase db = _redis.GetDatabase();

            if (!db.KeyExists(corrId))
            {
                db.StringSet(corrId, "");
            }

            if (consonantsCount > 0)
            {
                double rate = vowelsCount / consonantsCount;
                double difference = Math.Abs(Config.VOWELS_CONSONANTS_GOOD_RATE * .001);
                if (Math.Abs(rate - Config.VOWELS_CONSONANTS_GOOD_RATE) <= difference)
                {
                    db.StringAppend(corrId, line + Environment.NewLine);
                }
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
