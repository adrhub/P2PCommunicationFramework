using System;
using System.Collections.Generic;
using P2PCommunicationLibrary.Messages;

namespace P2PCommunicationLibrary.SuperPeer
{
    class ClientMessageManager
    {
        private readonly IClient _client;

        public ClientMessageManager(IClient client)
        {
            _client = client;            
        }

        public void BeginProcessClientMessages()
        {
            _client.MessageReceivedEvent += ClientOnMessageReceivedEvent;
            _client.Listen();
        }

        /// <summary>
        /// Process the client messages
        /// </summary>
        private void ClientOnMessageReceivedEvent(IClient sender, MessageEventArgs messageArgs)
        {
            var message = messageArgs.Message;

            if (message == null)
                return;

            switch (message.TypeOfMessage)
            {
                case MessageType.Request:
                    switch (((RequestMessage)message).RequestedMessageType)
                    {
                    }
                    break;

                case MessageType.ConnectAsServer:
                    InitConnectionAsServer(((PeerAddressMessage)messageArgs.Message).PeerAddress);
                    break;

                case MessageType.ConnectAsClient:
                    InitConnectionAsClient(((PeerAddressMessage)messageArgs.Message).PeerAddress);
                    break;
            }
        }

        #region ProcessClientMessages

        #region InitConnection
        private void InitConnectionAsServer(PeerAddress peerAddress)
        {
            List<IClient> onlineClients = new List<IClient>(ConnectionsRepository.GetClients());
            IClient targetClient = null;

            foreach (IClient client in onlineClients)
            {
                if (ClientRepository.GetClientInfo(client).PeerAddress().Equals(peerAddress))
                    targetClient = client;
            }

            if (IsBothClientsConnected(_client, targetClient))
            {
                ConnectionPair connectionPair = CreateConnectionPair(_client, targetClient);

                if (!ConnectionsRepository.GetConnections().Contains(connectionPair))
                {
                    ConnectionsRepository.AddConnection(connectionPair);
                    ProcessConnectionBetweenClients(connectionPair);
                }
            }
        }

        private void InitConnectionAsClient(PeerAddress peerAddress)
        {
            List<IClient> onlineServers = new List<IClient>(ConnectionsRepository.GetServers());
            IClient targetServer = null;

            foreach (IClient server in onlineServers)
            {
                if (ClientRepository.GetClientInfo(server).PeerAddress().Equals(peerAddress))
                    targetServer = server;
            }

            if (IsBothClientsConnected(targetServer, _client))
            {
                ConnectionPair connectionPair = CreateConnectionPair(targetServer, _client);

                if (!ConnectionsRepository.GetConnections().Contains(connectionPair))
                {
                    ConnectionsRepository.AddConnection(connectionPair);
                    ProcessConnectionBetweenClients(connectionPair);
                }
            }
        }


        private bool IsBothClientsConnected(IClient server, IClient client)
        {
            return server != null && client != null;
        }

        private ConnectionPair CreateConnectionPair(IClient server, IClient client)
        {
            ConnectionPair connectionPair = new ConnectionPair(server, client);

            if (!ConnectionsRepository.GetConnections().Contains(connectionPair))
            {
                ConnectionsRepository.AddConnection(connectionPair);
                return connectionPair;
            }

            return null;
        }
        #endregion

        #endregion

        private void ProcessConnectionBetweenClients(ConnectionPair connectionPair)
        {
            Console.WriteLine("Hello from process connection");
        }
    }
}
