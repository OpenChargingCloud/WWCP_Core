namespace cloud.charging.open.protocols.WWCP.UnitTests
{
    internal class DNSClient
    {
        private bool SearchForIPv6DNSServers;

        public DNSClient(bool SearchForIPv6DNSServers)
        {
            this.SearchForIPv6DNSServers = SearchForIPv6DNSServers;
        }
    }
}