using System.Collections.Generic;

namespace P2PCommunicationLibrary.SuperPeer
{
    static class ConnectionsRepository
    {
        private static readonly List<ConnectionPair> _connectionPairs;
        private static readonly List<SuperPeerServer> _servers;
        private static readonly List<SuperPeerClient> _clients;

        private static readonly object _peersMonitor = new object();

        static ConnectionsRepository()
        {
            _connectionPairs = new List<ConnectionPair>();
            _servers = new List<SuperPeerServer>();
            _clients = new List<SuperPeerClient>();
        }

        #region Connections
        public static List<ConnectionPair> GetConnections()
        {
            lock (_peersMonitor)
            {
                return new List<ConnectionPair>(_connectionPairs);
            }
        }

        public static void RemoveConnection(ConnectionPair pair)
        {
            lock (_peersMonitor)
            {
                _connectionPairs.Remove(pair);
            }
        }

        public static void AddConnection(ConnectionPair pair)
        {
            lock (_peersMonitor)
            {
                _connectionPairs.Add(pair);
            }
        }
        #endregion

        #region Servers
        public static List<SuperPeerServer> GetServers()
        {
            lock (_peersMonitor)
            {
                return new List<SuperPeerServer>(_servers);
            }
        }

        public static void RemoveServer(SuperPeerServer server)
        {
            lock (_peersMonitor)
            {
                _servers.Remove(server);
            }
        }

        public static void AddServer(SuperPeerServer server)
        {
            lock (_peersMonitor)
            {
                _servers.Add(server);
            }
        }       
        #endregion

        #region Clients
        public static List<SuperPeerClient> GetClients()
        {
            lock (_peersMonitor)
            {
                return new List<SuperPeerClient>(_clients);
            }
        }

        public static void RemoveClient(SuperPeerClient client)
        {
            lock (_peersMonitor)
            {
                _clients.Remove(client);
            }
        }

        public static void AddClient(SuperPeerClient client)
        {
            lock (_peersMonitor)
            {
                _clients.Add(client);
            }
        }
        #endregion
    }
}
