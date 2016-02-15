using System.Net;
using System.Threading.Tasks;
using P2PCommunicationLibrary.Messages;
using P2PCommunicationLibrary.Net;

namespace P2PCommunicationLibrary.SimplePeers
{
    public class ServerPeer
    {        
        private Peer Peer { get; }     
        
        event ClientConnectedToServerPeerEventHandler clientConnectedToServerPeerEvent;

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
            Peer.AddMethodToMessageReceivedEvent(ProcessSuperPeerMessages);
            Task.Factory.StartNew(() => Peer.StartListenMessagesFromSuperPeer());
        }

        public void Close()
        {
            Peer.StopListenMessagesFromSuperPeer();
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

        private void ProcessSuperPeerMessages(IClient client, MessageEventArgs messageEventArgs)
        {                      
            MessageType messageType = messageEventArgs.Message.TypeOfMessage;          
              
            switch (messageType)
            {
                case MessageType.TcpConnection:
                    break;
            }             
        }
    }
}
