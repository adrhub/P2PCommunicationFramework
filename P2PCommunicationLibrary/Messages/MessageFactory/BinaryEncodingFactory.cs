namespace P2PCommunicationLibrary.Messages
{
    class BinaryEncodingFactory:IEncodingFactory
    {
        public byte[] GetEncoding(BinaryMessageBase message)
        {
            return message.GetEncoding();
        }
    }
}
