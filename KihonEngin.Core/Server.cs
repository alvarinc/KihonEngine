namespace KihonEngin.Core
{
    using LiteNetLib;
    using LiteNetLib.Utils;
    using System;

    internal class Server
    {
        private NetManager _server;
        private EventBasedNetListener _listener;
        private ServerGameLogic _gameLogic;
        private bool _running = true;

        public IEnumerable<NetPeer> GetConnectedPeers() => _server.ConnectedPeerList;

        public Server()
        {
            _listener = new EventBasedNetListener();
            _server = new NetManager(_listener);
            _gameLogic = new ServerGameLogic(this);
        }

        public void Run()
        {
            _server.Start(9050);

            _listener.ConnectionRequestEvent += request =>
            {
                if (_server.ConnectedPeersCount < 10)
                    request.AcceptIfKey("SomeConnectionKey");
                else
                    request.Reject();
            };

            _listener.PeerConnectedEvent += peer =>
            {
                Console.WriteLine($"[Server] Client connected: {peer}");
                _gameLogic.OnClientConnected(peer);
            };

            _listener.NetworkReceiveEvent += (fromPeer, dataReader, deliveryMethod, channel) =>
            {
                string message = dataReader.GetString(100);
                Console.WriteLine($"[Server] Received: {message}");
                _gameLogic.OnMessageReceived(fromPeer, message);
                dataReader.Recycle();
            };

            while (_running)
            {
                _server.PollEvents();
                Thread.Sleep(15);
            }

            _server.Stop();
        }

        public void SendMessage(NetPeer peer, string message)
        {
            var writer = new NetDataWriter();
            writer.Put(message);
            peer.Send(writer, DeliveryMethod.ReliableOrdered);
        }

        public void Stop()
        {
            _running = false;
        }
    }
}
