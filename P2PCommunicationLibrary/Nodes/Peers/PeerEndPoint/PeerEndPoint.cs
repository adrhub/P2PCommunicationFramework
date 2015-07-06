using System;
using System.Globalization;
using System.Net;

namespace P2PCommunicationLibrary.Communication.Peers
{
    public class PeerEndPoint
    {
        public IPEndPoint LocalEndPoint { get; private set; }
        public IPEndPoint PublicEndPoint { get; private set; }

        public PeerEndPoint(IPEndPoint localEndPoint, IPEndPoint publicEndPoint)
        {            
            LocalEndPoint = localEndPoint;
            PublicEndPoint = publicEndPoint;
        }

        #region Helper Methods
        private static IPEndPoint CreateIPEndPoint(string endPoint)
        {
            string[] ep = endPoint.Split(':');
            if (ep.Length != 2) throw new FormatException("Invalid endpoint format");
            IPAddress ip;
            if (!IPAddress.TryParse(ep[0], out ip))
            {
                throw new FormatException("Invalid ip-adress");
            }
            int port;
            if (!int.TryParse(ep[1], NumberStyles.None, NumberFormatInfo.CurrentInfo, out port))
            {
                throw new FormatException("Invalid port");
            }
            return new IPEndPoint(ip, port);
        }

        private static bool IsLocalIP(IPAddress ipaddress)
        {
            String[] straryIPAddress = ipaddress.ToString()
                .Split(new String[] {"."}, StringSplitOptions.RemoveEmptyEntries);

            int[] iaryIPAddress = new int[]
            {
                int.Parse(straryIPAddress[0]), int.Parse(straryIPAddress[1]), int.Parse(straryIPAddress[2]),
                int.Parse(straryIPAddress[3])
            };
            if (iaryIPAddress[0] == 10 || (iaryIPAddress[0] == 192 && iaryIPAddress[1] == 168) ||
               (iaryIPAddress[0] == 172 && (iaryIPAddress[1] >= 16 && iaryIPAddress[1] <= 31)))
            {
                return true;
            }
            else
            {
                // IP Address is "probably" public. This doesn't catch some VPN ranges like OpenVPN and Hamachi.
                return false;
            }
        }
        #endregion
    }    
}
