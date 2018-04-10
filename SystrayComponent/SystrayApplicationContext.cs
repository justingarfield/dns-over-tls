using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Core;
using Windows.Foundation.Collections;

namespace JGarfield.DnsOverTls.SystrayComponent
{

    public class SystrayApplicationContext : ApplicationContext
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(SystrayApplicationContext));
        private AppServiceConnection connection = null;
        private NotifyIcon notifyIcon = null;
        private Form1 configWindow = new Form1();

        public SystrayApplicationContext()
        {
            MenuItem startMenuItem = new MenuItem("Start (Running)", new EventHandler(NoOp));
            MenuItem stopMenuItem = new MenuItem("Stop", new EventHandler(NoOp));
            MenuItem dashLineMenuItem = new MenuItem("============================", new EventHandler(NoOp));

            MenuItem openDashboardMenuItem = new MenuItem("Open Dashboard", new EventHandler(NoOp));
            MenuItem exitMenuItem = new MenuItem("Exit", new EventHandler(NoOp));
            dashLineMenuItem.Enabled = false;
            startMenuItem.DefaultItem = true;
            startMenuItem.Enabled = false;

            notifyIcon = new NotifyIcon();
            notifyIcon.DoubleClick += new EventHandler(OpenApp);
            notifyIcon.Icon = SystrayComponent.Properties.Resources.Icon1;
            notifyIcon.ContextMenu = new ContextMenu(new MenuItem[] { startMenuItem, stopMenuItem, dashLineMenuItem, openDashboardMenuItem, exitMenuItem });
            notifyIcon.Visible = true;
            
            notifyIcon.Text = $"DNS-over-TLS Proxy";

            QueryServiceController();

        }

        private void ConfigureTestMenu()
        {
            MenuItem testServiceNotFoundNotification = new MenuItem();
            
        }

        private void QueryServiceController()
        {

            try
            {

                ServiceController[] scServices = ServiceController.GetServices();

                ServiceController dnsOverTlsWindowsServiceRef = scServices.Where(sc => sc.ServiceName == "DNS-over-TLS").FirstOrDefault();
                if (dnsOverTlsWindowsServiceRef == null)
                {
                    NotifyUserOfWarning("DNS-over-TLS Proxy Windows Service could not be found. Please make sure it's installed before trying to use the System Tray Application.");

                } else
                {
                    ServiceController sc = new ServiceController("DNS-over-TLS");
                    if (sc.Status == ServiceControllerStatus.Stopped)
                    {
                        NotifyUserOfWarning("DNS-over-TLS Proxy Windows Service is currently Stopped.");
                    }
                }

            }
            catch (Win32Exception win32Ex)
            {
                Console.WriteLine(win32Ex.Message);
                Console.WriteLine(win32Ex.ErrorCode.ToString());
                Console.WriteLine(win32Ex.NativeErrorCode.ToString());
                Console.WriteLine(win32Ex.StackTrace);
                Console.WriteLine(win32Ex.Source);
                Exception e = win32Ex.GetBaseException();
                Console.WriteLine(e.Message);

                NotifyUserOfError("There was an error enumerating Windows Services to check for the presence of the DNS-over-TLS Proxy. Please review the Logs directory for more information.");

            }
            catch (Exception ex)
            {
                NotifyUserOfError("There was an error enumerating Windows Services to check for the presence of the DNS-over-TLS Proxy. Please review the Logs directory for more information.");
            }

        }

        private void NotifyUserOfWarning(string message)
        {
            notifyIcon.ShowBalloonTip(5000, "DNS-over-TLS Proxy", message, ToolTipIcon.Warning);
        }

        private void NotifyUserOfError(string message)
        {
            notifyIcon.ShowBalloonTip(5000, "DNS-over-TLS Proxy", message, ToolTipIcon.Error);
        }

        private async void NoOp(object sender, EventArgs e) { }

        public enum SimpleServiceCustomCommands {
            StopWorker = 128,
            RestartWorker,
            CheckWorker
        };

        private async void StartProxyService(object sender, EventArgs e)
        {
            ServiceController[] scServices;
            scServices = ServiceController.GetServices();

            foreach (ServiceController scTemp in scServices)
            {

                if (scTemp.ServiceName == "Simple Service")
                {

                    // Display properties for the Simple Service sample
                    // from the ServiceBase example.
                    ServiceController sc = new ServiceController("DnsOverTls");
                    Console.WriteLine("Status = " + sc.Status);
                    Console.WriteLine("Can Pause and Continue = " + sc.CanPauseAndContinue);
                    Console.WriteLine("Can ShutDown = " + sc.CanShutdown);
                    Console.WriteLine("Can Stop = " + sc.CanStop);
                    if (sc.Status == ServiceControllerStatus.Stopped)
                    {
                        sc.Start();
                        while (sc.Status == ServiceControllerStatus.Stopped)
                        {
                            Thread.Sleep(1000);
                            sc.Refresh();
                        }
                    }

                }

            }

        }

        private async void StopProxyService(object sender, EventArgs e)
        {
            IEnumerable<AppListEntry> appListEntries = await Package.Current.GetAppListEntriesAsync();
            await appListEntries.First().LaunchAsync();
        }

        private async void OpenApp(object sender, EventArgs e)
        {
            IEnumerable<AppListEntry> appListEntries = await Package.Current.GetAppListEntriesAsync();
            await appListEntries.First().LaunchAsync();
        }

        private async void SendToUWP(object sender, EventArgs e)
        {
            ValueSet message = new ValueSet();
            message.Add("content", "Message from Systray Extension");
            await SendToUWP(message);
        }

        private void OpenLegacy(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form.Show();
        }

        private async void Exit(object sender, EventArgs e)
        {
            ValueSet message = new ValueSet();
            message.Add("exit", "");
            await SendToUWP(message);
            Application.Exit();
        }

        private async Task SendToUWP(ValueSet message)
        {
            if (connection == null)
            {
                connection = new AppServiceConnection();
                connection.PackageFamilyName = Package.Current.Id.FamilyName;
                connection.AppServiceName = "SystrayExtensionService";
                connection.ServiceClosed += Connection_ServiceClosed;
                AppServiceConnectionStatus connectionStatus = await connection.OpenAsync();
                if (connectionStatus != AppServiceConnectionStatus.Success)
                {
                    MessageBox.Show("Status: " + connectionStatus.ToString());
                    return;
                }
            }

            await connection.SendMessageAsync(message);
        }

        private void Connection_ServiceClosed(AppServiceConnection sender, AppServiceClosedEventArgs args)
        {
            connection.ServiceClosed -= Connection_ServiceClosed;
            connection = null;
        }
    }
}