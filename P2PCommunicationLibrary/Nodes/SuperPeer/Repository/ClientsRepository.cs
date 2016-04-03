using System;
using System.Collections.Generic;
using System.Linq;
using P2PCommunicationLibrary.Net;

namespace P2PCommunicationLibrary.SuperPeer
{
    static class ClientRepository
    {
        private static readonly Dictionary<IClient, ClientInfo> Clients;
        private static readonly object ClientsMonitor = new object();        

        static ClientRepository()
        {
            Clients = new Dictionary<IClient, ClientInfo>();
        }

        #region Clients
        public static List<IClient> GetClients()
        {
            lock (ClientsMonitor)
            {
                return new List<IClient>(Clients.Keys);
            }            
        }

        public static void RemoveClient(IClient client)
        {
            lock (ClientsMonitor)
            {                
                Clients.Remove(client);
            }
        }

        public static void AddClient(IClient client, ClientInfo clientInfo)
        {
            lock (ClientsMonitor)
            {
                Clients.Add(client, clientInfo);
            }
        }

        public static void AddClient(IClient client)
        {
            AddClient(client, new ClientInfo(client));      
        }        

        public static ClientInfo GetClientInfo(IClient client)
        {
            lock (ClientsMonitor)
            {
                return Clients[client];
            }
        }
        #endregion        
    }
}
