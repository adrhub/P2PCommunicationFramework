using System.Net;
using P2PCommunicationLibrary.Messages;
using P2PCommunicationLibrary.Net;

namespace P2PCommunicationLibrary.SimplePeers.ClientPeer
{
    public class ClientPeer
    {
        internal Peer Peer { get; }

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

            var connectionAllowed = (RequestMessage) Peer.ReadFromSuperPeer();
            ProcessConnectionToServerPeer(connectionAllowed);
        }

        private void ProcessConnectionToServerPeer(RequestMessage requestMessage)
        {
            switch (requestMessage.RequestedMessageType)
            {
                case MessageType.TcpConnection:
                    var connection = new TcpClientPeerConnection(this, Peer.PeerAddress);
                    connection.ProcessConnection();
                    break;
            }
        }
    }
}
