using System.Net;
using System.Net.Sockets;
using P2PCommunicationLibrary.Messages;

namespace P2PCommunicationLibrary.Net
{    
    interface IClient : IPeer
    {
        event PeerClosedConnectionEventHandler ConnectionClosedEvent;
        event MessageReceivedEventHandler MessageReceivedEvent;

        IPEndPoint LocalEndPoint { get; }
        IPEndPoint RemoteEndPoint { get; }

        void Send(BinaryMessageBase message);
        BinaryMessageBase Read();

        Socket ClientSocket { get; }

        void ListenMessages();
        void ListenOneMessage();
        bool IsListeningMessages { get; }
        void StopListeningMessages();     
    }
}
