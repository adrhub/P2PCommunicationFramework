namespace P2PCommunicationLibrary.Messages
{
    interface IEncodingFactory
    {
        byte[] GetEncoding(BinaryMessageBase message);
    }
}
