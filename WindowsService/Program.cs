using System.ServiceProcess;

namespace JGarfield.DNSOverTLS.WindowsService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new DNSOverTLSProxyService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
