using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;

namespace Vowels
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
        public int VowelsCount { get; set; }
        public int ConsonantsCount { get; set; }
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
        public void Publish(Message message)
        {
            busControl.Publish<Message>(message);
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
