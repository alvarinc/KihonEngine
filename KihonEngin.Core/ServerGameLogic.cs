namespace KihonEngin.Core
{
    using LiteNetLib;
    using System;
    using System.Collections.Generic;

    internal class ServerGameLogic
    {
        private readonly Server _server;

        // Represents a player in the 3D space
        private class Player
        {
            public int Id { get; }
            public float X { get; set; }
            public float Y { get; set; }
            public float Z { get; set; }

            public Player(int id)
            {
                Id = id;
                X = 0;
                Y = 0;
                Z = 0;
            }

            public string GetPosition()
            {
                return $"{X:0.0},{Y:0.0},{Z:0.0}";
            }
        }

        private readonly Dictionary<int, Player> _players = new();

        public ServerGameLogic(Server server)
        {
            _server = server;
        }

        public void OnClientConnected(NetPeer peer)
        {
            // Create a new player for the client
            var player = new Player(peer.Id);
            _players[peer.Id] = player;

            Console.WriteLine($"[Server] Player connected. ID: {peer.Id}");
            _server.SendMessage(peer, $"CONNECTED:{player.GetPosition()}");
        }

        public void OnMessageReceived(NetPeer peer, string message)
        {
            if (!_players.TryGetValue(peer.Id, out Player player))
            {
                Console.WriteLine("[Server] Received a message from an unknown player.");
                return;
            }

            if (message == "STOP")
            {
                Console.WriteLine($"[Server] Player {peer.Id} requested to stop. Disconnecting...");
                peer.Disconnect();
                _players.Remove(peer.Id);
            }
            else if (message.StartsWith("MOVE:"))
            {
                string[] parts = message.Substring(5).Split(',');
                if (parts.Length == 3 &&
                    float.TryParse(parts[0], out float dx) &&
                    float.TryParse(parts[1], out float dy) &&
                    float.TryParse(parts[2], out float dz))
                {
                    player.X += dx;
                    player.Y += dy;
                    player.Z += dz;

                    Console.WriteLine($"[Server] Player {peer.Id} moved to: {player.GetPosition()}");

                    // Notify all players of the updated position
                    BroadcastMessage($"UPDATE:{peer.Id}:{player.GetPosition()}");
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
