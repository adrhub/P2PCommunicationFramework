using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using P2PCommunicationLibrary.Messages;
using P2PCommunicationLibrary.Net;

namespace P2PCommunicationLibrary.SimplePeers.ServerPeer
{
    public class ServerPeer
    {        
        internal Peer Peer { get; private set; }
        public int ServerPeerPort { get; private set; }
        private IClient _client;
        

        public IEncryptor Encryptor
        {
            get { return Peer.Encryptor; }
            set { Peer.Encryptor = value; }
        }

        private ServerPeer()
        {            
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
//            ProcessSuperPeerMessages();           
        }       

//        private void ProcessSuperPeerMessages()
//        {
//            Peer.AddMethodToMessageReceivedEvent(ProcessSuperPeerMessages);
//            Task.Factory.StartNew(() => Peer.StartListenMessagesFromSuperPeer());
//        }

//        private void ProcessSuperPeerMessages(IClient client, MessageEventArgs messageEventArgs)
//        {
//            RequestMessage requestMessage = (RequestMessage)messageEventArgs.Message;
//
//            switch (requestMessage.RequestedMessageType)
//            {
//                case MessageType.TcpConnection:
//                    ServerPeerConnection serverPeerConnection = new TcpServerPeerConnection(this);
//                    _client = serverPeerConnection.GetConnection();
//                    break;
//            }
//        }

        public void Close()
        {
            if (_client != null)
                _client.Close();
            //Peer.StopListenMessagesFromSuperPeer();
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

            Peer.ReadFromSuperPeer();
            ServerPeerConnection serverPeerConnection = new TcpServerPeerConnection(this);
            _client = serverPeerConnection.GetConnection();

            Peer.Close();
            Console.WriteLine("Client connected to server");            
        }

        public void Send(byte[] byteArray)
        {
            Peer.Send(_client, byteArray);
        }

        public byte[] Read()
        {
            return Peer.Read(_client);
        }
    }
}
