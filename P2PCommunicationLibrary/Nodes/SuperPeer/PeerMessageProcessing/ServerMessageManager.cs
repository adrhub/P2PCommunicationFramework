using P2PCommunicationLibrary.Messages;
using P2PCommunicationLibrary.Net;

namespace P2PCommunicationLibrary.SuperPeer
{
    class ServerMessageManager
    {
        private readonly SuperPeerServer _superPeerServer;

        public ServerMessageManager(SuperPeerServer superPeerServer)
        {
            _superPeerServer = superPeerServer;
        }

        public void ServerPeerOnMessageReceivedEvent(IClient sender, MessageEventArgs messageArgs)
        {
            var message = messageArgs.Message;

            switch (message.TypeOfMessage)
            {
                case MessageType.ConnectAsServer:                   
                    InitConnectionAsServer(((PeerAddressMessage) messageArgs.Message).PeerAddress);      
                    _superPeerServer.GetClientInfo().Client.StopListeningMessages();         
                    break;
            }
        }

        private void InitConnectionAsServer(PeerAddress peerAddress)
        {            
            _superPeerServer.AddAddressToAllowedConnections(peerAddress);
        }
    }
}
