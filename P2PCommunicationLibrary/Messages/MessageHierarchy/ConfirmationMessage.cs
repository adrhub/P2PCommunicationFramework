using System;
using System.IO;

namespace P2PCommunicationLibrary.Messages
{
    sealed class ConfirmationMessage : BinaryMessageBase
    {
        public override MessageType TypeOfMessage { get; protected set; }
        public MessageType ConfirmationMessageType { get; private set; }

        private ConfirmationMessage()
        {
            TypeOfMessage = MessageType.Confirmation;
        }

        public ConfirmationMessage(MessageType confirmationMessageType)
            : this()
        {
            ConfirmationMessageType = confirmationMessageType;
        }

        public ConfirmationMessage(byte[] encoding)
            : this()
        {
            try
            {
                MemoryStream input = new MemoryStream(encoding, 0, encoding.Length, false);
                BinaryReader binaryReader = new BinaryReader(new BufferedStream(input));

                MessagesEncodingUtil.ReadByte(binaryReader);
                ConfirmationMessageType = (MessageType)MessagesEncodingUtil.ReadByte(binaryReader);
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
                MessagesEncodingUtil.WriteByte(binaryWriter, (byte) ConfirmationMessageType);

                binaryWriter.Flush();

                byte[] buffer = outputStream.ToArray();

                if (buffer.Length < EncodingConstants.MAX_MESSAGE_LENGTH)
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
