using System;

namespace P2PCommunicationLibrary.Net
{
    class ClientInfo
    {
        public IClient Client { get; private set; }
        public DateTime LastPingMesssageDateTime { get; set; }
        public Token Token { get; private set; }
        private DateTime _connectionDateTime;
        private PeerAddress _peerAddress;
        private ClientType _clientType;        

        public ClientInfo(IClient client)
        {
            Client = client;
            Token = Token.GenerateNew();
        }

        public DateTime ConnectionDateTime()
        {
            return _connectionDateTime;
        }

        public PeerAddress PeerAddress()
        {
            return _peerAddress;
        }

        public ClientType ClientType()
        {
            return _clientType;
        }

        public ClientInfo ConnectionDateTime(DateTime connDateTime)
        {
            _connectionDateTime = connDateTime;
            return this;
        }

        public ClientInfo PeerAddress(PeerAddress peerAddress)
        {
            _peerAddress = peerAddress;
            return this;
        }

        public ClientInfo ClientType(ClientType clientType)
        {
            _clientType = clientType;
            return this;
        }        
    }
}
