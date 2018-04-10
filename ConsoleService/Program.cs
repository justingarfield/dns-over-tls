using JGarfield.DnsOverTls.Shared;
using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;

namespace JGarfield.DnsOverTls.ConsoleService
{

    /// <summary>
    /// Simple Console application that wraps the DnsResolver 
    /// in the Shared library. Used for Debugging when you don't
    /// feel like going through a full install for the Windows 
    /// Service project and only need to debug Shared.
    /// </summary>
    internal class Program
    {

        /// <summary>
        /// Logger used for Debug, Warnings, Info, etc.
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        /// <summary>
        /// Main entry point.
        /// </summary>
        static void Main()
        {

            // Configure log4net
            XmlDocument log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead("log4net.config"));
            var repo = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(Hierarchy));
            XmlConfigurator.Configure(repo, log4netConfig["log4net"]);

            // Fire up DNS Resolver
            CreateAndStartDnsResolver().Wait();

        }

        /// <summary>
        /// Creates a new DnsResolver and tells it to start 
        /// listening for traffic.
        /// </summary>
        /// <returns></returns>
        public async static Task CreateAndStartDnsResolver()
        {
            DnsResolver dnsResolver = new DnsResolver();
            await dnsResolver.Listen();
        }

    }

}
