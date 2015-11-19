using System;
using P2PCommunicationLibrary.Messages;

namespace P2PCommunicationLibrary.SuperPeer
{
    internal class ClientConnectionManager
    {
        private readonly IClient _client;

        public ClientConnectionManager(IClient client)
        {
            _client = client;
        }

        public void BeginProcessClientConnection()
        {
            var clientConnected = InitClientConnection();

            if (clientConnected)
            {
                Console.WriteLine("Client " + _client.RemoteEndPoint + " " + _client.LocalEndPoint + " Connected");

                ClientType clientType = InitPeerType();

                if (clientType != ClientType.None)
                    AddClientToRepositoryAndInitClientInfo(clientType);

                _client.Send(new ConfirmationMessage(MessageType.Connection));


                ClientMessageManager clientMessageManager = new ClientMessageManager(_client);
                clientMessageManager.BeginProcessClientMessages();                
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
                peerAddress.PrivateEndPoint = ((PeerAddressMessage) _client.Read()).PeerAddress.PrivateEndPoint;

                ClientInfo info = ClientRepository.AddClient(_client);
                info.ClientType(clientType).ConnectionDateTime(DateTime.Now).PeerAddress(peerAddress);
            }
            else
                _client.Close();
        }
    }
}
