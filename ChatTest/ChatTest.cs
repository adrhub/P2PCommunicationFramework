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
//            TextMessage message = new TextMessage("азазаз");
//            byte[] buffer = (new BinaryEncodingFactory()).GetEncoding(message);
//
//            MessageBase result = (TextMessage)(new BinaryMessageFactory()).GetMessage(buffer);
//            Console.WriteLine(result.TypeOfMessage); 
           
            //PeerEndPoint p = new PeerEndPoint("192.168.1.1:8080", AddresType.Private);
            //IPEndPoint p = new IPEndPoint(19216811L, 80);

//            IPEndPoint p1 = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 80);
//
//            PeerEndPoint peer = new PeerEndPoint(p1, AddresType.Private);

//            Console.WriteLine();
            SuperPeer s = new SuperPeer(IPAddress.Any, 7);
            s.Run();
         

            //Console.WriteLine(s.GetClients());


//            Socket c2 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);           
//            c2.Connect(Dns.GetHostName(), 7);
//            Thread.Sleep(1000);
//            Console.WriteLine(s.GetClients());

//            SuperPeer s = new SuperPeer(IPAddress.Any, 7);
//            s.Run();         
        }

        public static string LocalIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            return localIP;
        }
    }
}
