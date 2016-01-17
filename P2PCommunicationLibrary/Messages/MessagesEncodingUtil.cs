using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace P2PCommunicationLibrary.Messages
{
    static class MessagesEncodingUtil
    {
        public static string ReadString(BinaryReader binaryReader)
        {
            int stringLength = ReadInt(binaryReader);

            Encoding encoding = Encoding.GetEncoding(EncodingConstants.DEFAULT_CHAR_ENC);
            byte[] textBuffer = new byte[stringLength];
            binaryReader.Read(textBuffer, 0, stringLength);

            string text = encoding.GetString(textBuffer);
            return text;
        }

        public static byte ReadByte(BinaryReader binaryReader)
        {
            return binaryReader.ReadByte();
        }

        public static int ReadInt(BinaryReader binaryReader)
        {
            return IPAddress.NetworkToHostOrder(binaryReader.ReadInt32());
        }

        public static long ReadLong(BinaryReader binaryReader)
        {
            return IPAddress.NetworkToHostOrder(binaryReader.ReadInt64());
        }

        public static bool ReadBool(BinaryReader binaryReader)
        {
            return binaryReader.ReadBoolean();
        }

        public static void WriteString(BinaryWriter binaryWriter, string value)
        {
            Encoding encoding = Encoding.GetEncoding(EncodingConstants.DEFAULT_CHAR_ENC);
            byte[] textBuffer = encoding.GetBytes(value);
            WriteInt(binaryWriter, textBuffer.Length);
            binaryWriter.Write(textBuffer);
        }

        public static void WriteMessageType(BinaryWriter binaryWriter, BinaryMessageBase message)
        {
            binaryWriter.Write((byte)message.TypeOfMessage);
        }

        public static void WriteByte(BinaryWriter binaryWriter, byte value)
        {
            binaryWriter.Write(value);
        }

        public static void WriteInt(BinaryWriter binaryWriter, int value)
        {
            binaryWriter.Write(IPAddress.HostToNetworkOrder(value));
        }

        public static void WriteLong(BinaryWriter binaryWriter, long value)
        {
            binaryWriter.Write(IPAddress.HostToNetworkOrder(value));
        }

        public static void WriteBool(BinaryWriter binaryWriter, bool value)
        {
            binaryWriter.Write(value);
        }
    }
}
