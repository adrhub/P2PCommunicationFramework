using System;
using System.Collections.Generic;
using System.Linq;
using P2PCommunicationLibrary.Messages;

namespace P2PCommunicationLibrary.SuperPeer
{
    class ClientConnectionManager
    {
        private IClient _client;

        public ClientConnectionManager(IClient client)
        {
            _client = client;
        }

        public void ProcessClientConnection()
        {
            var clientConnected = InitClientConnection();

            if (clientConnected)
            {
                Console.WriteLine("Client " + _client.RemoteEndPoint + " " + _client.LocalEndPoint + " Connected");

                ClientType clientType = InitPeerType();

                if (clientType != ClientType.None)                                
                    AddClientToRepositoryAndInitClientInfo(clientType);

                _client.Send(new ConfirmationMessage(MessageType.Connection));
                _client.MessageReceivedEvent += ClientOnMessageReceivedEvent;                                                                
                _client.Listen();                
            }
        }        

        private bool InitClientConnection()
        {
            bool clientConnected = true;

            var message = _client.Read();

            if (message == null)
            {
                _client.Close();
                clientConnected = false;
            }
            else if (!(message is ConnectionMessage))
            {
                _client.Close();
                clientConnected = false;
            }

            return clientConnected;
        }

        private ClientType InitPeerType()
        {                      
            var message = _client.Read();            

            if (!(message is RequestMessage))
                _client.Close();
            else
            {
                RequestMessage requestMessage = (RequestMessage) message;

                if (requestMessage.RequestedMessageType == MessageType.InitConnectionAsClient)
                {
                    ConnectionsRepository.AddClient(_client);
                    return ClientType.Client;                    
                }
                else if (requestMessage.RequestedMessageType == MessageType.InitConnectionAsServer)
                {
                    ConnectionsRepository.AddServer(_client);
                    return ClientType.Server;
                }
                else
                    _client.Close();
            }
            
            return ClientType.None;            
        }

        private void AddClientToRepositoryAndInitClientInfo(ClientType clientType)
        {            
            var requestMessage = _client.Read() as RequestMessage;

            if (requestMessage != null && requestMessage.RequestedMessageType == MessageType.ClientPeerAddress)
            {                
                PeerAddress peerAddress = new PeerAddress {PublicEndPoint = _client.RemoteEndPoint};

                _client.Send(new PeerAddressMessage(peerAddress, MessageType.ClientPeerAddress));
                peerAddress.PrivateEndPoint = ((PeerAddressMessage)_client.Read()).PeerAddress.PrivateEndPoint;

                ClientInfo info = ClientRepository.AddClient(_client);
                info.ClientType(clientType).ConnectionDateTime(DateTime.Now).PeerAddress(peerAddress);
            }
            else
                _client.Close();
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
                    ProcessConnection(connectionPair);
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
                    ProcessConnection(connectionPair);
                }
            }
        }
        

        private bool IsBothClientsConnected(IClient server, IClient client)
        {
            return server != null && client != null;
        }

        private ConnectionPair CreateConnectionPair(IClient server, IClient client)
        {           
            ConnectionPair connectionPair =  new ConnectionPair(server, client);

            if (!ConnectionsRepository.GetConnections().Contains(connectionPair))
            {
                ConnectionsRepository.AddConnection(connectionPair);
                return connectionPair;
            }            

            return null;
        }        
        #endregion            

        #endregion

        private void ProcessConnection(ConnectionPair connectionPair)
        {
            Console.WriteLine("Hello from process connection");
        }
    }
}
