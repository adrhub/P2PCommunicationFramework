namespace P2PCommunicationLibrary.Messages
{
    abstract class BinaryMessageBase
    {        
        public abstract MessageType TypeOfMessage { get; protected set; }
        public abstract byte[] GetEncoding();
    }
}
