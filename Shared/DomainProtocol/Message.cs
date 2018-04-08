namespace JGarfield.DNSOverTLS.Shared.DomainProtocol
{

    public class Message
    {

        public Header Header { get; set; }

        public Question Question { get; set; }

        public ResourceRecord Answer { get; set; }
        
        public ResourceRecord Authority { get; set; }

        public ResourceRecord Additional { get; set; }

    }

}
