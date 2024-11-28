namespace KihonEngin.Core
{
    using LiteNetLib;
    using LiteNetLib.Utils;
    using System;

    internal class Client
    {
        private NetManager _client;
        private EventBasedNetListener _listener;
        private ClientGameLogic _gameLogic;
        private bool _running = true;

        public Client()
        {
            _listener = new EventBasedNetListener();
            _client = new NetManager(_listener);
            _gameLogic = new ClientGameLogic(this);
        }

        public void Run()
        {
            _client.Start();
            _client.Connect("localhost", 9050, "SomeConnectionKey");

            _listener.NetworkReceiveEvent += (fromPeer, dataReader, deliveryMethod, channel) =>
            {
                string message = dataReader.GetString(100);
                Console.WriteLine($"[Client] Received: {message}");
                _gameLogic.OnMessageReceived(message);
                dataReader.Recycle();
            };

            _listener.PeerDisconnectedEvent += (peer, disconnectInfo) =>
            {
                Console.WriteLine("[Client] Disconnected from server.");
                _running = false;
            };

            Console.WriteLine("[Client] Press keys to send to server. Press ESC to stop.");

            while (_running)
            {
                _gameLogic.HandleInput();
                _client.PollEvents();
                Thread.Sleep(15);
            }

            _client.Stop();
        }

        public void SendMessage(string message)
        {
            if (_client != null && _client.FirstPeer != null && _client.FirstPeer.ConnectionState == ConnectionState.Connected)
            {
                var writer = new NetDataWriter();
                writer.Put(message);
                _client.FirstPeer.Send(writer, DeliveryMethod.ReliableOrdered);
            }
        }

        public void Stop()
        {
            _running = false;
        }
    }
}
