using System;
using System.Threading.Tasks;
using P2PCommunicationLibrary.Net;

namespace P2PCommunicationLibrary.SuperPeer
{
    class ConnectionPair
    {
        private Connection _connection;
               
        public SuperPeerClient Client { get; private set; }
        public SuperPeerServer Server { get; private set; }         

        public ConnectionPair(SuperPeerServer server, SuperPeerClient client)
        {
            Server = server;
            Client = client;
        }

        public void ProcessConnection()
        {
            if (Server.GetClientInfo().PeerAddress().PublicEndPoint.Address.Equals(
                Client.GetClientInfo().PeerAddress().PublicEndPoint.Address))
            {
                _connection = new TcpConnection(Server, Client);
            }

            if ( _connection != null)
                _connection.ProcessConnection();
        }

        public bool ContainsClient(IClient client)
        {
            return Client.GetSuperPeerClient() == client || Server.GetSuperPeerClient() == client;
        }

        public void CloseConnection(IClient closedClient)
        {
            _connection.Close(closedClient);           
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

        
    }
}
