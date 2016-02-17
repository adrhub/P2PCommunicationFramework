using System.Net;
using System.Threading.Tasks;
using P2PCommunicationLibrary.Messages;
using P2PCommunicationLibrary.Net;
using P2PCommunicationLibrary.SimplePeers.ServerPeer.ServerInstances;

namespace P2PCommunicationLibrary.SimplePeers.ServerPeer
{
    public class ServerPeer
    {        
        internal Peer Peer { get; }
        public IPEndPoint ServerPeerEndPoint { get; private set; }

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

        public ServerPeer(IPEndPoint superPeerEndPoint, IPEndPoint serverPeerEndPoint)
            : this(superPeerEndPoint)
        {
            ServerPeerEndPoint = serverPeerEndPoint;
        }

        public ServerPeer(IPEndPoint superPeerEndPoint, IPEndPoint serverPeerEndPoint, IEncryptor encryptor)
            : this(superPeerEndPoint, encryptor)
        {
            ServerPeerEndPoint = serverPeerEndPoint;
        }

        public void Run()
        {
            Peer.Run(ClientType.Server);

            ProcessSuperPeerMessages();
            SetServerInstancesInitData();
        }

        private void SetServerInstancesInitData()
        {
            TcpServerSingleton.SetEncryptor(Encryptor);
            TcpServerSingleton.SetServerEndPoint(ServerPeerEndPoint);
        }

        private void ProcessSuperPeerMessages()
        {
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
            RequestMessage requestMessage = (RequestMessage)messageEventArgs.Message;          
              
            switch (requestMessage.RequestedMessageType)
            {
                case MessageType.TcpConnection:
                    ServerPeerConnection serverPeerConnection = new TcpServerPeerConnection(this, GetPeerAddress());
                    serverPeerConnection.ProcessConnection();
                    break;
            }             
        }
    }
}
