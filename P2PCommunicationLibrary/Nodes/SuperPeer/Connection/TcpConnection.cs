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
            PeerAddressMessage serverConnectionAddress = (PeerAddressMessage)server.Read();
            client.Send(new RequestMessage(MessageType.TcpConnection));
            client.Send(serverConnectionAddress);

        }

//        private void RunConnectionTcpServer()
//        {
//            
//            IPAddress address = Client.GetSuperPeer().Address;
//            MessageManager messageManager = Client.GetSuperPeer().GetMessageManager();
//            _server = new ServerTcp(address, 0, messageManager);
//            _server.NewClientEvent += ServerOnNewClientEvent;
//            _server.Bind();
//
//            Task.Factory.StartNew(() =>
//            {
//                _server.Listen();
//            });
//        }
//
//        private void ServerOnNewClientEvent(IServer sender, IClient newClient)
//        {    
//            Console.WriteLine("dasdasdasda");        
//        }
    }
}
