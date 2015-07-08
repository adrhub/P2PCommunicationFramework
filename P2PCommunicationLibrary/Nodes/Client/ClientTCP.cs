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

        private object _syncRoot = new object();
        #endregion

        #region Properties
        public bool IsListening { get; private set; }

        public IPEndPoint LocalEndPoint { get; private set; }
        public IPEndPoint RemoteEndPoint { get; private set; }
        public Socket ClientSocket { get; private set; }
        #endregion

        public ClientTCP(Socket clientSocket, MessageManager messageManager)
        {
            ClientSocket = clientSocket;
            _messageManager = messageManager;

            LocalEndPoint = (IPEndPoint)ClientSocket.LocalEndPoint;
            RemoteEndPoint = (IPEndPoint)ClientSocket.RemoteEndPoint;
            ClientSocket = clientSocket;
        }

        public void Send(BinaryMessageBase message)
        {
            byte[] buffer = _messageManager.Encode(message);            

            try
            {
                ClientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
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
                ClientSocket.Receive(buffer, 0, buffer.Length, SocketFlags.None);
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
