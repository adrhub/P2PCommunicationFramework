namespace P2PCommunicationLibrary.SimplePeers.ClientPeer
{
    class TcpClientPeerConnection : ClientPeerConnection
    {
        public TcpClientPeerConnection(ClientPeer serverPeer, PeerAddress peerAddress) 
            : base(serverPeer, peerAddress)
        {

        }

        public override void ProcessConnection()
        {
            throw new System.NotImplementedException();
        }

        public override void Close()
        {
            throw new System.NotImplementedException();
        }
    }
}
