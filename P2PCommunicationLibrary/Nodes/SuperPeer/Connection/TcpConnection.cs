using System;
using System.Net;
using System.Threading.Tasks;
using P2PCommunicationLibrary.Messages;
using P2PCommunicationLibrary.Net;

namespace P2PCommunicationLibrary.SuperPeer
{
    class TcpConnection : Connection
    {
        private ServerTcp _server;
        public TcpConnection(SuperPeerServer server, SuperPeerClient client)
            : base(server, client)
        {            
        }

        public override void ProcessConnection()
        {
            IClient server = Server.GetSuperPeerClient();
            IClient client = Client.GetSuperPeerClient();            
                        
            server.Send(new RequestMessage(MessageType.TcpConnection));
            RunConnectionTcpServer();

            server.Send(new IntegerMessage(_server.Port, MessageType.ConnectionPort));

           //new thread

//           // string clientToken = Client.GetClientInfo().Token.Key;
//            //server.Send(new TextMessage(clientToken));
//
//            //read confirmation message, check if server is ready; read port
//            var serverPrivateIpEndPoint = (PeerAddressMessage)server.Read(); // 111
//
//            //say to client that he can connect to the server; send port
//            client.Send(serverPrivateIpEndPoint);
//            //read confirmation message, check if client connected to the server
//            client.Read();
        }

        private void RunConnectionTcpServer()
        {
            
            IPAddress address = Client.GetSuperPeer().Address;
            MessageManager messageManager = Client.GetSuperPeer().GetMessageManager();
            _server = new ServerTcp(address, 0, messageManager);
            _server.NewClientEvent += ServerOnNewClientEvent;
            _server.Bind();

            Task.Factory.StartNew(() =>
            {
                _server.Listen();
            });
        }

        private void ServerOnNewClientEvent(IServer sender, IClient newClient)
        {    
            Console.WriteLine("dasdasdasda");        
        }
    }
}
