using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using P2PCommunicationLibrary.Messages;

namespace P2PCommunicationLibrary
{
    class ClientTCP : IClient
    {
        public event MessageReceivedEventHandler MessageReceivedEvent;

        #region Private members
        private MessageManager _messageManager;
        private Socket _clientSocket;

        private object _syncRoot = new object();
        #endregion

        #region Public members
        public bool IsListening { get; private set; }

        public IPEndPoint LocalEndPoint { get; private set; }
        public IPEndPoint RemoteEndPoint { get; private set; }
        #endregion

        public ClientTCP(Socket clientSocket, MessageManager messageManager)
        {
            _clientSocket = clientSocket;
            _messageManager = messageManager;

            LocalEndPoint = (IPEndPoint)_clientSocket.LocalEndPoint;
            RemoteEndPoint = (IPEndPoint)_clientSocket.RemoteEndPoint;           
        }

        public void Send(BinaryMessageBase message)
        {
            byte[] buffer = _messageManager.Encode(message);            

            try
            {
                _clientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
            }
            catch (SocketException)
            {
                Close();
            }
            catch (BinaryEncodingException)
            {
                Trace.WriteLine("BinaryEncodingException: Encode");
            }
        }

        public BinaryMessageBase Read()
        {
            byte[] buffer = new byte[EncodingConstants.MAX_MESSAGE_LENGTH];
            BinaryMessageBase receivedMessage = null;

            try
            {
                _clientSocket.Receive(buffer, 0, buffer.Length, SocketFlags.None);
                receivedMessage = _messageManager.Decode(buffer);
            }
            catch (SocketException)
            {
                Close();                
            }
            catch(BinaryEncodingException)
            {
                Trace.WriteLine("BinaryEncodingException: Decode");
            }

            return receivedMessage;
        }

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
                    _clientSocket.Close();
                }
                catch (SocketException se)
                {
                    Trace.WriteLine("SocketException: " + se.ErrorCode + " " + se.Message);
                }
            }
        }        
    }
}
