using System;
using System.Net;
using P2PCommunicationLibrary.Net;

namespace P2PCommunicationLibrary.SimplePeers.ServerInstances
{
    class TcpServerSingleton
    {
        private static readonly Lazy<ServerTCP> lazy =
        new Lazy<ServerTCP>(() => new ServerTCP());

        public static ServerTCP Instance { get { return lazy.Value; } }

        private TcpServerSingleton()
        {
        }
    }
}
