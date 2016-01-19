using System.Net;

namespace P2PCommunicationLibrary.Net
{
    interface IServer : IPeer
    {
        event ClientConnectedEventHandler NewClientEvent;

        IPAddress Address { get; }
        int Port { get; }       

        bool IsListening { get; }
        void Listen();
        void StopListening(); 
    }
}
