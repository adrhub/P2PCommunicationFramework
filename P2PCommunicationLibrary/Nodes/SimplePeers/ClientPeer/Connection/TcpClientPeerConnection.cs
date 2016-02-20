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
            var serverPrivateIpEndPoint = (PeerAddressMessage) ClientPeer.Peer.ReadFromSuperPeer();

            MessageManager messageManager = new MessageManager (ClientPeer.Encryptor);
            IPEndPoint connectionIpEndPoint = serverPrivateIpEndPoint.PeerAddress.PrivateEndPoint;

            var client = new ClientTcp(connectionIpEndPoint, messageManager);
        }

        public override void Close()
        {
            throw new System.NotImplementedException();
        }
    }
}
