namespace JGarfield.DNSOverTLS.Shared.DomainProtocol
{

    /// <summary>
    /// Represents a Domain Protocol Message Header. This follows the
    /// RFC located at https://www.ietf.org/rfc/rfc1035.txt and does
    /// not attempt to "beautify" property names and matches cAsE to
    /// the RFC itself.
    /// </summary>
    public class Header
    {

        /// <summary>
        /// A 16 bit identifier assigned by the program that
        /// generates any kind of query.This identifier is copied
        /// the corresponding reply and can be used by the requester
        /// to match up replies to outstanding queries.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool QR { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int OPCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool AA { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool TC { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool RD { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool RA { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Z { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool RCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool QDCOUNT { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool ANCOUNT { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool NSCOUNT { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool ARCOUNT { get; set; }

    }

}
