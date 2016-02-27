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
        private ClientTcp _superPeerClientTcp;

        public TcpServerPeerConnection(ServerPeer serverPeer)
            : base(serverPeer)
        {           
        }

        public override void ProcessConnection()
        {
            IntegerMessage integerMessage = ((IntegerMessage)ServerPeer.Peer.ReadFromSuperPeer());
            int superPeerConnectionPort = integerMessage.Integer;
            ProcessCommunicationOnNewThread(superPeerConnectionPort);


            //new thread            


//            var serverPrivateIpEndPoint = new IPEndPoint(ServerPeer.GetPeerAddress().PrivateEndPoint.Address, ServerPeer.ServerPeerPort);
//
//            ServerPeer.Peer.SendToSuperPeer(new PeerAddressMessage(new PeerAddress {PrivateEndPoint = serverPrivateIpEndPoint}));           
//            Console.WriteLine("...message sent...");
        }

        private void ProcessCommunicationOnNewThread(int superPeerConnectionPort)
        {
            Task.Factory.StartNew(() =>
            {
                RunConnectionTcpClient(superPeerConnectionPort);

            });
        }

        private void RunConnectionTcpClient(int superPeerConnectionPort)
        {

            IPEndPoint superPeerConnEndPoint = new IPEndPoint(ServerPeer.Peer.SuperPeerEndPoint.Address,
                superPeerConnectionPort);
            _superPeerClientTcp = new ClientTcp(superPeerConnEndPoint, ServerPeer.Peer.MessageManager);
        }

//
//        private void ServerOnNewClientEvent(IServer sender, IClient newClient)
//        {
//            PeerAddressMessage peerAddressMessage = (PeerAddressMessage) newClient.Read();
//
//            if (!(peerAddressMessage.PeerAddress.Equals(ServerPeer.GetPeerAddress())
//                  & newClient.RemoteEndPoint.Equals(ServerPeer.GetPeerAddress().PublicEndPoint)))
//                return;
//
//            RemoveMethodFromNewClientEvent(_serverTcp, ServerOnNewClientEvent);
//        }        
//
        public override void Close()
        {
            throw new System.NotImplementedException();
        }
    }
}
