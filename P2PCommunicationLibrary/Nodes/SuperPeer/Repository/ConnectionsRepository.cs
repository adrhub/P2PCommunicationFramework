using System.Collections.Generic;

namespace P2PCommunicationLibrary.SuperPeer
{
    static class ConnectionsRepository
    {
        private static List<ConnectionPair> _connectionPairs;
        private static List<IClient> _servers;
        private static List<IClient> _clients;

        private static object _peersMonitor = new object();

        static ConnectionsRepository()
        {
            _connectionPairs = new List<ConnectionPair>();
            _servers = new List<IClient>();
            _clients = new List<IClient>();
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
        public static List<IClient> GetServers()
        {
            lock (_peersMonitor)
            {
                return new List<IClient>(_servers);
            }
        }

        public static void RemoveServer(IClient client)
        {
            lock (_peersMonitor)
            {
                _servers.Remove(client);
            }
        }

        public static void AddServer(IClient client)
        {
            lock (_peersMonitor)
            {
                _servers.Add(client);
            }
        }
        #endregion

        #region Clients
        public static List<IClient> GetClients()
        {
            lock (_peersMonitor)
            {
                return new List<IClient>(_clients);
            }
        }

        public static void RemoveClient(IClient client)
        {
            lock (_peersMonitor)
            {
                _clients.Remove(client);
            }
        }

        public static void AddClient(IClient client)
        {
            lock (_peersMonitor)
            {
                _clients.Add(client);
            }
        }
        #endregion
    }
}
