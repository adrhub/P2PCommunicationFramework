using System;
using System.Collections.Generic;
using System.Linq;
using P2PCommunicationLibrary.Messages;

namespace P2PCommunicationLibrary.SuperPeer
{
    class ClientConnection
    {
        private IClient _client;

        public ClientConnection(IClient client)
        {
            _client = client;
        }

        public void ProcessClientConnection()
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

            if (clientConnected)
            {
                Console.WriteLine("Client " + _client.RemoteEndPoint + " " + _client.LocalEndPoint + " Connected");
                _client.MessageReceivedEvent += ClientOnMessageReceivedEvent;
                
                
                ClientRepository.AddClient(_client);
                _client.Send(new ConfirmationMessage(MessageType.Connection));
                _client.Listen();                
            }            
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
                        case MessageType.ClientPeerAddress:
                            SendClientPeerAddress();
                            break;                        
                    }
                    break;

                case MessageType.ConnectAsServer:
                    ConnectionsRepository.AddServer(_client);
                    InitConnectionAsServer(((PeerAddressMessage)messageArgs.Message).PeerAddress);
                    break;

                case MessageType.ConnectAsClient:
                    ConnectionsRepository.AddClient(_client);
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
                if (IsEqualPeerAddressAndIClientAddress(peerAddress, client))
                    targetClient = client;
            }

            if (IsBothClientsConnected(_client, targetClient))
            {
                ConnectionPair connectionPair = CreateConnectionPair(_client, targetClient);
                ProcessConnection(connectionPair);
            }
        }

        private void InitConnectionAsClient(PeerAddress peerAddress)
        {
            List<IClient> onlineServers = new List<IClient>(ConnectionsRepository.GetServers());
            IClient targetServer = null;

            foreach (IClient server in onlineServers)
            {
                if (IsEqualPeerAddressAndIClientAddress(peerAddress, server))
                    targetServer = server;
            }

            if (IsBothClientsConnected(targetServer, _client))
            {
                ConnectionPair connectionPair = CreateConnectionPair(targetServer, _client);
                ProcessConnection(connectionPair);
            }
        }

        private static bool IsEqualPeerAddressAndIClientAddress(PeerAddress peerAddress, IClient client)
        {
            return client.LocalEndPoint.ToString() == peerAddress.PublicEndPoint.ToString()
                   && client.RemoteEndPoint.ToString() == peerAddress.PrivateEndPoint.ToString();
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
        
        private void SendClientPeerAddress()
        {
            PeerAddress clientPublicAddress = new PeerAddress();
            clientPublicAddress.PublicEndPoint = _client.LocalEndPoint;

            var message = new PeerAddressMessage(clientPublicAddress, MessageType.ClientPeerAddress);
            _client.Send(message);
        }

        #endregion

        private void ProcessConnection(ConnectionPair connectionPair)
        {

        }
    }
}
