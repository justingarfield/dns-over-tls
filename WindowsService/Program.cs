using log4net;
using log4net.Config;
using System.ServiceProcess;

namespace JGarfield.DnsOverTls.WindowsService
{

    /// <summary>
    /// 
    /// </summary>
    internal static class Program
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

            XmlConfigurator.Configure(new System.IO.FileInfo("log4net.config"));

            ServiceBase[] servicesToRun;
            servicesToRun = new ServiceBase[]
            {
                new DnsOverTlsProxyService()
            };
            ServiceBase.Run(servicesToRun);
            
        }

    }

}
