using P2PCommunicationLibrary.Net;

namespace P2PCommunicationLibrary.SimplePeers.ClientPeer
{
    abstract class ClientPeerConnection
    {
        public IClient Client { get; protected set; }
        public ClientPeer ClientPeer { get; private set; }
        public PeerAddress ClientPeerAddress { get; private set; }

        public ClientPeerConnection(ClientPeer clientPeer, PeerAddress clientPeerAddress)
        {
            ClientPeer = clientPeer;
            ClientPeerAddress = clientPeerAddress;
        }

        public abstract void ProcessConnection();

        public virtual void Close()
        {
            if (Client != null)
                Client.Close();   
        }      
    }
}
