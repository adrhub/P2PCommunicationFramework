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
                Console.WriteLine("Client " + _client.ClientSocket.RemoteEndPoint + " Connected");
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

        private void InitConnectionAsServer(PeerAddress peerAddress)
        {
            List<IClient> onlineClients = new List<IClient>(ConnectionsRepository.GetClients());
            IClient targetClient = null;

            foreach (IClient client in onlineClients)
            {
                if (client.LocalEndPoint.ToString() == peerAddress.PublicEndPoint.ToString()
                    && client.RemoteEndPoint.ToString() == peerAddress.PrivateEndPoint.ToString())
                    targetClient = client;
            }

            if (targetClient != null)
            {
                ConnectionPair connectionPair = new ConnectionPair(_client, targetClient);

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
                if (server.LocalEndPoint.ToString() == peerAddress.PublicEndPoint.ToString()
                    && server.RemoteEndPoint.ToString() == peerAddress.PrivateEndPoint.ToString())
                    targetServer = server;
            }

            if (targetServer != null)
            {
                ConnectionPair connectionPair = new ConnectionPair(targetServer, _client);

                if (!ConnectionsRepository.GetConnections().Contains(connectionPair))
                {
                    ConnectionsRepository.AddConnection(connectionPair);
                    ProcessConnection(connectionPair);
                }
            }
        }

        private void ProcessConnection(ConnectionPair connectionPair)
        {

        }

        private void SendClientPeerAddress()
        {
            PeerAddress clientPublicAddress = new PeerAddress();
            clientPublicAddress.PublicEndPoint = _client.LocalEndPoint;

            var message = new PeerAddressMessage(clientPublicAddress, MessageType.ClientPeerAddress);
            _client.Send(message);
        }        
    }
}
