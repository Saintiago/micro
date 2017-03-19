using Microsoft.Owin.Hosting;
using System;
using System.Configuration;


namespace Circuit
{
    public class Program
    {
        static void Main()
        {
            WebApp.Start<Startup>(url: ConfigurationManager.AppSettings["base-address"]);
            Console.WriteLine("Vowels counter process started");
            Console.ReadLine();
        }
    }
}
