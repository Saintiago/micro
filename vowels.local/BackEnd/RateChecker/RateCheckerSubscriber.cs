using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using StackExchange.Redis;

namespace Vowels
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
            int vowelsCount = context.Message.VowelsCount;
            int consonantsCount = context.Message.ConsonantsCount;

            Console.WriteLine(corrId);
            Console.WriteLine(line);
            Console.WriteLine(vowelsCount);
            Console.WriteLine(consonantsCount);

            IDatabase db = _redis.GetDatabase();

            if (!db.KeyExists(corrId))
            {
                db.StringSet(corrId, "");
            }

            if ((vowelsCount / consonantsCount) == Config.VOWELS_CONSONANTS_GOOD_RATE)
            {
                db.StringAppend(corrId, line + Environment.NewLine);
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
                    e.Consumer<RateCheckerConsumer>();
                });
            });
        }
    }
   
}
