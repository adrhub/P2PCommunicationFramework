namespace P2PCommunicationLibrary.SimplePeers.ClientPeer
{
    abstract class ClientPeerConnection
    {
        public ClientPeer ClientPeer { get; private set; }
        public PeerAddress ClientPeerAddress { get; private set; }

        public ClientPeerConnection(ClientPeer clientPeer, PeerAddress clientPeerAddress)
        {
            ClientPeer = clientPeer;
            ClientPeerAddress = clientPeerAddress;
        }

        public abstract void ProcessConnection();
        public abstract void Close();        
    }
}
