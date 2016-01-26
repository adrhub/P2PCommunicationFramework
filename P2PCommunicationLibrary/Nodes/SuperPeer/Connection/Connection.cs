using P2PCommunicationLibrary.Net;

namespace P2PCommunicationLibrary.SuperPeer
{
    abstract class Connection
    {
        public SuperPeerServer Server { get; set; }
        public SuperPeerClient Client { get; set; }

        public Connection(SuperPeerServer server, SuperPeerClient client)
        {
            Server = server;
            Client = client;
        }

        public abstract void ProcessConnection();

        public virtual void Close(IClient closedClient)
        {
//            if (_server.GetSuperPeerClient() == closedClient)
//            {
//                closedClient.Close();
//            }
//            else if (_client.GetSuperPeerClient() == closedClient)
//            {
//                foreach (var client in _server.GetConnectedClients())
//                {
//                    client.GetSuperPeerClient().Close();
//                }
//            }
        }
    }
}
