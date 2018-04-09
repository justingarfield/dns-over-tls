namespace JGarfield.DNSOverTLS.Shared.DomainProtocol
{

    /// <summary>
    /// Represents a Domain Protocol Resource Record. This follows the
    /// RFC located at https://www.ietf.org/rfc/rfc1035.txt and does
    /// not attempt to "beautify" property names and matches cAsE to
    /// the RFC itself.
    /// </summary>
    public class ResourceRecord
    {

        /// <summary>
        /// a domain name to which this resource record pertains.
        /// </summary>
        public string NAME { get; set; }

        /// <summary>
        /// two octets containing one of the RR type codes.  This
        /// field specifies the meaning of the data in the RDATA
        /// field.
        /// </summary>
        public short TYPE { get; set; }

        /// <summary>
        /// two octets which specify the class of the data in the
        /// RDATA field.
        /// </summary>
        public short CLASS { get; set; }

        /// <summary>
        /// a 32 bit unsigned integer that specifies the time
        /// interval(in seconds) that the resource record may be
        /// cached before it should be discarded.Zero values are
        /// interpreted to mean that the RR can only be used for the
        /// transaction in progress, and should not be cached.
        /// </summary>
        public int TTL { get; set; }

        /// <summary>
        /// an unsigned 16 bit integer that specifies the length in
        /// octets of the RDATA field.
        /// </summary>
        public ushort RDLENGTH { get; set; }

        /// <summary>
        /// a variable length string of octets that describes the
        /// resource.The format of this information varies
        /// according to the TYPE and CLASS of the resource record.
        /// For example, the if the TYPE is A and the CLASS is IN,
        /// the RDATA field is a 4 octet ARPA Internet address.
        /// </summary>
        public byte[] RDATA { get; set; }

    }

}
