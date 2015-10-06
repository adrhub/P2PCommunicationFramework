using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using P2PCommunicationLibrary.Messages;

namespace P2PCommunicationLibrary.Peers
{
    public abstract class Peer
    {
        private IClient _superPeerClient;
        private IPEndPoint _superPeerEndPoint;
        private IEncrtyptor _encrtyptor;
        private MessageManager _messageManager;

        public bool IsRunning { get; private set; }
        public IEncrtyptor Encrtyptor
        {
            get { return _encrtyptor; }
            set
            {
                if (!IsRunning)
                {
                    _encrtyptor = value;
                }
            }
        }    
        
        public Peer(IPEndPoint superPeerEndPoint)
        {
            if (Encrtyptor != null)
                _messageManager = new MessageManager(Encrtyptor);
            else
                _messageManager = new MessageManager();

            _superPeerEndPoint = superPeerEndPoint;
        }

        /// <summary>
        /// Connectiong to Super Peer
        /// </summary>
        public void Run()
        {
            IsRunning = true;

            if (Encrtyptor != null)
                _messageManager = new MessageManager(Encrtyptor);
            else
                _messageManager = new MessageManager();

            try
            {
                Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                clientSocket.Connect(_superPeerEndPoint);
                _superPeerClient = new ClientTCP(clientSocket, _messageManager);              
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
//            if (IsRunning)
//            {
//                IsRunning = false;
//                _superPeerClient.Close();
//            }
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
    }
}
