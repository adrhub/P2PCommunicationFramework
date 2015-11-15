namespace P2PCommunicationLibrary.Messages
{

    public interface IEncryptor
    {
        byte[] GetEncryption(byte[] buffer);
        byte[] GetDecryption(byte[] buffer);
    }

}
