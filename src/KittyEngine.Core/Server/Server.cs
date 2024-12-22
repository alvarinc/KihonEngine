﻿namespace KittyEngine.Core.Server
{
    using KittyEngine.Core.Client;
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

        public void Run(int port)
        {
            _server.Start(port);

            _listener.ConnectionRequestEvent += request =>
            {
                if (_server.ConnectedPeersCount < 10)
                    request.AcceptIfKey("Client=KittyEngine.Core.Client");
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
                GameCommandInput input = null;

                try
                {
                    input = JsonConvert.DeserializeObject<GameCommandInput>(message);
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

            GameLoop();

            _server.Stop();
        }

        private void GameLoop()
        {
            while (_running)
            {
                // Update players
                HandleClientEvents();

                // Update AIs
                // TODO

                // Update physics
                // TODO

                Thread.Sleep(15);
            }
        }

        public void SendMessage(NetPeer peer, GameCommandInput cmd)
        {
            var writer = new NetDataWriter();
            var json = JsonConvert.SerializeObject(cmd);
            writer.Put(json);
            peer.Send(writer, DeliveryMethod.ReliableOrdered);
        }

        private void HandleClientEvents()
        {
            _server.PollEvents();
        }
    }
}
