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
        // TODO: Determine if this should be signed or unsigned. Not fully clear in the RFC.
        public short ID { get; set; }

        /// <summary>
        /// A one bit field that specifies whether this message is a
        /// query(0), or a response(1).
        /// </summary>
        public MessageType QR { get; set; }

        /// <summary>
        /// A four bit field that specifies kind of query in this
        /// message.This value is set by the originator of a query
        /// and copied into the response.
        /// </summary>
        public QueryType OPCODE { get; set; }

        /// <summary>
        /// Authoritative Answer - this bit is valid in responses,
        /// and specifies that the responding name server is an
        /// authority for the domain name in question section.
        /// </summary>
        public bool AA { get; set; }

        /// <summary>
        /// TrunCation - specifies that this message was truncated
        /// due to length greater than that permitted on the
        /// transmission channel.
        /// </summary>
        public bool TC { get; set; }

        /// <summary>
        /// Recursion Desired - this bit may be set in a query and
        /// is copied into the response.If RD is set, it directs
        /// the name server to pursue the query recursively.
        /// Recursive query support is optional.
        /// </summary>
        public bool RD { get; set; }

        /// <summary>
        /// Recursion Available - this be is set or cleared in a
        /// response, and denotes whether recursive query support is
        /// available in the name server.
        /// </summary>
        public bool RA { get; set; }

        /// <summary>
        /// Reserved for future use.  Must be zero in all queries
        /// and responses.
        /// </summary>
        public byte Z { get; }

        /// <summary>
        /// Response code - this 4 bit field is set as part of
        /// responses.
        /// </summary>
        public byte RCODE { get; set; }

        /// <summary>
        /// An unsigned 16 bit integer specifying the number of
        /// entries in the question section.
        /// </summary>
        public short QDCOUNT { get; set; }

        /// <summary>
        /// An unsigned 16 bit integer specifying the number of
        /// resource records in the answer section.
        /// </summary>
        public short ANCOUNT { get; set; }

        /// <summary>
        /// An unsigned 16 bit integer specifying the number of name
        /// server resource records in the authority records
        /// section.
        /// </summary>
        public short NSCOUNT { get; set; }

        /// <summary>
        /// An unsigned 16 bit integer specifying the number of
        /// resource records in the additional records section.
        /// </summary>
        public short ARCOUNT { get; set; }

    }

}
