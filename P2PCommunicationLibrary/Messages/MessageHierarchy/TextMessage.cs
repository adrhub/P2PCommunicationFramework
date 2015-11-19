using System;
using System.IO;

namespace P2PCommunicationLibrary.Messages
{
    sealed class TextMessage : BinaryMessageBase
    {
        public override MessageType TypeOfMessage { get; protected set; }
        public string Text { get; private set; }

        private TextMessage()
        {
            TypeOfMessage = MessageType.TextMessage;
        }

        public TextMessage(string text)
            : this()
        {
            Text = text;
        }

        public TextMessage(byte[] encoding)
            : this()
        {
            try
            {
                MemoryStream input = new MemoryStream(encoding, 0, encoding.Length, false);
                BinaryReader binaryReader = new BinaryReader(new BufferedStream(input));

                MessagesEncodingUtil.ReadByte(binaryReader);
                Text = MessagesEncodingUtil.ReadString(binaryReader);            
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
                MessagesEncodingUtil.WriteString(binaryWriter, Text);

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
