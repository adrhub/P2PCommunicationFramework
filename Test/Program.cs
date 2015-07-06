using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

            Socket c = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            c.Connect(Dns.GetHostName(), 7);
            byte[] buffer = new byte[1] { 1 };
            c.Send(buffer, 0, buffer.Length, SocketFlags.None);

            Thread.Sleep(5000);

            buffer = new byte[2] { 2, 103};
            c.Send(buffer, 0, buffer.Length, SocketFlags.None);

            Thread.Sleep(60000);
        }
    }
}
