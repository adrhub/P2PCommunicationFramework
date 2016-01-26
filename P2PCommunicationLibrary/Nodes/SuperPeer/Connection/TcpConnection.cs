using P2PCommunicationLibrary.Messages;
using P2PCommunicationLibrary.Net;

namespace P2PCommunicationLibrary.SuperPeer
{
    class TcpConnection : Connection
    {
        public TcpConnection(SuperPeerServer server, SuperPeerClient client)
            :base(server, client)
        {            
        }

        public override void ProcessConnection()
        {
            IClient server = Server.GetSuperPeerClient();
            IClient client = Client.GetSuperPeerClient();

            server.Send(new RequestMessage(MessageType.TcpConnection));
            server.Read(); //read confirmation message, check is server is ready

            client.Send(new RequestMessage(MessageType.TcpConnection));
            client.Read();
        }         
    }
}
