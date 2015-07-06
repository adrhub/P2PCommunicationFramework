namespace P2PCommunicationLibrary.Messages
{
    interface IDecodingFactory
    {
        BinaryMessageBase GetDecoding(byte[] encoding);
    }
}
