using System;
using System.Threading;
using P2PCommunicationLibrary.Messages;
using P2PCommunicationLibrary.Net;
using P2PCommunicationLibrary.SimplePeers.ServerPeer.ServerInstances;

namespace P2PCommunicationLibrary.SimplePeers.ServerPeer
{
    class TcpServerPeerConnection : ServerPeerConnection
    {
        private ServerTCP _server;

        public TcpServerPeerConnection(ServerPeer serverPeer, PeerAddress peerAddress)
            : base(serverPeer, peerAddress)
        {           
        }

        public override void ProcessConnection()
        {            
            _server = TcpServerSingleton.GetInstance();
            AllowClientToConnect();
            ServerPeer.Peer.SendToSuperPeer(new ConfirmationMessage(MessageType.TcpConnection));
        }

        private void AllowClientToConnect()
        {
            AddMethodToNewClientEvent(_server, ServerOnNewClientEvent);

            PeriodicTask eventCleaner = new PeriodicTask(
                () => RemoveMethodFromNewClientEvent(_server, ServerOnNewClientEvent),
                TimeSpan.FromSeconds(0),
                TimeSpan.FromSeconds(60),
                CancellationToken.None);

            eventCleaner.DoPeriodicWorkAsync();
        }

        private void ServerOnNewClientEvent(IServer sender, IClient newClient)
        {
            PeerAddressMessage peerAddressMessage = (PeerAddressMessage) newClient.Read();

            if (!(peerAddressMessage.PeerAddress.Equals(PeerAddress)
                  & newClient.RemoteEndPoint.Equals(PeerAddress.PublicEndPoint)))
                return;


            RemoveMethodFromNewClientEvent(_server, ServerOnNewClientEvent);
        }        

        public override void Close()
        {
            throw new System.NotImplementedException();
        }
    }
}
