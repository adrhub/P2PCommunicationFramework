using System.Net;
using System.Net.Sockets;
using P2PCommunicationLibrary.Messages;

namespace P2PCommunicationLibrary
{
    class ClientUDP : ClientBase
    {
        public ClientUDP(Socket clientSocket, IPEndPoint connectionIpEndPoint, MessageManager messageManager)
            : base(connectionIpEndPoint, messageManager)
        {
            ClientSocket = clientSocket;
            LocalEndPoint = (IPEndPoint)ClientSocket.LocalEndPoint;
            RemoteEndPoint = (IPEndPoint)ClientSocket.RemoteEndPoint;
        }

        public ClientUDP(IPEndPoint connectionIpEndPoint, MessageManager messageManager)
            : base(connectionIpEndPoint, messageManager)
        {
            ClientSocket = InitUdpSocketConnection();
            LocalEndPoint = (IPEndPoint)ClientSocket.LocalEndPoint;
            RemoteEndPoint = (IPEndPoint)ClientSocket.RemoteEndPoint;
        }

        private Socket InitUdpSocketConnection()
        {
            throw new System.NotImplementedException();
        }

        public override void Send(BinaryMessageBase message)
        {
            throw new System.NotImplementedException();
        }

        public override BinaryMessageBase Read()
        {
            throw new System.NotImplementedException();
        }
    }
}
