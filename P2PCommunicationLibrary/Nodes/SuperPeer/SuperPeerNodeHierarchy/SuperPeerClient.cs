using P2PCommunicationLibrary.Net;

namespace P2PCommunicationLibrary.SuperPeer
{
    class SuperPeerClient : SuperPeerNode
    {        
        public SuperPeerServer SuperPeerServer { get; set; }

        public SuperPeerClient(IClient peerClient)
            : base(peerClient)
        {           
        }               
    }
}
