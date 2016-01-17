using System.Net;
using P2PCommunicationLibrary.Messages;

namespace P2PCommunicationLibrary.Peers
{
    public class ServerPeer
    {        
        private Peer Peer { get; }            

        public IEncryptor Encryptor
        {
            get { return Peer.Encryptor; }
            set { Peer.Encryptor = value; }
        }   

        public ServerPeer(IPEndPoint superPeerEndPoint)
        {            
            Peer = new Peer(superPeerEndPoint);            
        }

        public ServerPeer(IPEndPoint superPeerEndPoint, IEncryptor encryptor)
        {
            Peer = new Peer(superPeerEndPoint, encryptor);
        }

        public void Run()
        {
            Peer.Run(ClientType.Server);
        }

        public void Close()
        {
            Peer.Close();
        }

        public PeerAddress GetPeerAddress()
        {
            return Peer.PeerAddress;
        }

        public void AllowConnection(PeerAddress peerAddress)
        {
            var connectAsServerMessage = new PeerAddressMessage(peerAddress, MessageType.ConnectAsServer);
            Peer.SendToSuperPeer(connectAsServerMessage);
        }
    }
}
