using JGarfield.DNSOverTLS.Shared;
using System;
using System.Threading.Tasks;

namespace ConsoleService
{
    class Program
    {
        static void Main(string[] args)
        {

            DNSResolver dnsResolver = new DNSResolver();
            Task.FromResult(
                dnsResolver.Listen()
            ).GetAwaiter();

            Console.ReadLine();

        }
    }
}
