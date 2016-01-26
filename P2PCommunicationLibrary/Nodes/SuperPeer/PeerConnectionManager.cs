using System;
using P2PCommunicationLibrary.Messages;
using P2PCommunicationLibrary.Net;

namespace P2PCommunicationLibrary.SuperPeer
{
    class PeerConnectionManager
    {
        private readonly IClient _client;
        private ClientInfo _clientInfo;

        public PeerConnectionManager(IClient client)
        {
            _client = client;
            _clientInfo = new ClientInfo(client); 
                                  
            _client.ConnectionClosedEvent += RepositoryCleaner.ClientOnConnectionClosedEvent;

            Console.WriteLine("Client " + _client.RemoteEndPoint + " " + _client.LocalEndPoint + " Connected");
        }

        private void InitClientInfo()
        {           
            _clientInfo.ConnectionDateTime(DateTime.Now);
            _clientInfo.LastPingMesssageDateTime = DateTime.Now;           
        }

        public void BeginProcessClientConnection()
        {       
            if (!InitClientConnection())
                return;                    

            if (!InitPeerType())
                return;

            if (!InitClientAddress())
                return;

            InitClientInfo();

            ClientRepository.AddClient(_client, _clientInfo);
            
            SuperPeerNode superPeerNode = GetSuperPeerNodeByClientType(_clientInfo.ClientType());
            AddSuperPeerNodeToConnectionsRepository(_clientInfo.ClientType(), superPeerNode);

            _client.Send(new ConfirmationMessage(MessageType.Connection));

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

        private bool InitPeerType()
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
                        _clientInfo.ClientType(ClientType.Client);
                        return true;
                    case MessageType.InitConnectionAsServer:
                        _clientInfo.ClientType(ClientType.Server);
                        return true;
                    default:
                        _clientInfo.ClientType(ClientType.None);
                        _client.Close();
                        break;
                }
            }

            return false;
        }

        private bool InitClientAddress()
        {
            var requestMessage = _client.Read() as RequestMessage;

            if (requestMessage != null && requestMessage.RequestedMessageType == MessageType.ClientPeerAddress)
            {
                PeerAddress peerAddress = new PeerAddress {PublicEndPoint = _client.RemoteEndPoint};

                _client.Send(new PeerAddressMessage(peerAddress, MessageType.ClientPeerAddress));
                peerAddress.PrivateEndPoint = ((PeerAddressMessage) _client.Read()).PeerAddress.PrivateEndPoint;

                _clientInfo.PeerAddress(peerAddress);
                return true;
            }
            else
            {
                _client.Close();
                return false;
            }
        }
    }
}
