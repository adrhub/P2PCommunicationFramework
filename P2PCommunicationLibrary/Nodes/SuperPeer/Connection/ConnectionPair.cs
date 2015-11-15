using System.Collections.Generic;

namespace P2PCommunicationLibrary.SuperPeer
{
    class ConnectionPair
    {
        private readonly List<IClient> _clients = new List<IClient>();

        public IClient Client { get; private set; }
        public IClient Server{ get; private set; }
   
        public List<IClient> Peers => new List<IClient>(_clients);

        public ConnectionPair(IClient server, IClient client)
        {
            Server = server;
            Client = client;

            _clients.Add(server);
            _clients.Add(client);
        }

        protected bool Equals(ConnectionPair other)
        {
            return Equals(Client.LocalEndPoint.ToString(), other.Client.LocalEndPoint.ToString())
                   && Equals(Server.LocalEndPoint.ToString(), other.Server.LocalEndPoint.ToString());
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ConnectionPair) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Client != null ? Client.LocalEndPoint.ToString().GetHashCode() : 0)*397) 
                    ^ (Server != null ? Server.LocalEndPoint.ToString().GetHashCode() : 0);
            }
        }
    }
}
