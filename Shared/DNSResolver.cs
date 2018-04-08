using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace JGarfield.DNSOverTLS.Shared
{

    public class DNSResolver
    {

        private bool keepRunning = true;

        public async Task Listen()
        {

            try
            {

                using (UdpClient udpClient = new UdpClient(53))
                {

                    while (keepRunning)
                    {

                        // Blocks until a message returns on this socket from a remote host.
                        UdpReceiveResult udpReceiveResult = await udpClient.ReceiveAsync();
                        var bufferLength = udpReceiveResult.Buffer.Length;
                        

                    }

                }

            }
            catch (SocketException se)
            {
                Console.WriteLine(se.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

    }

}
