using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using P2PCommunicationLibrary.Messages;

namespace P2PCommunicationLibrary
{
    class ServerTCP : IServer
    {
        public event ClientConnectedEventHandler NewClientEvent;

        #region Private Members
        private Socket _listener;
        private MessageManager _messageManager;

        private int _backlog = 32;
        private object _syncRoot = new object();
        #endregion

        #region Properties
        public IPAddress Address { get; private set; }
        public int Port { get; private set; }
        public bool IsListening { get; private set; }
        #endregion

        #region Constructors
        public ServerTCP(IPAddress address, int port, MessageManager messageManager)
        {
            Port = port;
            Address = address;
            _messageManager = messageManager;
        }
        #endregion        

        #region Public Methods
        public void Listen()
        {
            try
            {
                lock (_syncRoot)
                {
                    _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    _listener.Bind(new IPEndPoint(Address, Port));
                    // fire up the server
                    _listener.Listen(_backlog);

                    // set listening bit
                    IsListening = true;
                }

                // Enter the listening loop.
                do
                {
                    Trace.Write("Looking for someone to talk to... ");

                    // Wait for connection
                    Socket newClient = _listener.Accept();
                    Trace.WriteLine("Connected to new client");

                    // queue a request to take care of the client
                    ProcessClient(new ClientTCP(newClient, _messageManager));
                    //ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessClient), newClient);
                }
                while (IsListening);
            }
            catch (SocketException se)
            {
                Trace.WriteLine("SocketException: " + se.ErrorCode + " " + se.Message);
            }
            finally
            {
                // shut it down
                StopListening();
            }
        }

        public void StopListening()
        {
            if (IsListening)
            {
                lock (_syncRoot)
                {
                    // set listening bit
                    IsListening = false;                  
                }
            }
        }
        #endregion

        #region Private Methods
        private void ProcessClient(IClient client)
        {
            NewClientEvent(this, client);
        }
        #endregion
    }
}