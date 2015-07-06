using System.Net;

namespace P2PCommunicationLibrary
{
    public class PeerAddress
    {
        public IPEndPoint LocalEndPoint { get; set; }
        public IPEndPoint PublicEndPoint { get; set; }

        public PeerAddress()
        {            
        }

        public PeerAddress(IPEndPoint localEndPoint, IPEndPoint publicEndPoint)
        {
            LocalEndPoint = localEndPoint;
            PublicEndPoint = publicEndPoint;
        }
    }
}
