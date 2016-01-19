using System;
using P2PCommunicationLibrary.Messages;
using P2PCommunicationLibrary.Net;

namespace P2PCommunicationLibrary.SuperPeer
{
    class PeerConnectionManager
    {
        private readonly IClient _client;

        public PeerConnectionManager(IClient client)
        {
            _client = client;
        }

        public void BeginProcessClientConnection()
        {
            var clientConnected = InitClientConnection();

            if (!clientConnected)
                return;

            Console.WriteLine("Client " + _client.RemoteEndPoint + " " + _client.LocalEndPoint + " Connected");

            ClientType clientType = GetPeerType();

            if (clientType == ClientType.None)
                return;

             AddClientToRepositoryAndInitClientInfo(clientType);

            _client.Send(new ConfirmationMessage(MessageType.Connection));

            SuperPeerNode superPeerNode = GetSuperPeerNodeByClientType(clientType);           
            AddSuperPeerNodeToConnectionsRepository(clientType, superPeerNode);

            PeerMessageManager peerMessageManager = new PeerMessageManager(superPeerNode);
            peerMessageManager.BeginProcessClientMessages();
        }

        private void AddSuperPeerNodeToConnectionsRepository(ClientType clientType, SuperPeerNode superPeerNode)
        {
            if (clientType == ClientType.Client)
                ConnectionsRepository.AddClient((SuperPeerClient) superPeerNode);
            else
                ConnectionsRepository.AddServer((SuperPeerServer) superPeerNode);
        }

        private SuperPeerNode GetSuperPeerNodeByClientType(ClientType clientType)
        {
            SuperPeerNode superPeerNode;

            if (clientType == ClientType.Client)            
                superPeerNode = new SuperPeerClient(_client);            
            else
                superPeerNode = new SuperPeerServer(_client);

            return superPeerNode;
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

        private ClientType GetPeerType()
        {
            var message = _client.Read();

            if (!(message is RequestMessage))
                _client.Close();
            else
            {
                RequestMessage requestMessage = (RequestMessage) message;

                switch (requestMessage.RequestedMessageType)
                {
                    case MessageType.InitConnectionAsClient:
                        return ClientType.Client;
                    case MessageType.InitConnectionAsServer:
                        return ClientType.Server;
                    default:
                        _client.Close();
                        break;
                }
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
                peerAddress.PrivateEndPoint = ((PeerAddressMessage) _client.Read()).PeerAddress.PrivateEndPoint;

                ClientInfo info = ClientRepository.AddClient(_client);
                info.ClientType(clientType).ConnectionDateTime(DateTime.Now).PeerAddress(peerAddress);
            }
            else
                _client.Close();
        }
    }
}
