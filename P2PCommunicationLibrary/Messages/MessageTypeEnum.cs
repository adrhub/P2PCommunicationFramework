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
        Ping = 4,

        TextMessage = 101,
        IntegerMessage = 102,
        BinaryArrayMessage = 103,

        PeerAddress = 111,     
        ClientPeerAddress = 112,

        InitConnectionAsClient = 121,
        InitConnectionAsServer = 122,
        ConnectAsClient = 123,
        ConnectAsServer = 124,

        TcpConnection = 151, 
        TcpConnectionAllowed = 152, 
        
        ConnectionPort = 155,      

        CloseConnection = 250,        
    }
}