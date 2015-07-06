using P2PCommunicationLibrary.Messages;

namespace P2PCommunicationLibrary.SuperPeer
{
    class ClientServerConnection
    {
        private IClient _client;

        public ClientServerConnection(IClient client)
        {
            _client = client;            
        }

        public void ProcessClient()
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
                _client.MessageReceivedEvent += ClientOnMessageReceivedEvent;
                _client.Listen();
            }
        }
        /// <summary>
        /// Process the client messages
        /// </summary>
        private void ClientOnMessageReceivedEvent(IClient sender, MessageEventArgs messageArgs)
        {
            var message = messageArgs.Message;

            switch (message.TypeOfMessage)
            {
                case MessageType.PeerAddress:
                    break;
                
                case MessageType.Request:
                    switch (((RequestMessage)message).RequestedMessageType)
                    {
                        case MessageType.ClientPeerAddress:
                            SendClientPeerAddress();
                            break;
                    }
                    break;
            }
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
