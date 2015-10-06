using System;
using System.Collections.Generic;
using System.Linq;

namespace P2PCommunicationLibrary.SuperPeer
{
    static class ClientRepository
    {
        private static List<IClient> _clients;
        private static object _clientsMonitor = new object();        

        static ClientRepository()
        {
            _clients = new List<IClient>();
        }

        #region Clients
        public static List<IClient> GetClients()
        {
            lock (_clientsMonitor)
            {
                return new List<IClient>(_clients);
            }            
        }

        public static void RemoveClient(IClient client)
        {
            lock (_clientsMonitor)
            {
                _clients.Remove(client);
            }
        }

        public static void AddClient(IClient client)
        {
            lock (_clientsMonitor)
            {
                _clients.Add(client);
            }
        }
        #endregion        
    }
}
