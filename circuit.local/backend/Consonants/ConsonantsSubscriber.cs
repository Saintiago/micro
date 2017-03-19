using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using GreenPipes;

namespace Circuit
{
    public class ConsonantCounterConsumer : IConsumer<Message>
    {
        public async Task Consume(ConsumeContext<Message> context)
        {
            if (context.Message.Target != MessageTarget.Consonants)
            {
                return;
            }

            Message msg = new Message();
            msg.corrId = context.Message.corrId;
            msg.Line = context.Message.Line;
            msg.VowelsCount = context.Message.VowelsCount;
            CharCounter ConsonantCounter = new CharCounter(Config.CONSONANTS);
            msg.ConsonantsCount = ConsonantCounter.Count(context.Message.Line);
            msg.Target = MessageTarget.RateChecker;

            Console.WriteLine(msg.corrId);
            Console.WriteLine(msg.Line);
            Console.WriteLine(msg.VowelsCount);
            Console.WriteLine(msg.ConsonantsCount);

            Publisher publisher = new Publisher();
            publisher.Publish(msg);
        }
    }

    public class ConsonantsSubscriber : Transport
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

                cfg.ReceiveEndpoint(host, "consonants_count_queue", e =>
                {
                    e.UseCircuitBreaker(cb =>
                    {
                        cb.TrackingPeriod = TimeSpan.FromMinutes(1);
                        cb.TripThreshold = 15;
                        cb.ActiveThreshold = 10;
                        cb.ResetInterval = TimeSpan.FromMinutes(5);
                    });

                    e.Consumer<ConsonantCounterConsumer>();
                });
            });
        }
    }
   
}
