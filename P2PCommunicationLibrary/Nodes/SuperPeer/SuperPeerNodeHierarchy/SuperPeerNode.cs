using P2PCommunicationLibrary.Net;

namespace P2PCommunicationLibrary.SuperPeer
{
    abstract class SuperPeerNode
    {
        private readonly SuperPeer _superPeer;
        private readonly IClient _peerClient;

        protected SuperPeerNode(SuperPeer superPeer, IClient peerClient)
        {
            _superPeer = superPeer;
            _peerClient = peerClient;
        }

        public IClient GetSuperPeerClient()
        {
            return _peerClient;
        }

        public ClientInfo GetClientInfo()
        {
            return ClientRepository.GetClientInfo(_peerClient);
        }

        public SuperPeer GetSuperPeer()
        {
            return _superPeer;
        }
    }
}
