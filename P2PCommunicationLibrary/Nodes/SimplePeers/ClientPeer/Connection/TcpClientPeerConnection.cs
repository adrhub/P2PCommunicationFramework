using System;
using System.Net;
using P2PCommunicationLibrary.Messages;
using P2PCommunicationLibrary.Net;

namespace P2PCommunicationLibrary.SimplePeers.ClientPeer
{
    class TcpClientPeerConnection : ClientPeerConnection
    {
        public TcpClientPeerConnection(ClientPeer clientPeer, PeerAddress clientPeerAddress) 
            : base(clientPeer, clientPeerAddress)
        {            
        }

        public override void ProcessConnection()
        {
            PeerAddressMessage peerAddressMessage = (PeerAddressMessage) ClientPeer.Peer.ReadFromSuperPeer();

            MessageManager messageManager = ClientPeer.Peer.MessageManager;
            IPEndPoint connectionIpEndPoint = peerAddressMessage.PeerAddress.PrivateEndPoint;
            Client = new ClientTcp(connectionIpEndPoint, messageManager);            
        }        
    }
}
