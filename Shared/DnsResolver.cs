using DNS.Client;
using DNS.Client.RequestResolver;
using DNS.Server;
using log4net;
using System;
using System.Net;
using System.Threading.Tasks;

namespace JGarfield.DnsOverTls.Shared
{

    /// <summary>
    /// 
    /// </summary>
    public class DnsResolver
    {

        /// <summary>
        /// Logger used for Debug, Warnings, Info, etc.
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(typeof(DnsResolver));

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task Listen()
        {
            MasterFile masterFile = new MasterFile();
            IRequestResolver tlsRequestResolver = new TlsRequestResolver(new System.Net.IPEndPoint(IPAddress.Parse("1.1.1.1"), 853));
            DnsServer server = new DnsServer(tlsRequestResolver);

            server.Responded += (request, response) => Console.WriteLine("{0} => {1}", request, response);
            server.Listening += () => Console.WriteLine("Listening");
            server.Errored += (e) => {
                Console.WriteLine("Errored: {0}", e);
                ResponseException responseError = e as ResponseException;
                if (responseError != null) Console.WriteLine(responseError.Response);
            };

            await server.Listen();
        }

    }

}
