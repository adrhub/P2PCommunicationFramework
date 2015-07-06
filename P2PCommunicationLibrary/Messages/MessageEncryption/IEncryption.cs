namespace P2PCommunicationLibrary.Messages
{

    public interface IEncrtyptor
    {
        byte[] GetEncryption(byte[] buffer);
        byte[] GetDecryption(byte[] buffer);
    }

}
