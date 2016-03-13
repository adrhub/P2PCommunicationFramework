using System;
using System.Collections.Generic;
using System.Threading;
using P2PCommunicationLibrary.Messages;
using P2PCommunicationLibrary.Net;

namespace P2PCommunicationLibrary.SuperPeer
{
    class PeerMessageManager
    {        
        private readonly SuperPeerNode _superPeerNode;

        public PeerMessageManager(SuperPeerNode superPeerNode)
        {
            _superPeerNode = superPeerNode;
        }

        public void BeginProcessClientMessages()
        {            
            _superPeerNode.GetSuperPeerClient().MessageReceivedEvent += PeerOnMessageReceivedEvent;
            _superPeerNode.GetSuperPeerClient().ListenMessages();
        }

        /// <summary>
        /// Process the peer messages
        /// </summary>
        private void PeerOnMessageReceivedEvent(IClient sender, MessageEventArgs messageArgs)
        {                        
            var message = messageArgs.Message;

            if (message == null)
                return;
            
            switch (message.TypeOfMessage)
            {               
                case MessageType.Request:
                    switch (((RequestMessage)message).RequestedMessageType)
                    {
                        case MessageType.Ping:
                            _superPeerNode.GetClientInfo().LastPingMesssageDateTime = DateTime.Now;
                            break;
                        case MessageType.CloseConnection:                            
                            _superPeerNode.GetSuperPeerClient().Close();                           
                            break;
                    }
                    break;               

                default:
                    if (_superPeerNode is SuperPeerClient)
                        new ClientMessageManager((SuperPeerClient)_superPeerNode).ClientPeerOnMessageReceivedEvent(sender, messageArgs);
                    else if (_superPeerNode is SuperPeerServer)
                        new ServerMessageManager((SuperPeerServer)_superPeerNode).ServerPeerOnMessageReceivedEvent(sender, messageArgs);
                    break;
            }          
        }
    }
}
