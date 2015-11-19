using System;
using System.IO;

namespace P2PCommunicationLibrary.Messages
{
    sealed class RequestMessage : BinaryMessageBase
    {
        public override MessageType TypeOfMessage { get; protected set; }
        public MessageType RequestedMessageType { get; private set; }

        private RequestMessage()
        {
            TypeOfMessage = MessageType.Request;
        }

        public RequestMessage(MessageType requestedMessageType)
            : this()
        {
            RequestedMessageType = requestedMessageType;
        }

        public RequestMessage(byte[] encoding)
            : this()
        {
            try
            {
                MemoryStream input = new MemoryStream(encoding, 0, encoding.Length, false);
                BinaryReader binaryReader = new BinaryReader(new BufferedStream(input));

                MessagesEncodingUtil.ReadByte(binaryReader);
                RequestedMessageType = (MessageType)MessagesEncodingUtil.ReadByte(binaryReader);
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
                MessagesEncodingUtil.WriteByte(binaryWriter, (byte) RequestedMessageType);

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
    }
}
