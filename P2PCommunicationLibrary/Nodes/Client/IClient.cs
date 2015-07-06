using System.Net;
using P2PCommunicationLibrary.Messages;

namespace P2PCommunicationLibrary
{    
    interface IClient
    {        
        event MessageReceivedEventHandler MessageReceivedEvent;

        IPEndPoint LocalEndPoint { get; }
        IPEndPoint RemoteEndPoint { get; }
        bool IsListening { get; }

        void Send(BinaryMessageBase message);
        BinaryMessageBase Read();
        void Listen();        
        void StopListening();
        void Close();
    }
}
