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

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerPeer server = new ServerPeer(new IPEndPoint(IPAddress.Parse("127.1.0.0"), 8090));           
            server.Run();

            ClientPeer client = new ClientPeer(new IPEndPoint(IPAddress.Parse("127.1.0.0"), 8090));
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
