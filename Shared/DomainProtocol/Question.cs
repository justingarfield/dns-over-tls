namespace JGarfield.DNSOverTLS.Shared.DomainProtocol
{

    /// <summary>
    /// Represents a Domain Protocol Message Question. This follows the
    /// RFC located at https://www.ietf.org/rfc/rfc1035.txt and does
    /// not attempt to "beautify" property names and matches cAsE to
    /// the RFC itself.
    /// </summary>
    public class Question
    {

        /// <summary>
        /// A domain name represented as a sequence of labels, where
        /// each label consists of a length octet followed by that
        /// number of octets.The domain name terminates with the
        /// zero length octet for the null label of the root.
        /// 
        /// Note that this field may be an odd number of octets; 
        /// no padding is used.
        /// </summary>
        public string QNAME { get; set; }

        /// <summary>
        /// A two octet code which specifies the type of the query.
        /// The values for this field include all codes valid for a
        /// TYPE field, together with some more general codes which
        /// can match more than one type of RR.
        /// </summary>
        public short QTYPE { get; set; }

        /// <summary>
        /// A two octet code that specifies the class of the query.
        /// For example, the QCLASS field is IN for the Internet.
        /// </summary>
        public short QCLASS { get; set; }

    }

}
