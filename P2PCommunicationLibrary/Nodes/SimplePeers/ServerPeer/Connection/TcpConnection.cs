using System.Net;
using P2PCommunicationLibrary.Net;

namespace P2PCommunicationLibrary.SimplePeers
{
    class TcpConnection : Connection
    {
        private IServer server;

        public TcpConnection(ServerPeer serverPeer)
            : base(serverPeer)
        {           
        }

        public override void ProcessConnection()
        {
            server = new ServerTCP(IPAddress.Any,);
        }

        public override void Close()
        {
            throw new System.NotImplementedException();
        }
    }
}
