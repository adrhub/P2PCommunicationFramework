using System.Net;
using P2PCommunicationLibrary.Peers;

namespace P2PCommunicationLibrary
{
    public class PeerAddress
    {      
        public IPEndPoint PrivateEndPoint { get; set; }
        public IPEndPoint PublicEndPoint { get; set; }

        public PeerAddress()
        {            
        }

        public PeerAddress(IPEndPoint privateEndPoint, IPEndPoint publicEndPoint)
        {
            PrivateEndPoint = privateEndPoint;
            PublicEndPoint = publicEndPoint;
        }      

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PeerAddress) obj);
        }

        protected bool Equals(PeerAddress other)
        {
            return Equals(PrivateEndPoint, other.PrivateEndPoint) 
                && Equals(PublicEndPoint, other.PublicEndPoint);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((PrivateEndPoint != null ? PrivateEndPoint.GetHashCode() : 0) * 397) 
                    ^ (PublicEndPoint != null ? PublicEndPoint.GetHashCode() : 0);
            }
        }

        public PeerAddress Parse(string peerAddress)
        {
               
        }
    }
}
