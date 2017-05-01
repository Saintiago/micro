using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Configuration;

namespace Circuit
{
    public class Program
    {
        static void Main()
        {
            Console.WriteLine("Good poem process started");
            Task.Factory.StartNew(() => StartSubscriber());
            Task.Factory.StartNew(() => StartWebInterface());
            Console.ReadLine();
        }

        static void StartSubscriber()
        {
            GoodPoemSubscriber consonantTransport = new GoodPoemSubscriber();
            Console.WriteLine("Good poem subscriber task started");
            Console.ReadLine();
        }

        static void StartWebInterface()
        {
            WebApp.Start<GoodPoemControllerStartup>(url: ConfigurationManager.AppSettings["base-address"]);
            Console.WriteLine("Good poem web controller started");
            Console.ReadLine();
        }
    }
}