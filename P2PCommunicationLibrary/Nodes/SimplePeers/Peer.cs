﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using P2PCommunicationLibrary.Messages;

namespace P2PCommunicationLibrary.Peers
{
    class Peer
    {
        private IClient _superPeerClient;
        private IPEndPoint _superPeerEndPoint;
        private IEncryptor _encryptor;
        private MessageManager _messageManager;

        public PeerAddress PeerAddress{ get; private set; }
        public bool IsRunning { get; private set; }
        public IEncryptor Encryptor
        {
            get { return _encryptor; }
            set
            {
                if (!IsRunning)
                {
                    _encryptor = value;
                }
            }   
        }    
        
        public Peer(IPEndPoint superPeerEndPoint)
        {
            InitMessageManager();
            _superPeerEndPoint = superPeerEndPoint;
        }

        public Peer(IPEndPoint superPeerEndPoint, IEncryptor encryptor)
        {
            Encryptor = encryptor;
            InitMessageManager();
            _superPeerEndPoint = superPeerEndPoint;
        }

        private void InitMessageManager()
        {
            if (Encryptor != null)
                _messageManager = new MessageManager(Encryptor);
            else
                _messageManager = new MessageManager();
        }

        /// <summary>
        /// Connectiong to Super Peer
        /// </summary>
        public void Run(ClientType clientType)
        {
            IsRunning = true;           

            try
            {              
                InitSuperPeerConnection();
                InitPeerType(clientType);
                PeerAddress = InitPeerAddress();

                //Read confirmation message
                _superPeerClient.Read();
            }
            catch (SocketException se)
            {
                Trace.WriteLine("Error connectiong to the Server");
                Trace.WriteLine("SocketException: " + se.ErrorCode + " " + se.Message);
                Close();
                throw;
            }
        }        

        private void InitSuperPeerConnection()
        {
            _superPeerClient = new ClientTCP(_superPeerEndPoint, _messageManager);
            _superPeerClient.Send(new ConnectionMessage());
        }

        private void InitPeerType(ClientType clientType)
        {           
            if (clientType == ClientType.Client)
                _superPeerClient.Send(new RequestMessage(MessageType.InitConnectionAsClient));
            else if (clientType == ClientType.Server)
            {             
                _superPeerClient.Send(new RequestMessage(MessageType.InitConnectionAsServer));                               
            }           
        }

        private PeerAddress InitPeerAddress()
        {                      
            var requestMessage = new RequestMessage(MessageType.ClientPeerAddress);
            _superPeerClient.Send(requestMessage);

            PeerAddress peerAddress = ((PeerAddressMessage)_superPeerClient.Read()).PeerAddress;
            peerAddress.PrivateEndPoint = new IPEndPoint(LocalIpAddress(), _superPeerClient.LocalEndPoint.Port);
            _superPeerClient.Send(new PeerAddressMessage(peerAddress, MessageType.ClientPeerAddress));
           
            return peerAddress;
        }

        public void Close()
        {
            if (IsRunning)
            {
                IsRunning = false;
                _superPeerClient.Close();
            }
        }                   

        private static IPAddress LocalIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            return host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        }

        public void SendToSuperPeer(BinaryMessageBase message)
        {
            _superPeerClient.Send(message);
        }

        public BinaryMessageBase ReadFromSuperPeer()
        {
            return _superPeerClient.Read();
        }
    }
}