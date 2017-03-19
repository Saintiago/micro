using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using System.Diagnostics;

namespace Circuit
{
    public enum MessageTarget
    {
        Consonants,
        RateChecker
    }

    public class Message
    {
        public string corrId { get; set; }
        public string Line { get; set; }
        public double VowelsCount { get; set; }
        public double ConsonantsCount { get; set; }
        public MessageTarget Target { get; set; }

    }

    public abstract class Transport
    {

        protected IBusControl busControl;

        public Transport()
        {
            busControl = ConfigureBus();
            busControl.Start();
        }

        ~Transport()
        {
            busControl.Stop();
        }

        protected abstract IBusControl ConfigureBus();

    }

    public class Publisher : Transport
    {
        private Random rnd = new Random();
        /**
         * Retry pattern
        */
        public void Publish(Message message)
        {
            int currentRetry = 0;

            for (;;)
            {
                try
                {
                    // Imitating failure with 0.25 chance
                    if (rnd.Next() < (int.MaxValue / 4))
                    {
                        throw new Exception("Retry!");
                    }

                    busControl.Publish<Message>(message);
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("We've got an exception! Retrying...");

                    currentRetry++;

                    if (currentRetry > Config.RETRY_COUNT)
                    {
                        throw;
                    }
                }

                Task.Delay(Config.DELAY);
            }
        }

        protected override IBusControl ConfigureBus()
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://localhost/"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
            });
        }
    }
}
