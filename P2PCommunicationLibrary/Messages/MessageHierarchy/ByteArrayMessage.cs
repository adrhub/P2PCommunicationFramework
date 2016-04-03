using System;
using System.IO;
using System.Net.Mime;

namespace P2PCommunicationLibrary.Messages
{
    class ByteArrayMessage : BinaryMessageBase
    {
        public override MessageType TypeOfMessage { get; protected set; }
        public byte[] ByteArray { get; private set; }    

        private ByteArrayMessage()
        {
            TypeOfMessage = MessageType.BinaryArrayMessage;
        }

        public ByteArrayMessage(byte[] byteArray, int index, int length)
            : this()
        {
            ByteArray = new byte[length];
            Array.Copy(byteArray, index, ByteArray, 0, length);
        }

        public ByteArrayMessage(byte[] encoding)
            : this()
        {
            try
            {
                MemoryStream input = new MemoryStream(encoding, 0, encoding.Length, false);
                BinaryReader binaryReader = new BinaryReader(new BufferedStream(input));

                MessagesEncodingUtil.ReadByte(binaryReader);
                int binaryArrayLength = MessagesEncodingUtil.ReadInt(binaryReader);
                ByteArray = MessagesEncodingUtil.ReadByteArray(binaryReader, binaryArrayLength);
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
                MessagesEncodingUtil.WriteInt(binaryWriter, ByteArray.Length);
                MessagesEncodingUtil.WriteByteArray(binaryWriter, ByteArray);

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
