namespace KihonEngine.Core.Server
{
    using LiteNetLib;
    using LiteNetLib.Utils;
    using Newtonsoft.Json;
    using System;
    using System.Text.Json;
    using System.Text.Json.Nodes;

    public class Server
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
                    request.AcceptIfKey("Client=KihonEngine.Core.Client");
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
                string message = dataReader.GetString();
                Console.WriteLine($"[Server] Received: {message}");
                GameCommand input = null;

                try
                {
                    input = JsonConvert.DeserializeObject<GameCommand>(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Server] invalid message from {fromPeer.Id}");
                }

                if (input != null)
                {
                    _gameLogic.OnMessageReceived(fromPeer, input);
                }

                dataReader.Recycle();

            };

            while (_running)
            {
                _server.PollEvents();
                Thread.Sleep(15);
            }

            _server.Stop();
        }

        public void SendMessage(NetPeer peer, GameCommand cmd)
        {
            var writer = new NetDataWriter();
            var json = JsonConvert.SerializeObject(cmd);
            writer.Put(json);
            peer.Send(writer, DeliveryMethod.ReliableOrdered);
        }

        public void Stop()
        {
            _running = false;
        }
    }
}
