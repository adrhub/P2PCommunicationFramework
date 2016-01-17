using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using P2PCommunicationLibrary.Messages;

namespace P2PCommunicationLibrary
{
    class ClientTCP : ClientBase
    {        
        private ICommunicator _communicator;

        public ClientTCP(Socket clientSocket, MessageManager messageManager)
            : base(messageManager)
        {
            ClientSocket = clientSocket;         
            InitProperties();
        }

        public ClientTCP(IPEndPoint connectionIpEndPoint, MessageManager messageManager)
            : base(connectionIpEndPoint, messageManager)
        {
            ClientSocket = InitTcpSocketConnection();
            InitProperties();
        }

        private void InitProperties()
        {
            LocalEndPoint = (IPEndPoint) ClientSocket.LocalEndPoint;
            RemoteEndPoint = (IPEndPoint) ClientSocket.RemoteEndPoint;
            _communicator = new TcpCommunicator(ClientSocket);
        }

        private Socket InitTcpSocketConnection()
        {
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(ConnectionIpEndPoint);
            return clientSocket;
        }

        public override void Send(BinaryMessageBase message)
        {
            try
            {
                var buffer = MessageManager.Encode(message);
                _communicator.Write(buffer);
            }
            catch (SocketException)
            {
                Close();
            }
            catch (BinaryEncodingException)
            {
                Console.WriteLine("BinaryEncodingException: Encode");
            }
        }

        public override BinaryMessageBase Read()
        {
            byte[] buffer = new byte[EncodingConstants.MAX_MESSAGE_LENGTH];
            BinaryMessageBase receivedMessage = null;

            try
            {
                buffer = _communicator.Read();
                receivedMessage = MessageManager.Decode(buffer);
            }
            catch (SocketException)
            {
                Close();                
            }
            catch(BinaryEncodingException)
            {
                Console.WriteLine("BinaryEncodingException: Decode");
            }         

            return receivedMessage;
        }
    }
}
