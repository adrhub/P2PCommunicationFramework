using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using P2PCommunicationLibrary;
using P2PCommunicationLibrary.Peers;

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

            ServerPeer server = new ServerPeer(new IPEndPoint(IPAddress.Parse("192.168.1.2"), 7));
            server.Encrtyptor = null;
            server.Run();

            ClientPeer client = new ClientPeer(new IPEndPoint(IPAddress.Parse("192.168.1.2"), 7));
            client.Encrtyptor = null;
            client.Run();

            server.AllowConnection(client.GetPeerAddress());
            client.Connect(server.GetPeerAddress());
            
            //PeerAddress peerAddress = peer.GetPeerAddress();


            Thread.Sleep(60000);

        }
    }
}
