using System;
using System.Collections.Generic;

namespace P2PCommunicationLibrary.SuperPeer
{
    class SuperPeerServer : SuperPeerNode
    {
        private readonly object _monitor = new object();       
        private List<SuperPeerClient> connectedClients = new List<SuperPeerClient>();
        private List<PeerAddress> allowedConnections = new List<PeerAddress>();
       
        public SuperPeerServer(IClient peerClient)
            : base(peerClient)
        {            
        }       

        public List<SuperPeerClient> GetConnectedClients()
        {
            lock (_monitor)
            {
                return new List<SuperPeerClient>(connectedClients);
            }
        }

        public void AddConnectedClient(SuperPeerClient client)
        {
            lock (_monitor)
            {
                connectedClients.Add(client);
            }
        }

        public void RemoveConnectedClient(SuperPeerClient client)
        {
            lock (_monitor)
            {
                connectedClients.Remove(client);
            }
        }



        public List<PeerAddress> GetAllowedConnections()
        {
            lock (_monitor)
            {
                return new List<PeerAddress>(allowedConnections);
            }
        }

        public void AddAddressToAllowedConnections(PeerAddress peerAddress)
        {
            lock (_monitor)
            {
                allowedConnections.Add(peerAddress);               
            }
        }

        public void RemoveAddressFromAllowedConnections(PeerAddress peerAddress)
        {
            lock (_monitor)
            {
                allowedConnections.Remove(peerAddress);
            }
        }

    }
}
