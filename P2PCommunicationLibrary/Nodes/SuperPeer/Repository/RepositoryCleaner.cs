using System;
using System.Collections.Generic;
using System.Linq;
using P2PCommunicationLibrary.Net;

namespace P2PCommunicationLibrary.SuperPeer
{
    class RepositoryCleaner
    {
        public void BeginCleanRepository()
        {
            
        }

        public void ClientOnConnectionClosedEvent(IClient client, EventArgs enentArgs)
        {
            CleanClientRepository(client);
            CleanConnectionRepository(client);
        }

        private static void CleanConnectionRepository(IClient closedClient)
        {
            CleanClientFromConnectionsRepository(closedClient);
            CleanServerFromConnectionsRepository(closedClient);
            CleanConnectionsFromConnectionsRepository(closedClient);
        }

        private static void CleanConnectionsFromConnectionsRepository(IClient closedClient)
        {
            List<ConnectionPair> connectionPairList = ConnectionsRepository.GetConnections();

            foreach (ConnectionPair connectionPair in connectionPairList.Where(connectionPair => connectionPair.ContainsClient(closedClient)))
            {
                ConnectionsRepository.RemoveConnection(connectionPair);
                connectionPair.CloseConnection();
            }
        }

        private static void CleanServerFromConnectionsRepository(IClient closedClient)
        {
            List<SuperPeerServer> serverList = ConnectionsRepository.GetServers();

            foreach (SuperPeerServer server in serverList.Where(server => server.GetSuperPeerClient() == closedClient))
            {
                ConnectionsRepository.RemoveServer(server);
            }
        }

        private static void CleanClientFromConnectionsRepository(IClient closedClient)
        {
            List<SuperPeerClient> clientList = ConnectionsRepository.GetClients();

            foreach (SuperPeerClient client in clientList.Where(client => client.GetSuperPeerClient() == closedClient))
            {
                ConnectionsRepository.RemoveClient(client);
            }
        }

        private static void CleanClientRepository(IClient closedClient)
        {
            ClientRepository.RemoveClient(closedClient);            
        }
    }
}
