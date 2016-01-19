using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using P2PCommunicationLibrary.Messages;

namespace P2PCommunicationLibrary.Net
{
    abstract class ClientBase : IClient
    {
        public event PeerClosedConnectionEventHandler ConnectionClosedEvent;
        public event MessageReceivedEventHandler MessageReceivedEvent;

        #region Private members
        private readonly object _socketMonitor = new object();
        #endregion

        #region Properties

        protected MessageManager MessageManager { get; private set; }
        protected IPEndPoint ConnectionIpEndPoint { get; private set; }

        public bool IsListeningMessages { get; private set; }

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

        public void ListenMessages()
        {
            lock (_socketMonitor)
            {
                IsListeningMessages = true;

                do
                {
                    BinaryMessageBase receivedMessage = Read();
                    MessageReceivedEvent(this, new MessageEventArgs(receivedMessage));

                } while (IsListeningMessages);
            }
        }

        public void StopListeningMessages()
        {
            if (IsListeningMessages)
            {
                lock (_socketMonitor)
                {                    
                    IsListeningMessages = false;
                }
            }
        }

        public void Close()
        {
            lock (_socketMonitor)
            {
                try
                {                    
                    IsListeningMessages = false;
                    ClientSocket.Close();                    
                    
                }
                catch (SocketException se)
                {
                    Trace.WriteLine("SocketException: " + se.ErrorCode + " " + se.Message);
                }

                if (ConnectionClosedEvent != null)
                    ConnectionClosedEvent.Invoke(this, new EventArgs());
            }
        }
    }
}
