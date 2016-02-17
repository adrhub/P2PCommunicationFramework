using P2PCommunicationLibrary.Net;

namespace P2PCommunicationLibrary.SimplePeers.ServerPeer
{
    abstract class ServerPeerConnection
    {
        private static readonly object newClientEventMonitor = new object();

        public ServerPeer ServerPeer { get; private set; }
        public PeerAddress PeerAddress { get; private set; }

        public ServerPeerConnection(ServerPeer serverPeer, PeerAddress peerAddress)
        {
            ServerPeer = serverPeer;
            PeerAddress = peerAddress;
        }

        public abstract void ProcessConnection();
        public abstract void Close();

        protected static void AddMethodToNewClientEvent(IServer server, ClientConnectedEventHandler clientConnectedEventHandler)
        {
            lock (newClientEventMonitor)
            {
                server.NewClientEvent += clientConnectedEventHandler;
            }            
        }

        protected static void RemoveMethodFromNewClientEvent(IServer server, ClientConnectedEventHandler clientConnectedEventHandler)
        {
            lock (clientConnectedEventHandler)
            {
                server.NewClientEvent -= clientConnectedEventHandler;
            }            
        }
    }
}
