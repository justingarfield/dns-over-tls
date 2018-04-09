namespace JGarfield.DNSOverTLS.Shared.DomainProtocol
{

    /// <summary>
    /// The type of Query contained in an Internet Protocol Message.
    /// </summary>
    public enum QueryType
    {

        /// <summary>
        /// A Standard Query
        /// </summary>
        QUERY = 0,

        /// <summary>
        /// An Inverse Query
        /// </summary>
        IQUERY = 1,

        /// <summary>
        /// A Server Status Request
        /// </summary>
        STATUS = 2,

        /// <summary>
        /// Reserved for Future Use
        /// </summary>
        RESERVED = 3
        
    }

}
