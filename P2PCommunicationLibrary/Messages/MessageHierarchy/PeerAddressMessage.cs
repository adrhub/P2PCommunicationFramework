using System;
using System.Globalization;
using System.IO;
using System.Net;

namespace P2PCommunicationLibrary.Messages
{
    sealed class PeerAddressMessage : BinaryMessageBase
    {
        public override MessageType TypeOfMessage { get; protected set; }
        public PeerAddress PeerAddress { get; set; }

        private PeerAddressMessage()
        {
            TypeOfMessage = MessageType.PeerAddress;
        }

        public PeerAddressMessage(PeerAddress peerAddress, MessageType messageType = MessageType.PeerAddress)
        {
            PeerAddress = peerAddress;
            TypeOfMessage = messageType;
        }

        public PeerAddressMessage(byte[] encoding)
            : this()
        {
            try
            {
                MemoryStream memoryStream = new MemoryStream(encoding, 0, encoding.Length, false);
                BinaryReader binaryReader = new BinaryReader(new BufferedStream(memoryStream));

                TypeOfMessage = (MessageType)MessagesEncodingUtil.ReadByte(binaryReader);

                IPEndPoint localEndPoint = ParseIpEndPoint(MessagesEncodingUtil.ReadString(binaryReader));
                IPEndPoint publicEndPoint = ParseIpEndPoint(MessagesEncodingUtil.ReadString(binaryReader));

                PeerAddress = new PeerAddress(localEndPoint, publicEndPoint);
            }
            catch (Exception)
            {
                throw new BinaryEncodingException("Encode");
            }
        }

        public override byte[] GetEncoding()
        {
            try
            {
                MemoryStream outputStream = new MemoryStream();
                BinaryWriter binaryWriter = new BinaryWriter(new BufferedStream(outputStream));

                MessagesEncodingUtil.WriteMessageType(binaryWriter, this);

                if (PeerAddress.PrivateEndPoint == null)
                    MessagesEncodingUtil.WriteString(binaryWriter, "null");
                else
                    MessagesEncodingUtil.WriteString(binaryWriter, PeerAddress.PrivateEndPoint.ToString());

                if (PeerAddress.PublicEndPoint == null)
                    MessagesEncodingUtil.WriteString(binaryWriter, "null");
                else
                    MessagesEncodingUtil.WriteString(binaryWriter, PeerAddress.PublicEndPoint.ToString());

                binaryWriter.Flush();

                byte[] buffer = outputStream.ToArray();

                if (buffer.Length <= EncodingConstants.MAX_MESSAGE_LENGTH)
                    return outputStream.ToArray();
                else
                    throw new BinaryEncodingException();
            }
            catch (Exception)
            {
                throw new BinaryEncodingException("Decode");
            }
        }

        private static IPEndPoint ParseIpEndPoint(string endPoint)
        {
            if (endPoint == "null")
                return null;

            string[] ep = endPoint.Split(':');

            if (ep.Length < 2)
                throw new FormatException("Invalid endpoint format");

            IPAddress ip;

            if (ep.Length > 2)
            {
                if (!IPAddress.TryParse(string.Join(":", ep, 0, ep.Length - 1), out ip))
                {
                    throw new FormatException("Invalid ip-adress");
                }
            }
            else
            {
                if (!IPAddress.TryParse(ep[0], out ip))
                {
                    throw new FormatException("Invalid ip-adress");
                }
            }

            int port;

            if (!int.TryParse(ep[ep.Length - 1], NumberStyles.None, NumberFormatInfo.CurrentInfo, out port))
            {
                throw new FormatException("Invalid port");
            }

            return new IPEndPoint(ip, port);
        }
    }
}