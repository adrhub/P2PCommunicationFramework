using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using P2PCommunicationLibrary.Messages;

namespace P2PCommunicationLibrary
{
    class ClientTCP : ClientBase
    {        
        public ClientTCP(Socket clientSocket, MessageManager messageManager)
            : base(messageManager)
        {
            ClientSocket = clientSocket;         
            LocalEndPoint = (IPEndPoint)ClientSocket.LocalEndPoint;
            RemoteEndPoint = (IPEndPoint)ClientSocket.RemoteEndPoint;           
        }

        public ClientTCP(IPEndPoint connectionIpEndPoint, MessageManager messageManager)
            : base(connectionIpEndPoint, messageManager)
        {
            ClientSocket = InitTcpSocketConnection();
            LocalEndPoint = (IPEndPoint) ClientSocket.LocalEndPoint;
            RemoteEndPoint = (IPEndPoint) ClientSocket.RemoteEndPoint;
        }

        private Socket InitTcpSocketConnection()
        {
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(ConnectionIpEndPoint);
            return clientSocket;
        }

        public override void Send(BinaryMessageBase message)
        {
            byte[] buffer = MessageManager.Encode(message);            

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

        public override BinaryMessageBase Read()
        {
            byte[] buffer = new byte[EncodingConstants.MAX_MESSAGE_LENGTH];
            BinaryMessageBase receivedMessage = null;

            try
            {
                ClientSocket.Receive(buffer, 0, buffer.Length, SocketFlags.None);
                receivedMessage = MessageManager.Decode(buffer);
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
    }
}
