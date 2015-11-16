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
//                Console.Write("\nSend: ");
//                foreach (var b in buffer)
//                {
//                    Console.Write(b + " ");
//                }                                

                ClientSocket.Receive(new byte[1], 0, 1, SocketFlags.None);
                ClientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
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
                ClientSocket.Send(new byte[]{1}, 0, 1, SocketFlags.None);
                ClientSocket.Receive(buffer, 0, buffer.Length, SocketFlags.None);
                receivedMessage = MessageManager.Decode(buffer);

//                Console.Write("\nRead: ");
//                foreach (var b in buffer)
//                {
//                    Console.Write(b + " ");
//                }
//                Console.WriteLine();
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
