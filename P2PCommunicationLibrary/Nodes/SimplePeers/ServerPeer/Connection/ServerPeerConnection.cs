using P2PCommunicationLibrary.Net;

namespace P2PCommunicationLibrary.SimplePeers.ServerPeer
{
    abstract class ServerPeerConnection
    {
        private static readonly object newClientEventMonitor = new object();

        public ServerPeer ServerPeer { get; private set; }        
        public IClient Client { get; protected set; }

        public ServerPeerConnection(ServerPeer serverPeer)
        {
            ServerPeer = serverPeer;
         
        }

        public abstract void ProcessConnection();        

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

        public void Close()
        {
            if (Client != null)
                Client.Close();
        }
    }
}
