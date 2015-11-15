using System.Net;
using P2PCommunicationLibrary.Messages;

namespace P2PCommunicationLibrary.Peers
{
    public class SP
    {        
        private Peer Peer { get; }            

        public IEncryptor Encryptor
        {
            get { return Peer.Encryptor; }
            set { Peer.Encryptor = value; }
        }   

        public SP(IPEndPoint superPeerEndPoint)
        {            
            Peer = new Peer(superPeerEndPoint);            
        }

        public SP(IPEndPoint superPeerEndPoint, IEncryptor encryptor)
        {
            Peer = new Peer(superPeerEndPoint, encryptor);
        }

        public void Run()
        {
            Peer.Run();
        }

        public void Close()
        {
            Peer.Close();
        }

        public PeerAddress GetPeerAddress()
        {
            return Peer.GetPeerAddress();
        }

        public void AllowConnection(PeerAddress peerAddress)
        {
            var connectAsServerMessage = new PeerAddressMessage(peerAddress, MessageType.ConnectAsServer);
            Peer.SendToSuperPeer(connectAsServerMessage);
        }
    }
}
