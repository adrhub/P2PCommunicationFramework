using System.Net;

namespace P2PCommunicationLibrary
{
    interface IServer
    {
        event ClientConnectedEventHandler NewClientEvent;
        
        IPAddress Address { get; }
        int Port { get; }
        bool IsListening { get; }

        void Listen();
        void StopListening();
    }
}
