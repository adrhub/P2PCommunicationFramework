using System.Net;
using System.Net.Mail;
using P2PCommunicationLibrary.Messages;
using P2PCommunicationLibrary.Net;

namespace P2PCommunicationLibrary.SimplePeers.ClientPeer
{
    public class ClientPeer
    {
        private IClient _server;
        internal Peer Peer { get; private set; }

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

        public PeerAddress GetPeerAddress()
        {
            return Peer.PeerAddress;
        }

        public void Connect(PeerAddress peerAddress)
        {
            var connectAsServerMessage = new PeerAddressMessage(peerAddress, MessageType.ConnectAsClient);
            Peer.SendToSuperPeer(connectAsServerMessage);

            Peer.ReadFromSuperPeer();            
            var connection = new TcpClientPeerConnection(this, Peer.PeerAddress);
            _server = connection.GetConnection();
            Peer.Close();
        }

//        private void ProcessConnectionToServerPeer(RequestMessage requestMessage)
//        {        
//            switch (requestMessage.RequestedMessageType)
//            {
//                case MessageType.TcpConnection:
//                    var connection = new TcpClientPeerConnection(this, Peer.PeerAddress);
//                    _server = connection.GetConnection();
//                    break;
//            }
//        }

        public void Send(byte[] byteArray)
        {
            Peer.Send(_server, byteArray);
        }

        public byte[] Read()
        {
            return Peer.Read(_server);
        }

        public void Close()
        {
            if (_server != null)
                _server.Close();

            Peer.Close();
        }
    }
}
