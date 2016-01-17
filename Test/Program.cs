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

//            ServerPeer2 server = new ServerPeer2(new IPEndPoint(IPAddress.Parse("127.1.0.0"), 7));
//            server.Encryptor = null;
//            server.Run();

            ServerPeer server = new ServerPeer(new IPEndPoint(IPAddress.Parse("127.1.0.0"), 8090));
            server.Encryptor = null;
            server.Run();

            ClientPeer client = new ClientPeer(new IPEndPoint(IPAddress.Parse("127.1.0.0"), 8090));
            client.Encryptor = null;
            client.Run();

           

            server.AllowConnection(client.GetPeerAddress());
            Thread.Sleep(1000);
            client.Connect(server.GetPeerAddress());
            
            //PeerAddress peerAddress = peer.GetPeerAddress();


            Thread.Sleep(60000);
        }
    }
}
