namespace P2PCommunicationLibrary.SimplePeers
{
    abstract class Connection
    {
        public ServerPeer ServerPeer { get; private set; }

        public Connection(ServerPeer serverPeer)
        {
            ServerPeer = serverPeer;
        }

        public abstract void ProcessConnection();
        public abstract void Close();
    }
}
