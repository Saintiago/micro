using Microsoft.Owin.Hosting;
using System;
using System.Net.Http;

namespace BackEnd
{
    public class Program
    {
        static void Main()
        {
            string baseAddress = "http://localhost:8080/";

            // Start OWIN host 
            WebApp.Start<Startup>(url: baseAddress);

            Console.WriteLine("Backend process started");

            Console.ReadLine();
        }
    }
}
