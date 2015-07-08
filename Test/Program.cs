using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using P2PCommunicationLibrary;
using P2PCommunicationLibrary.ClientPeer;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
//            Socket c = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//            Socket c2 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//            Socket c1 = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
//
//            c.Bind(new IPEndPoint(IPAddress.Any, 7));
//            Console.WriteLine("sadas");
//
//            c2.Bind(new IPEndPoint(IPAddress.Any, 7));
//            Console.WriteLine("sadas");
//
//            c1.Bind(new IPEndPoint(IPAddress.Any, 7));
//            Console.WriteLine("sadas");
//            Thread.Sleep(60000);

            ClientPeer peer = new ClientPeer(new IPEndPoint(IPAddress.Parse("192.168.1.3"), 7));
            peer.Encrtyptor = null;
            peer.Run();
            PeerAddress peerAddress = peer.GetPeerAddress();

        }
    }
}
