using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace dns_over_tls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        // Every protocol typically has a standard port number. For example, HTTP is typically 80, FTP is 20 and 21, etc.
        // For this example, we'll choose an arbitrary port number.
        static string PortNumber = "1337";
        static string DNS_LISTENING_PORT = "53";
        static string CLOUDFLARE_DNS_HOSTNAME = "cloudflare-dns.com";
        static string CLOUDFLARE_DNS_OVER_TLS_PORT = "853";

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.StartDNSToDNSOverTLSProxy();
        }

        /// <summary>
        /// Starts a Proxy that takes local DNS requests and transforms them into
        /// DNS-over-TLS versions instead, using the provided DNS-over-TLS endpoint.
        /// </summary>
        private async void StartDNSToDNSOverTLSProxy()
        {

            try
            {
                var serverDatagramSocket = new DatagramSocket();
                
                // The ConnectionReceived event is raised when connections are received.
                serverDatagramSocket.MessageReceived += ServerDatagramSocket_MessageReceived;
                
                this.serverListBox.Items.Add("server is about to bind...");
                var hostName = new Windows.Networking.HostName("192.168.175.13");
                // Start listening for incoming TCP connections on the specified port. You can specify any port that's not currently in use.
                //await serverDatagramSocket.BindEndpointAsync(hostName, DNS_LISTENING_PORT);
                await serverDatagramSocket.BindServiceNameAsync(DNS_LISTENING_PORT);

                this.serverListBox.Items.Add(string.Format("server is bound to port number {0}", DNS_LISTENING_PORT));
                
            }
            catch (Exception ex)
            {
                Windows.Networking.Sockets.SocketErrorStatus webErrorStatus = Windows.Networking.Sockets.SocketError.GetStatus(ex.GetBaseException().HResult);
                this.serverListBox.Items.Add(webErrorStatus.ToString() != "Unknown" ? webErrorStatus.ToString() : ex.Message);
            }

        }

        private async void ServerDatagramSocket_MessageReceived(Windows.Networking.Sockets.DatagramSocket sender, Windows.Networking.Sockets.DatagramSocketMessageReceivedEventArgs args)
        {
            string request;
            using (DataReader dataReader = args.GetDataReader())
            {
                request = dataReader.ReadString(dataReader.UnconsumedBufferLength).Trim();
            }

            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => this.serverListBox.Items.Add(string.Format("server received the request: \"{0}\"", request)));
            
        }

        private async void StreamSocketListener_ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            string request;
            using (var streamReader = new StreamReader(args.Socket.InputStream.AsStreamForRead()))
            {
                request = await streamReader.ReadLineAsync();
            }

            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => this.serverListBox.Items.Add(string.Format("server received the request: \"{0}\"", request)));

            // Echo the request back as the response.
            using (Stream outputStream = args.Socket.OutputStream.AsStreamForWrite())
            {
                using (var streamWriter = new StreamWriter(outputStream))
                {
                    await streamWriter.WriteLineAsync(request);
                    await streamWriter.FlushAsync();
                }
            }

            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => this.serverListBox.Items.Add(string.Format("server sent back the response: \"{0}\"", request)));

            sender.Dispose();

            await this.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => this.serverListBox.Items.Add("server closed its socket"));
        }

        private async void DoSomeShit()
        {
            
            try
            {
                // Create the StreamSocket and establish a connection to the echo server.
                using (var streamSocket = new StreamSocket())
                {
                    
                    // The server hostname that we will be establishing a connection to. In this example, the server and client are in the same process.
                    var hostName = new Windows.Networking.HostName("1.1.1.1");
                    
                    this.clientListBox.Items.Add("client is trying to connect...");

                    await streamSocket.ConnectAsync(hostName, "853", SocketProtectionLevel.Tls12);
                    
                    IBuffer certificateBuffer = streamSocket.Information.ServerCertificate.GetCertificateBlob();
                    Stream certificateStream = certificateBuffer.AsStream();

                    //Take the SHA2-256 hash of the DER ASN.1 encoded value
                    Byte[] digest;
                    using (var sha2 = new SHA256Managed())
                    {
                        digest = sha2.ComputeHash(certificateStream);
                    }

                    //Convert hash to base64
                    String hash = Convert.ToBase64String(digest);

                    this.clientListBox.Items.Add("client connected");
                    
                }

                this.clientListBox.Items.Add("client closed its socket");
            }
            catch (Exception ex)
            {
                Windows.Networking.Sockets.SocketErrorStatus webErrorStatus = Windows.Networking.Sockets.SocketError.GetStatus(ex.GetBaseException().HResult);
                this.clientListBox.Items.Add(webErrorStatus.ToString() != "Unknown" ? webErrorStatus.ToString() : ex.Message);
            }

        }
        
    }



}
