using System;
using P2PCommunicationLibrary.Net;

namespace P2PCommunicationLibrary.SuperPeer
{
    class ConnectionPair
    {       
        public SuperPeerClient Client { get; private set; }
        public SuperPeerServer Server{ get; private set; }         

        public ConnectionPair(SuperPeerServer server, SuperPeerClient client)
        {
            Server = server;
            Client = client;
        }

        protected bool Equals(ConnectionPair other)
        {
            return Equals(Client.GetSuperPeerClient().LocalEndPoint.ToString(), other.Client.GetSuperPeerClient().LocalEndPoint.ToString())
                   && Equals(Server.GetSuperPeerClient().LocalEndPoint.ToString(), other.Server.GetSuperPeerClient().LocalEndPoint.ToString());
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
                return ((Client != null ? Client.GetSuperPeerClient().LocalEndPoint.ToString().GetHashCode() : 0)*397) 
                    ^ (Server != null ? Server.GetSuperPeerClient().LocalEndPoint.ToString().GetHashCode() : 0);
            }
        }

        public bool ContainsClient(IClient client)
        {
            return Client.GetSuperPeerClient() == client || Server.GetSuperPeerClient() == client;
        }

        public void CloseConnection()
        {
            throw new NotImplementedException();
        }
    }
}
