using System;
using System.Net;
using System.Threading.Tasks;
using P2PCommunicationLibrary.Messages;

namespace P2PCommunicationLibrary.SuperPeer
{
    public class SuperPeer
    {
        private IServer _server;
        private MessageManager _messageManager;
        private IEncrtyptor _encrtyptor;

        #region Properties
        public IPAddress Address { get; private set; }        
        public int Port { get; private set; }

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

        #endregion

        public SuperPeer(IPAddress address, int port)
        {
            Address = address;
            Port = port;
        }
        /// <summary>
        /// Runs the server
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
                _server = new ServerTCP(Address, Port, _messageManager);
                _server.NewClientEvent += ClientConnected_EventHandler;
                _server.Listen();
            }            
            catch (Exception)
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
        /// This event occurs when a client connects to the server
        /// </summary>       
        private void ClientConnected_EventHandler(IServer sender, IClient client)
        {
            Task.Factory.StartNew(() =>
            {
                var newClientConnection = new ClientServerConnection(client);
                newClientConnection.ProcessClient();                
            });
        }                
    }
}
