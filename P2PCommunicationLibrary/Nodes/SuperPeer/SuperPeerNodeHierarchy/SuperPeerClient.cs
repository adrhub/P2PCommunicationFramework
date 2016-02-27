using P2PCommunicationLibrary.Net;

namespace P2PCommunicationLibrary.SuperPeer
{
    class SuperPeerClient : SuperPeerNode
    {        
        public SuperPeerServer SuperPeerServer { get; set; }

        public SuperPeerClient(SuperPeer superPeer, IClient peerClient)
            : base(superPeer, peerClient)
        {           
        }               
    }
}
