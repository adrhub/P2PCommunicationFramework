using System.Net.Sockets;
using P2PCommunicationLibrary.Messages;

namespace P2PCommunicationLibrary
{
    class TcpCommunicator : ICommunicator
    {
        private Socket _communicationSocket;

        public TcpCommunicator(Socket communicationSocket)
        {
            _communicationSocket = communicationSocket;
        }

        public byte[] Read()
        {
            return null;
        }

        public void Write(byte[] buffer)
        {
            byte[] sendBuffer = new byte[buffer.Length + 1];            
        }
    }
}
