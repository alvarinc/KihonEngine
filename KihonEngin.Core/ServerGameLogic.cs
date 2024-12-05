namespace KihonEngin.Core
{
    using LiteNetLib;
    using System;
    using System.Collections.Generic;

    internal class ServerGameLogic
    {
        private readonly Server _server;

        //private readonly Dictionary<int, Player> _players = new();
        private readonly GameState _gameState = new();

        public ServerGameLogic(Server server)
        {
            _server = server;
        }

        public void OnClientConnected(NetPeer peer)
        {
            // Create a new player for the client
            var player = new Player(peer.Id);
            _gameState.Players[peer.Id] = player;

            Console.WriteLine($"[Server] Player connected. ID: {peer.Id}");
            _server.SendMessage(peer, $"CONNECTED:{player.GetPosition()}");
        }

        public void OnMessageReceived(NetPeer peer, string message)
        {
            if (!_gameState.Players.TryGetValue(peer.Id, out Player player))
            {
                Console.WriteLine("[Server] Received a message from an unknown player.");
                return;
            }

            if (message == "STOP")
            {
                Console.WriteLine($"[Server] Player {peer.Id} requested to stop. Disconnecting...");
                peer.Disconnect();
                _gameState.Players.Remove(peer.Id);
            }
            else if (message.StartsWith("MOVE:"))
            {
                string[] parts = message.Substring(5).Split(',');
                if (parts.Length == 3 &&
                    float.TryParse(parts[0], out float dx) &&
                    float.TryParse(parts[1], out float dy) &&
                    float.TryParse(parts[2], out float dz))
                {
                    var updated = false;
                    if (dx != 0 && player.X + dx >= _gameState.Map.MinX && player.X + dx <= _gameState.Map.MaxX)
                    {
                        player.X += dx;
                        updated = true;
                    }

                    if (dy != 0 && player.Y + dy >= _gameState.Map.MinY && player.Y + dy <= _gameState.Map.MaxY)
                    {
                        player.Y += dy;
                        updated = true;
                    }

                    if (dz != 0 && player.Z + dz >= _gameState.Map.MinZ && player.Z + dz <= _gameState.Map.MaxZ)
                    {
                        player.Z += dz;
                        updated = true;
                    }

                    if (updated)
                    {
                        Console.WriteLine($"[Server] Player {peer.Id} moved to: {player.GetPosition()}");

                        // Notify all players of the updated position
                        BroadcastMessage($"UPDATE:{peer.Id}:{player.GetPosition()}");
                    }
                }
            }
        }

        private void BroadcastMessage(string message)
        {
            foreach (var peer in _server.GetConnectedPeers())
            {
                _server.SendMessage(peer, message);
            }
        }
    }
}
