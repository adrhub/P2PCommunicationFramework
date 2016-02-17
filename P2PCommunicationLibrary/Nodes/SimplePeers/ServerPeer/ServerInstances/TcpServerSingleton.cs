using System;
using System.Net;
using System.Threading.Tasks;
using P2PCommunicationLibrary.Messages;
using P2PCommunicationLibrary.Net;

namespace P2PCommunicationLibrary.SimplePeers.ServerPeer.ServerInstances
{
    static class TcpServerSingleton
    {
        private static IEncryptor _encryptor;
        private static IPEndPoint _serverEndPoint;

        private static Lazy<ServerTCP> lazyServer;      

        static TcpServerSingleton()
        {
            lazyServer = new Lazy<ServerTCP>(() =>
            {
                MessageManager manager = new MessageManager(_encryptor);                               
                IPAddress ipAddress = _serverEndPoint.Address;
                int port = _serverEndPoint.Port;

                var serverTcp = new ServerTCP(ipAddress, port, manager);

                Task.Factory.StartNew(() => serverTcp.Listen());
                return serverTcp;
            });
        }

        public static ServerTCP GetInstance()
        {
            return lazyServer.Value;
        }

        public static void SetEncryptor(IEncryptor encryptor)
        {
            _encryptor = encryptor;
        }

        public static void SetServerEndPoint(IPEndPoint serverEndPoint)
        {
            _serverEndPoint = serverEndPoint;
        }
    }
}
