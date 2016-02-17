using System;
using System.Globalization;
using System.Net;

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
            return Equals((PeerAddress)obj);
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
            return null;
        }

        private static IPEndPoint ParseIpEndPoint(string endPoint)
        {
            if (endPoint == "null")
                return null;

            string[] ep = endPoint.Split(':');

            if (ep.Length < 2)
                throw new FormatException("Invalid endpoint format");

            IPAddress ip;

            if (ep.Length > 2)
            {
                if (!IPAddress.TryParse(string.Join(":", ep, 0, ep.Length - 1), out ip))
                {
                    throw new FormatException("Invalid ip-adress");
                }
            }
            else
            {
                if (!IPAddress.TryParse(ep[0], out ip))
                {
                    throw new FormatException("Invalid ip-adress");
                }
            }

            int port;

            if (!int.TryParse(ep[ep.Length - 1], NumberStyles.None, NumberFormatInfo.CurrentInfo, out port))
            {
                throw new FormatException("Invalid port");
            }

            return new IPEndPoint(ip, port);
        }
    }
}