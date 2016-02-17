using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using P2PCommunicationLibrary;
using P2PCommunicationLibrary.SuperPeer;

namespace ChatTest
{
    class ChatTest
    {
        static void Main(string[] args)
        {
            SuperPeer s = new SuperPeer(IPAddress.Parse("127.0.0.1"), 8090);
            s.Run();               
        }

        public static string LocalIpAddress()
        {
            string localIp = "";
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIp = ip.ToString();
                    break;
                }
            }
            return localIp;
        }
    }
}
