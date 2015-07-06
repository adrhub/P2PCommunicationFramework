namespace P2PCommunicationLibrary.Communication.Peers
{
    public class Peer
    {
        public PeerEndPoint EndPoint { get; private set; }
        public ConnectionType DefaultConnectionType { get; set; }

        public Peer(PeerEndPoint endPoint)
        {
   
        }

        public void Connect(SuperPeer superNode)
        {
                
        }
    }
}
