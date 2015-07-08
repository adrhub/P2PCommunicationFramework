using System.Net;
using System.Net.Sockets;
using P2PCommunicationLibrary.Messages;

namespace P2PCommunicationLibrary
{    
    interface IClient
    {        
        event MessageReceivedEventHandler MessageReceivedEvent;

        IPEndPoint LocalEndPoint { get; }
        IPEndPoint RemoteEndPoint { get; }
        

        void Send(BinaryMessageBase message);
        BinaryMessageBase Read();

        Socket ClientSocket { get; }
        void Listen();
        bool IsListening { get; }
        void StopListening();
        void Close();
    }
}
