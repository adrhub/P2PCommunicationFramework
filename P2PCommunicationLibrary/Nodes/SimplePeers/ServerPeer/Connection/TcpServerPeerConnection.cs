using System;
using System.Net;
using System.Threading;
using P2PCommunicationLibrary.Messages;
using P2PCommunicationLibrary.Net;

namespace P2PCommunicationLibrary.SimplePeers.ServerPeer
{
    class TcpServerPeerConnection : ServerPeerConnection
    {
        private ServerTcp _serverTcp;

        public TcpServerPeerConnection(ServerPeer serverPeer)
            : base(serverPeer)
        {           
        }

        public override void ProcessConnection()
        {            
            _serverTcp = ServerPeer.GetTcpServerInstance();
            AllowClientToConnect();
            var serverPrivateIpEndPoint = new IPEndPoint(ServerPeer.GetPeerAddress().PrivateEndPoint.Address, ServerPeer.ServerPeerPort);

            ServerPeer.Peer.SendToSuperPeer(new PeerAddressMessage(new PeerAddress {PrivateEndPoint = serverPrivateIpEndPoint}));           
            Console.WriteLine("...message sent...");
        }

        private void AllowClientToConnect()
        {
            AddMethodToNewClientEvent(_serverTcp, ServerOnNewClientEvent);

            PeriodicTask eventCleaner = new PeriodicTask(
                () => RemoveMethodFromNewClientEvent(_serverTcp, ServerOnNewClientEvent),
                TimeSpan.FromSeconds(0),
                TimeSpan.FromSeconds(60),
                CancellationToken.None);

            eventCleaner.DoPeriodicWorkAsync();
        }

        private void ServerOnNewClientEvent(IServer sender, IClient newClient)
        {
            PeerAddressMessage peerAddressMessage = (PeerAddressMessage) newClient.Read();

            if (!(peerAddressMessage.PeerAddress.Equals(ServerPeer.GetPeerAddress())
                  & newClient.RemoteEndPoint.Equals(ServerPeer.GetPeerAddress().PublicEndPoint)))
                return;

            RemoveMethodFromNewClientEvent(_serverTcp, ServerOnNewClientEvent);
        }        

        public override void Close()
        {
            throw new System.NotImplementedException();
        }
    }
}
