using Microsoft.Owin.Hosting;
using System;
using System.Net.Http;
using Vowels;

namespace Vowels
{
    public class Program
    {
        static void Main()
        {
            Console.WriteLine("Rate checker counter process started");
            RateCheckerSubscriber consonantTransport = new RateCheckerSubscriber();
            Console.ReadLine();
        }
    }
}
