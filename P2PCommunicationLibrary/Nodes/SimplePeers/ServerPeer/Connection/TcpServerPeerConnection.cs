using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using P2PCommunicationLibrary.Messages;
using P2PCommunicationLibrary.Net;

namespace P2PCommunicationLibrary.SimplePeers.ServerPeer
{
    class TcpServerPeerConnection : ServerPeerConnection
    {
        private ServerTcp _server; 

        public TcpServerPeerConnection(ServerPeer serverPeer)
            : base(serverPeer)
        {           
        }

        public override IClient GetConnection()
        {                       
            SetupListener();
            ServerPeer.Peer.SendToSuperPeer(
                new PeerAddressMessage(
                new PeerAddress(
                    new IPEndPoint(ServerPeer.Peer.PeerAddress.PrivateEndPoint.Address, _server.Port),
                    null)));

            Client = _server.AcceptClient();
            _server.Close();

            return Client;
        }

        private void SetupListener()
        {                    
            IPAddress address = ServerPeer.Peer.PeerAddress.PrivateEndPoint.Address;
            MessageManager messageManager = ServerPeer.Peer.MessageManager;
            _server = new ServerTcp(address, 0, messageManager);            
            _server.Bind();                                                       
        }               
    }
}
