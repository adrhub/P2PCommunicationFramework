using System.IO;
using System.Net;
using System.Text;

namespace P2PCommunicationLibrary.Messages
{
    abstract class BinaryEncodableBase
    {
        protected static string ReadString(BinaryReader binaryReader)
        {
            int stringLength = ReadInt(binaryReader);

            Encoding encoding = Encoding.GetEncoding(EncodingConstants.DEFAULT_CHAR_ENC);
            byte[] textBuffer = new byte[stringLength];
            binaryReader.Read(textBuffer, 0, stringLength); 

            string text = encoding.GetString(textBuffer);
            return text;
        }

        protected static byte ReadByte(BinaryReader binaryReader)
        {
            return binaryReader.ReadByte();
        }

        protected static int ReadInt(BinaryReader binaryReader)
        {
            return IPAddress.NetworkToHostOrder(binaryReader.ReadInt32());
        }

        protected static long ReadLong(BinaryReader binaryReader)
        {
            return IPAddress.NetworkToHostOrder(binaryReader.ReadInt64());
        }

        protected static bool ReadBool(BinaryReader binaryReader)
        {
            return binaryReader.ReadBoolean();
        }

        protected static void WriteString(BinaryWriter binaryWriter, string value)
        {
            Encoding encoding = Encoding.GetEncoding(EncodingConstants.DEFAULT_CHAR_ENC);
            byte[] textBuffer = encoding.GetBytes(value);
            WriteInt(binaryWriter, textBuffer.Length);
            binaryWriter.Write(textBuffer);
        }

        protected static void WriteMessageType(BinaryWriter binaryWriter, BinaryMessageBase message)
        {
            binaryWriter.Write((byte)message.TypeOfMessage);
        }

        protected static void WriteByte(BinaryWriter binaryWriter, byte value)
        {
            binaryWriter.Write(value);
        }

        protected static void WriteInt(BinaryWriter binaryWriter, int value)
        {
            binaryWriter.Write(IPAddress.HostToNetworkOrder(value));
        }

        protected static void WriteLong(BinaryWriter binaryWriter, long value)
        {
            binaryWriter.Write(IPAddress.HostToNetworkOrder(value));
        }

        protected static void WriteBool(BinaryWriter binaryWriter, bool value)
        {
            binaryWriter.Write(value);
        }      
    }
}
