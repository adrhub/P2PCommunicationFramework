namespace P2PCommunicationLibrary.Messages
{
    abstract class BinaryMessageBase : BinaryEncodableBase
    {        
        public abstract MessageType TypeOfMessage { get; protected set; }
        public abstract byte[] GetEncoding();
    }
}
