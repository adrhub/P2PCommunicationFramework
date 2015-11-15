using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using P2PCommunicationLibrary.Messages;

namespace P2PCommunicationLibrary.Peers
{
    class Peer
    {
        private IClient _superPeerClient;
        private IPEndPoint _superPeerEndPoint;
        private IEncryptor _encryptor;
        private MessageManager _messageManager;

        public PeerAddress PeerAddress{ get; private set; }
        public bool IsRunning { get; private set; }
        public IEncryptor Encryptor
        {
            get { return _encryptor; }
            set
            {
                if (!IsRunning)
                {
                    _encryptor = value;
                }
            }   
        }    
        
        public Peer(IPEndPoint superPeerEndPoint)
        {
            InitMessageManager();
            _superPeerEndPoint = superPeerEndPoint;
        }

        public Peer(IPEndPoint superPeerEndPoint, IEncryptor encryptor)
        {
            Encryptor = encryptor;
            InitMessageManager();
            _superPeerEndPoint = superPeerEndPoint;
        }

        private void InitMessageManager()
        {
            if (Encryptor != null)
                _messageManager = new MessageManager(Encryptor);
            else
                _messageManager = new MessageManager();
        }

        /// <summary>
        /// Connectiong to Super Peer
        /// </summary>
        public void Run()
        {
            IsRunning = true;           

            try
            {              
                _superPeerClient = new ClientTCP(_superPeerEndPoint, _messageManager);              
                _superPeerClient.Send(new ConnectionMessage());
                //Read confirmation message
                _superPeerClient.Read();
            }
            catch (SocketException se)
            {
                Trace.WriteLine("Error connectiong to the server");
                Trace.WriteLine("SocketException: " + se.ErrorCode + " " + se.Message);
                Close();
                throw;
            }
        }

        public void Close()
        {
            if (IsRunning)
            {
                IsRunning = false;
                _superPeerClient.Close();
            }
        }    

        public PeerAddress GetPeerAddress()
        {           
            var requestMessage = new RequestMessage(MessageType.ClientPeerAddress);
            _superPeerClient.Send(requestMessage);

            PeerAddress peerAddress = ((PeerAddressMessage)_superPeerClient.Read()).PeerAddress;
            peerAddress.PrivateEndPoint = new IPEndPoint(LocalIPAddress(), _superPeerClient.LocalEndPoint.Port);

            return peerAddress;
        }

        private static IPAddress LocalIPAddress()
        {
            IPHostEntry host;
            IPAddress localIP = null ;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip;
                    break;
                }
            }
            return localIP;
        }

        public void SendToSuperPeer(BinaryMessageBase message)
        {
            _superPeerClient.Send(message);
        }

        public BinaryMessageBase ReadFromSuperPeer()
        {
            return _superPeerClient.Read();
        }
    }
}
