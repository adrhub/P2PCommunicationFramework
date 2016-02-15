using System.Net;
using P2PCommunicationLibrary.Messages;
using P2PCommunicationLibrary.Net;

namespace P2PCommunicationLibrary.SimplePeers
{
    public class ClientPeer
    {
        private Peer Peer { get; }

        public IEncryptor Encryptor
        {
            get { return Peer.Encryptor; }
            set { Peer.Encryptor = value; }
        }

        public ClientPeer(IPEndPoint superPeerEndPoint)
        {
            Peer = new Peer(superPeerEndPoint);
        }

        public ClientPeer(IPEndPoint superPeerEndPoint, IEncryptor encryptor)
        {
            Peer = new Peer(superPeerEndPoint, encryptor);
        }

        public void Run()
        {
            Peer.Run(ClientType.Client);
        }

        public void Close()
        {
            Peer.Close();
        }

        public PeerAddress GetPeerAddress()
        {
            return Peer.PeerAddress;
        }

        public void Connect(PeerAddress peerAddress)
        {
            var connectAsServerMessage = new PeerAddressMessage(peerAddress, MessageType.ConnectAsClient);
            Peer.SendToSuperPeer(connectAsServerMessage);
        }
    }
}
