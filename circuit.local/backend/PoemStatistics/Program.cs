using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Configuration;
using System.Runtime.Caching;
using PoemUtils;


namespace Circuit
{
    public class Statistics
    {
        public int totalLinesCount { get; set; } = -1;
        public int goodLinesCount { get; set; } = -1;
    }

    public class Program
    {
        static void Main()
        {
            ObjectCache cache = MemoryCache.Default;
            cache.Set(new CacheItem(Config.CORR_ID_LIST_CACHE_KEY, new List<string>()), new CacheItemPolicy());

            Console.WriteLine("Poem statistics process started");

            Task.Factory.StartNew(() => StartPoemStatisticsStartedSubscriber());
            Task.Factory.StartNew(() => StartPoemStatisticsCompletedSubscriber());
            Task.Factory.StartNew(() => StartWebInterface());

            Console.ReadLine();
        }

        static void StartPoemStatisticsStartedSubscriber()
        {
            PoemStatisticsStartedSubscriber poemStatisticsStartedSubscriber = new PoemStatisticsStartedSubscriber();
            Console.WriteLine("Poem statistics started subscriber task started");
            Console.ReadLine();
        }

        static void StartPoemStatisticsCompletedSubscriber()
        {
            PoemStatisticsCompletedSubscriber poemStatisticsCompletedSubscriber = new PoemStatisticsCompletedSubscriber();
            Console.WriteLine("Poem statistics completed subscriber task started");
            Console.ReadLine();
        }

        static void StartWebInterface()
        {
            WebApp.Start<PoemStatisticsControllerStartup>(url: ConfigurationManager.AppSettings["base-address"]);
            Console.WriteLine("Poem statistics web controller started");
            Console.ReadLine();
        }
    }
}