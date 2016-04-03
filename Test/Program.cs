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
            int port = 8091;

            ServerPeer server = new ServerPeer(superPeerEndPoint, port);           
            server.Run();

            ClientPeer client = new ClientPeer(superPeerEndPoint);
            client.Run();

            //used Task.Factory.StartNew(() and sleep to simulate that this tasks are performed on different applications/machines
            Task.Factory.StartNew(() => server.AllowConnection(client.GetPeerAddress()));         
            Thread.Sleep(3000);    
            Task.Factory.StartNew(() => client.Connect(server.GetPeerAddress()));

            Thread.Sleep(3000);
            server.Send(new byte[] {1,2,3,4,5});
            Console.WriteLine(client.Read());
            
            client.Close();
            Thread.Sleep(1000);
            server.Close();            

            Thread.Sleep(60000);
        }
    }
}
