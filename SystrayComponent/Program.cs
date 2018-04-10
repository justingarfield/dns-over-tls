using System;
using System.Threading;
using System.Windows.Forms;
using log4net;
using log4net.Config;

namespace JGarfield.DnsOverTls.SystrayComponent
{

    internal static class Program
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            // Set up a simple configuration that logs on the console.
            BasicConfigurator.Configure();

            log.Info("Entering application.");
            
            Mutex mutex = null;
            try
            {
                if (!Mutex.TryOpenExisting("DnsOverTlsSystemTrayMutex", out mutex))
                {
                    mutex = new Mutex(false, "DnsOverTlsSystemTrayMutex");
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new SystrayApplicationContext());
                    mutex.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            log.Info("Exiting application.");

        }

    }

}
