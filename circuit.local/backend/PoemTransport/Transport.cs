using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MassTransit;

namespace PoemUtils
{
    public interface IMessage
    {
        string corrId { get; set; }
    }

    public class Message : IMessage
    {
        public string corrId { get; set; }
        public string Line { get; set; }
        public double VowelsCount { get; set; }
        public double ConsonantsCount { get; set; }
        public int linesCount { get; set; }
    }

    public class ConsonantsMessage : Message { }
    public class RateCheckerMessage : Message { }

    public class PoemFilteringStarted : IMessage
    {
        public string corrId { get; set; }
        public string poem { get; set; }
    }

    public class PoemFilteringCompleted : IMessage
    {
        public string corrId { get; set; }
        public string poemGoodLines { get; set; }
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
        public IBusControl GetBus()
        {
            return busControl;
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
