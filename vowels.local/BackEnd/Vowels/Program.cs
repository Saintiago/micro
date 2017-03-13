using Microsoft.Owin.Hosting;
using System;
using System.Net.Http;

namespace Vowels
{
    public class Program
    {
        static void Main()
        {
            WebApp.Start<Startup>(url: Config.BASE_ADDRESS);
            Console.WriteLine("Vowels counter process started");
            Console.ReadLine();
        }
    }
}
