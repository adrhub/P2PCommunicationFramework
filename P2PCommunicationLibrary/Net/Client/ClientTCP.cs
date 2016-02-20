using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using P2PCommunicationLibrary.Messages;

namespace P2PCommunicationLibrary.Net
{
    class ClientTcp : ClientBase
    {        
        private ICommunicator _communicator;

        public ClientTcp(Socket clientSocket, MessageManager messageManager)
            : base(messageManager)
        {
            ClientSocket = clientSocket;         
            InitProperties();
        }

        public ClientTcp(IPEndPoint connectionIpEndPoint, MessageManager messageManager)
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
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            lock (SendMonitor)
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
                catch (IOException)
                {
                    Close();
                }
                catch (BinaryEncodingException)
                {
                    Console.WriteLine("BinaryEncodingException: Encode");
                }
            }
        }

        public override BinaryMessageBase Read()
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            byte[] buffer = new byte[EncodingConstants.MAX_MESSAGE_LENGTH];
            BinaryMessageBase receivedMessage = null;

            lock (ReadMonitor)
            {
                try
                {
                    buffer = _communicator.Read();
                    receivedMessage = MessageManager.Decode(buffer);
                }
                catch (SocketException)
                {
                    Close();
                }
                catch (IOException)
                {
                    Close();
                }
                catch (BinaryEncodingException)
                {
                    Console.WriteLine("BinaryEncodingException: Decode");
                }                
            }

            return receivedMessage;
        }
    }
}
