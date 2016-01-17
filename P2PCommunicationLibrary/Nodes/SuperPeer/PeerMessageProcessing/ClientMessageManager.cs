using System;
using System.Collections.Generic;
using P2PCommunicationLibrary.Messages;

namespace P2PCommunicationLibrary.SuperPeer
{
    class ClientMessageManager
    {
        private readonly SuperPeerClient _superPeerClient;

        public ClientMessageManager(SuperPeerClient superPeerClient)
        {
            _superPeerClient = superPeerClient;
        }

        public void ClientPeerOnMessageReceivedEvent(IClient sender, MessageEventArgs messageArgs)
        {
            var message = messageArgs.Message;

            switch (message.TypeOfMessage)
            {
                case MessageType.ConnectAsClient:
                    InitConnectionAsClient(((PeerAddressMessage)messageArgs.Message).PeerAddress);
                    break;
            }
        }

        private void InitConnectionAsClient(PeerAddress peerAddress)
        {
            List<SuperPeerServer> onlineServers = new List<SuperPeerServer>(ConnectionsRepository.GetServers());
            SuperPeerServer targetServer = null;

            foreach (SuperPeerServer server in onlineServers)
            {
                if (server.GetClientInfo().PeerAddress().Equals(peerAddress))
                    targetServer = server;
            }

            if (targetServer == null)
                return;

            if (!targetServer.GetAllowedConnections().Contains(_superPeerClient.GetClientInfo().PeerAddress()))
                return;

            ConnectionPair connectionPair = CreateConnectionPair(targetServer, _superPeerClient);                        
            ProcessConnectionBetweenClients(connectionPair);
        }
       
        private ConnectionPair CreateConnectionPair(SuperPeerServer server, SuperPeerClient client)
        {
            ConnectionPair connectionPair = new ConnectionPair(server, client);

            if (!ConnectionsRepository.GetConnections().Contains(connectionPair))
            {
                ConnectionsRepository.AddConnection(connectionPair);
                return connectionPair;
            }

            return null;
        }

        private void ProcessConnectionBetweenClients(ConnectionPair connectionPair)
        {
            Console.WriteLine("Hello from process connection");
        }
    }
}
