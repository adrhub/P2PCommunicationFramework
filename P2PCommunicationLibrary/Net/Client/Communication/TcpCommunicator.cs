using System;
using System.CodeDom;
using System.IO;
using System.Net;
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
            byte[] lengthBuffer = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(messageLength));
            //1862270976
          
            Console.Write("Read: " + "num" + " message : ");
            PrintBuffer(lengthBuffer);

            byte[] readBuffer = binaryReader.ReadBytes(messageLength);
            
            PrintBuffer(readBuffer);
            Console.WriteLine();

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

            Console.Write("Send: " + buffer[0] + " message : ");
            PrintBuffer(sendBuffer);
            Console.WriteLine();
            _communicationSocket.Send(sendBuffer, 0, sendBuffer.Length, SocketFlags.None);

            binaryWriter.Close();
            outputStream.Close();
        }

        private void PrintBuffer(byte[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
                Console.Write(buffer[i] + " ");
        }
    }
}
