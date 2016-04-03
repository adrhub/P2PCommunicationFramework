using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using P2PCommunicationLibrary.Messages;
using P2PCommunicationLibrary.Net;

namespace P2PCommunicationLibrary.SuperPeer
{
    static class RepositoryCleaner
    {
        public static void CheckClientsConnection()
        {
            Console.WriteLine("check...");

            DisplayRepositoryState();

            CheckClientsLastPingMessageDateTime();
            CheckSocketConnection();
        }

        private static void DisplayRepositoryState()
        {
            Console.WriteLine("Clients :" + ClientRepository.GetClients().Count
                              + "\nConnections :" + ConnectionsRepository.GetConnections().Count
                              + "\nPeerClients :" + ConnectionsRepository.GetClients().Count
                              + "\nPeerServers :" + ConnectionsRepository.GetServers().Count);
        }

        private static void CheckSocketConnection()
        {
            foreach (IClient client in ClientRepository.GetClients())
            {
                if (!client.ClientSocket.IsConnected())
                    CleanRepositories(client);
            }
        }

        private static bool IsConnected(this Socket socket)
        {
            try
            {
                return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
            }
            catch (SocketException)
            {
                return false;
            }
        }

        private static void CheckClientsLastPingMessageDateTime()
        {
            foreach (IClient client in ClientRepository.GetClients())
            {
                DateTime lastPingMessageDateTime = ClientRepository.GetClientInfo(client).LastPingMesssageDateTime;

                if ((DateTime.Now - lastPingMessageDateTime).TotalSeconds > 60)
                    CleanRepositories(client);
            }
        }

        public static void ClientOnConnectionClosedEvent(IClient client, EventArgs enentArgs)
        {            
            CleanRepositories(client);
        }

        public static void CleanRepositories(IClient client)
        {            
            //DisplayRepositoryState();

            CleanClientRepository(client);
            CleanConnectionRepository(client);

            DisplayRepositoryState();
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
                connectionPair.CloseConnection(closedClient);
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
