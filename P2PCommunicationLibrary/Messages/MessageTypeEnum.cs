namespace P2PCommunicationLibrary.Messages
{
    /// <summary>
    /// The first byte of a message represents the message type.
    /// </summary>

    public enum MessageType : byte
    {
        None = 0,

        Connection = 1,
        Request = 2,
        Confirmation = 3,

        TextMessage = 101,

        PeerAddress = 111,     
        ClientPeerAddress = 112,

        InitConnectionAsClient = 121,
        InitConnectionAsServer = 122,
        ConnectAsClient = 123,
        ConnectAsServer = 124
    }
}