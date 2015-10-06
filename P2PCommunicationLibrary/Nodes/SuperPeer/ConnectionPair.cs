using System.Collections.Generic;

namespace P2PCommunicationLibrary.SuperPeer
{
    class ConnectionPair
    {
        private readonly List<IClient> _clients = new List<IClient>();

        public IClient Client { get; private set; }
        public IClient Server{ get; private set; }
   
        public List<IClient> Peers
        {
            get
            {
                return new List<IClient>(_clients);
            }
        }
     
        public ConnectionPair(IClient server, IClient client)
        {
            Server = server;
            Client = client;

            _clients.Add(server);
            _clients.Add(client);
        }

        public override bool Equals(object obj)
        {
            ConnectionPair pair = obj as ConnectionPair;

            if (pair == null)
                return false;

            if (this.GetHashCode() == pair.GetHashCode())
                return true;
            
            return false;            
        }

        public override int GetHashCode()
        {
            return _clients[0].RemoteEndPoint.ToString().GetHashCode() ^ 
                _clients[1].RemoteEndPoint.ToString().GetHashCode();
        }
    }
}
