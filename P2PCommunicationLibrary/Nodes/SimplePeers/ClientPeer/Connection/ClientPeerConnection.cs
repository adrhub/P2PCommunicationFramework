namespace P2PCommunicationLibrary.SimplePeers.ClientPeer
{
    abstract class ClientPeerConnection
    {
        public ClientPeer ServerPeer { get; private set; }
        public PeerAddress PeerAddress { get; private set; }

        public ClientPeerConnection(ClientPeer serverPeer, PeerAddress peerAddress)
        {
            ServerPeer = serverPeer;
            PeerAddress = peerAddress;
        }

        public abstract void ProcessConnection();
        public abstract void Close();        
    }
}
