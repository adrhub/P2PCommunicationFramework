using System.IO;
using System.Net.Sockets;
using P2PCommunicationLibrary.Messages;

namespace P2PCommunicationLibrary.Net
{
    class TcpCommunicator : ICommunicator
    {
        private readonly Socket _communicationSocket;

        public TcpCommunicator(Socket communicationSocket)
        {
            _communicationSocket = communicationSocket;
        }

        public byte[] Read()
        {
            NetworkStream networkStream = new NetworkStream(_communicationSocket);
            BinaryReader binaryReader = new BinaryReader(networkStream);

            int messageLength = MessagesEncodingUtil.ReadInt(binaryReader);

            byte[] readBuffer = binaryReader.ReadBytes(messageLength);

            binaryReader.Close();
            networkStream.Close();

            return readBuffer;
        }

        public void Write(byte[] buffer)
        {
            MemoryStream outputStream = new MemoryStream();
            BinaryWriter binaryWriter = new BinaryWriter(new BufferedStream(outputStream));

            MessagesEncodingUtil.WriteInt(binaryWriter, buffer.Length);
            binaryWriter.Write(buffer, 0, buffer.Length);

            binaryWriter.Flush();
            byte[] sendBuffer = outputStream.ToArray();

            _communicationSocket.Send(sendBuffer, 0, sendBuffer.Length, SocketFlags.None);

            binaryWriter.Close();
            outputStream.Close();
        }
    }
}
