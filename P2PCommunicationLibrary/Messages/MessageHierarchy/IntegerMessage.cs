using System;
using System.IO;
using System.Net.Mime;

namespace P2PCommunicationLibrary.Messages
{
    class IntegerMessage : BinaryMessageBase
    {
        public override MessageType TypeOfMessage { get; protected set; }
        public int Integer { get; private set; }

        private IntegerMessage()
        {
            TypeOfMessage = MessageType.IntegerMessage;
        }

        public IntegerMessage(int integer, MessageType messageType = MessageType.PeerAddress)
            : this()
        {
            Integer = integer;
            TypeOfMessage = messageType;
        }

        public IntegerMessage(byte[] encoding)
            : this()
        {
            try
            {
                MemoryStream input = new MemoryStream(encoding, 0, encoding.Length, false);
                BinaryReader binaryReader = new BinaryReader(new BufferedStream(input));

                MessagesEncodingUtil.ReadByte(binaryReader);
                Integer = MessagesEncodingUtil.ReadInt(binaryReader);
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
                MessagesEncodingUtil.WriteInt(binaryWriter, Integer);

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
