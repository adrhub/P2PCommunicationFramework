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

            //say to server to run listener
            server.Send(new RequestMessage(MessageType.TcpConnection)); // 2
            //read confirmation message, check if server is ready; read port
            var serverPrivateIpEndPoint = (PeerAddressMessage)server.Read(); // 111

            //say to client that he can connect to the server; send port
            client.Send(serverPrivateIpEndPoint);
            //read confirmation message, check if client connected to the server
            client.Read();
        }         
    }
}
