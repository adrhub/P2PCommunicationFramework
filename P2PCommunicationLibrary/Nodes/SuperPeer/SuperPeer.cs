using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using P2PCommunicationLibrary.Messages;

namespace P2PCommunicationLibrary.SuperPeer
{
    public class SuperPeer
    {
        private IServer _server;
        private MessageManager _messageManager;
        private IEncryptor _encryptor;

        #region Properties
        public IPAddress Address { get; private set; }        
        public int Port { get; private set; }

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

        #endregion

        public SuperPeer(IPAddress address, int port)
        {
            Address = address;
            Port = port;
        }
        /// <summary>
        /// Runs the Server
        /// </summary>
        public void Run()
        {
            IsRunning = true;

            if (Encryptor != null)
                _messageManager = new MessageManager(Encryptor);
            else            
                _messageManager = new MessageManager();            

            try
            {
                _server = new ServerTCP(Address, Port, _messageManager);
                _server.NewClientEvent += ClientConnected_EventHandler;
                _server.Listen();
            }            
            catch (SocketException se)
            {
                StopRunning();
                throw;
            }
        }

        public void StopRunning()
        {
            if (IsRunning)
            {
                IsRunning = false;
                _server.StopListening();
            }
        }

        /// <summary>
        /// This event occurs when a Client connects to the server
        /// </summary>       
        private void ClientConnected_EventHandler(IServer sender, IClient client)
        {
            Task.Factory.StartNew(() =>
            {
                var newClientConnection = new PeerConnectionManager(client);
                newClientConnection.BeginProcessClientConnection();                
            });

//            var newClientConnection = new PeerConnectionManager(Client);
//            newClientConnection.BeginProcessClientConnection();
//
//            Thread thread = new Thread(newClientConnection.BeginProcessClientConnection);
//            thread.Start();
        }                
    }
}
