using DNS.Client;
using DNS.Client.RequestResolver;
using DNS.Protocol;
using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace JGarfield.DNSOverTLS.Shared
{

    public class TlsRequestResolver : IRequestResolver
    {

        private IPEndPoint dns;

        public TlsRequestResolver(IPEndPoint dns)
        {
            this.dns = dns;
        }

        static String GetPublicKeyPinningHash(X509Certificate2 x509Cert)
        {
            //Public Domain: No attribution required
            //Get the SubjectPublicKeyInfo member of the certificate
            Byte[] subjectPublicKeyInfo = GetSubjectPublicKeyInfoRaw(x509Cert);

            //Take the SHA2-256 hash of the DER ASN.1 encoded value
            Byte[] digest;
            using (var sha2 = new SHA256Managed())
            {
                digest = sha2.ComputeHash(subjectPublicKeyInfo);
            }

            //Convert hash to base64
            String hash = Convert.ToBase64String(digest);

            return hash;
        }

        static Byte[] GetSubjectPublicKeyInfoRaw(X509Certificate2 x509Cert)
        {
            //Public Domain: No attribution required
            Byte[] rawCert = x509Cert.GetRawCertData();

            /*
             Certificate is, by definition:

                Certificate  ::=  SEQUENCE  {
                    tbsCertificate       TBSCertificate,
                    signatureAlgorithm   AlgorithmIdentifier,
                    signatureValue       BIT STRING  
                }

               TBSCertificate  ::=  SEQUENCE  {
                    version         [0]  EXPLICIT Version DEFAULT v1,
                    serialNumber         CertificateSerialNumber,
                    signature            AlgorithmIdentifier,
                    issuer               Name,
                    validity             Validity,
                    subject              Name,
                    subjectPublicKeyInfo SubjectPublicKeyInfo,
                    issuerUniqueID  [1]  IMPLICIT UniqueIdentifier OPTIONAL, -- If present, version MUST be v2 or v3
                    subjectUniqueID [2]  IMPLICIT UniqueIdentifier OPTIONAL, -- If present, version MUST be v2 or v3
                    extensions      [3]  EXPLICIT Extensions       OPTIONAL  -- If present, version MUST be v3
                }

            So we walk to ASN.1 DER tree in order to drill down to the SubjectPublicKeyInfo item
            */
            Byte[] list = AsnNext(ref rawCert, true); //unwrap certificate sequence
            Byte[] tbsCertificate = AsnNext(ref list, false); //get next item; which is tbsCertificate
            list = AsnNext(ref tbsCertificate, true); //unwap tbsCertificate sequence

            Byte[] version = AsnNext(ref list, false); //tbsCertificate.Version
            Byte[] serialNumber = AsnNext(ref list, false); //tbsCertificate.SerialNumber
            Byte[] signature = AsnNext(ref list, false); //tbsCertificate.Signature
            Byte[] issuer = AsnNext(ref list, false); //tbsCertificate.Issuer
            Byte[] validity = AsnNext(ref list, false); //tbsCertificate.Validity
            Byte[] subject = AsnNext(ref list, false); //tbsCertificate.Subject        
            Byte[] subjectPublicKeyInfo = AsnNext(ref list, false); //tbsCertificate.SubjectPublicKeyInfo        

            return subjectPublicKeyInfo;
        }

        static Byte[] AsnNext(ref Byte[] buffer, Boolean unwrap)
        {
            //Public Domain: No attribution required
            Byte[] result;

            if (buffer.Length < 2)
            {
                result = buffer;
                buffer = new Byte[0];
                return result;
            }

            int index = 0;
            Byte entityType = buffer[index];
            index += 1;

            int length = buffer[index];
            index += 1;

            int lengthBytes = 1;
            if (length >= 0x80)
            {
                lengthBytes = length & 0x0F; //low nibble is number of length bytes to follow
                length = 0;

                for (int i = 0; i < lengthBytes; i++)
                {
                    length = (length << 8) + (int)buffer[2 + i];
                    index += 1;
                }
                lengthBytes++;
            }

            int copyStart;
            int copyLength;
            if (unwrap)
            {
                copyStart = 1 + lengthBytes;
                copyLength = length;
            }
            else
            {
                copyStart = 0;
                copyLength = 1 + lengthBytes + length;
            }
            result = new Byte[copyLength];
            Array.Copy(buffer, copyStart, result, 0, copyLength);

            Byte[] remaining = new Byte[buffer.Length - (copyStart + copyLength)];
            if (remaining.Length > 0)
                Array.Copy(buffer, copyStart + copyLength, remaining, 0, remaining.Length);
            buffer = remaining;

            return result;
        }

        private bool UserCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            Console.WriteLine("Pinning Hash: {0}", GetPublicKeyPinningHash((X509Certificate2)certificate));
            if (sslPolicyErrors != SslPolicyErrors.None)
            {
                Console.WriteLine("SSL Certificate Validation Error!");
                Console.WriteLine(sslPolicyErrors.ToString());
                return false;
            }
            else if (GetPublicKeyPinningHash((X509Certificate2)certificate) != "yioEpqeR4WtDwE9YxNVnCEkTxIjx6EEIwFSQW+lJsbc=")
                return false;
            else
                return true;
        }

        private X509Certificate UserCertificateSelectionCallback(object sender, string targetHost, X509CertificateCollection localCertificates, X509Certificate localCertificate, string[] acceptableIssuers)
        {
            return null;
        }

        public async Task<IResponse> Resolve(IRequest request)
        {

            using (TcpClient tcp = new TcpClient())
            {
                
                await tcp.ConnectAsync(dns.Address, dns.Port);

                using (SslStream sslStream = new SslStream(tcp.GetStream(), true, new RemoteCertificateValidationCallback(UserCertificateValidationCallback), new LocalCertificateSelectionCallback(UserCertificateSelectionCallback), EncryptionPolicy.RequireEncryption))
                {
                    await sslStream.AuthenticateAsClientAsync("1.1.1.1");

                    byte[] buffer = request.ToArray();
                    byte[] length = BitConverter.GetBytes((ushort)buffer.Length);

                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(length);
                    }

                    await sslStream.WriteAsync(length, 0, length.Length);
                    await sslStream.WriteAsync(buffer, 0, buffer.Length);

                    buffer = new byte[2];
                    await Read(sslStream, buffer);
                    
                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(buffer);
                    }

                    buffer = new byte[BitConverter.ToUInt16(buffer, 0)];
                    await Read(sslStream, buffer);

                    IResponse response = Response.FromArray(buffer);

                    return ClientResponse.FromArray(request, buffer);
                }

            }
        }

        private static async Task Read(Stream stream, byte[] buffer)
        {
            int length = buffer.Length;
            int offset = 0;
            int size = 0;

            while (length > 0 && (size = await stream.ReadAsync(buffer, offset, length)) > 0)
            {
                offset += size;
                length -= size;
            }

            if (length > 0)
            {
                throw new IOException("Unexpected end of stream");
            }
        }

    }

}
