namespace P2PCommunicationLibrary.SuperPeer
{
    abstract class SuperPeerNode
    {
        private readonly IClient _peerClient;

        protected SuperPeerNode(IClient peerClient)
        {
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
    }
}
