using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using P2PCommunicationLibrary;
using P2PCommunicationLibrary.SimplePeers;
using P2PCommunicationLibrary.SimplePeers.ClientPeer;
using P2PCommunicationLibrary.SimplePeers.ServerPeer;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            IPEndPoint superPeerEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8090);
            IPEndPoint serverPeerEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.2"), 8091);

            ServerPeer server = new ServerPeer(superPeerEndPoint, serverPeerEndPoint);           
            server.Run();

            ClientPeer client = new ClientPeer(superPeerEndPoint);
            client.Run();

            server.AllowConnection(client.GetPeerAddress());  
            Thread.Sleep(1000);          
            client.Connect(server.GetPeerAddress());
            
//            client.Close();
//            Thread.Sleep(1000);
//            server.Close();            

            Thread.Sleep(60000);
        }
    }
}
