using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using P2PCommunicationLibrary.Messages;

namespace P2PCommunicationLibrary
{
    abstract class ClientBase : IClient
    {
        public event MessageReceivedEventHandler MessageReceivedEvent;

        #region Private members
        private object _syncRoot = new object();
        #endregion

        #region Properties

        protected MessageManager MessageManager { get; private set; }
        protected IPEndPoint ConnectionIpEndPoint { get; private set; }

        public bool IsListening { get; private set; }

        public IPEndPoint LocalEndPoint { get; protected set; }
        public IPEndPoint RemoteEndPoint { get; protected set; }                       

        public Socket ClientSocket { get; protected set; }
        #endregion

        protected ClientBase(MessageManager messageManager)
        {
            MessageManager = messageManager;
        }

        protected ClientBase(IPEndPoint connectionIpEndPoint, MessageManager messageManager)
            :this(messageManager)
        {
            ConnectionIpEndPoint = connectionIpEndPoint;            
        }
        
        public abstract void Send(BinaryMessageBase message);

        public abstract BinaryMessageBase Read();

        public void Listen()
        {
            lock (_syncRoot)
            {
                IsListening = true;

                do
                {
                    BinaryMessageBase receivedMessage = Read();
                    MessageReceivedEvent(this, new MessageEventArgs(receivedMessage));

                } while (IsListening);
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

        public void Close()
        {
            lock (_syncRoot)
            {
                try
                {
                    IsListening = false;
                    ClientSocket.Close();
                }
                catch (SocketException se)
                {
                    Trace.WriteLine("SocketException: " + se.ErrorCode + " " + se.Message);
                }
            }
        }
    }
}
