using System;
using System.Net;
using System.Threading.Tasks;
using P2PCommunicationLibrary.Messages;
using P2PCommunicationLibrary.Net;

namespace P2PCommunicationLibrary.SimplePeers.ServerPeer
{
    public class ServerPeer
    {        
        internal Peer Peer { get; }
        public int ServerPeerPort { get; private set; }

        private Lazy<ServerTCP> _lazyTcpServer;

        event ClientConnectedToServerPeerEventHandler clientConnectedToServerPeerEvent;

        public IEncryptor Encryptor
        {
            get { return Peer.Encryptor; }
            set { Peer.Encryptor = value; }
        }

        private ServerPeer()
        {
            InitLazyTcpServer();
        }

        private void InitLazyTcpServer()
        {
            _lazyTcpServer = new Lazy<ServerTCP>(() =>
            {
                MessageManager manager = new MessageManager(Encryptor);
                IPAddress ipAddress = GetPeerAddress().PrivateEndPoint.Address;
                int port = ServerPeerPort;

                var serverTcp = new ServerTCP(ipAddress, port, manager);

                Task.Factory.StartNew(() => serverTcp.Listen());
                return serverTcp;
            });
        }

        public ServerPeer(IPEndPoint superPeerEndPoint)
            : this()
        {            
            Peer = new Peer(superPeerEndPoint);            
        }        

        public ServerPeer(IPEndPoint superPeerEndPoint, IEncryptor encryptor)
            : this()
        {
            Peer = new Peer(superPeerEndPoint, encryptor);
        }

        public ServerPeer(IPEndPoint superPeerEndPoint, int serverPeerPort)
            : this(superPeerEndPoint)
        {
            ServerPeerPort = serverPeerPort;
        }

        public ServerPeer(IPEndPoint superPeerEndPoint, int serverPeerPort, IEncryptor encryptor)
            : this(superPeerEndPoint, encryptor)
        {
            ServerPeerPort = serverPeerPort;
        }
        
        public void Run()
        {
            Peer.Run(ClientType.Server);
            ProcessSuperPeerMessages();           
        }       

        private void ProcessSuperPeerMessages()
        {
            Peer.AddMethodToMessageReceivedEvent(ProcessSuperPeerMessages);
            Task.Factory.StartNew(() => Peer.StartListenMessagesFromSuperPeer());
        }

        private void ProcessSuperPeerMessages(IClient client, MessageEventArgs messageEventArgs)
        {
            RequestMessage requestMessage = (RequestMessage)messageEventArgs.Message;

            switch (requestMessage.RequestedMessageType)
            {
                case MessageType.TcpConnection:
                    ServerPeerConnection serverPeerConnection = new TcpServerPeerConnection(this);
                    serverPeerConnection.ProcessConnection();
                    break;
            }
        }

        public void Close()
        {
            Peer.StopListenMessagesFromSuperPeer();
            Peer.Close();

            if (_lazyTcpServer.IsValueCreated)            
                _lazyTcpServer.Value.Close();
            
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
        
        internal ServerTCP GetTcpServerInstance()
        {            
            return _lazyTcpServer.Value;
        }
    }
}
