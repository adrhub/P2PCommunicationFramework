using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PCommunicationLibrary.Messages
{
    class MessageManager
    {
        private IEncryptor _encryptor;

        private BinaryEncodingFactory _encodingFactory = new BinaryEncodingFactory();
        private BinaryDecodingFactory _decodingFactory = new BinaryDecodingFactory();

        public MessageManager()
        {
            _encryptor = null;
        }

        public MessageManager(IEncryptor encryptor)
        {
            _encryptor = encryptor;
        }

        public BinaryMessageBase Decode(byte[] buffer)
        {
            if (_encryptor != null)
                buffer = _encryptor.GetDecryption(buffer);

            BinaryMessageBase message = _decodingFactory.GetDecoding(buffer);

            return message;
        }

        public byte[] Encode(BinaryMessageBase message)
        {
            byte[] buffer = _encodingFactory.GetEncoding(message);

            if (_encryptor != null)
                return _encryptor.GetEncryption(buffer);
            else
                return buffer;
        }
    }
}
