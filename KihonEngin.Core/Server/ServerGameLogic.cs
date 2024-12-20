namespace KihonEngine.Core.Server
{
    using KihonEngine.Core.State;
    using LiteNetLib;
    using System;
    using System.Collections.Generic;


    internal class ServerGameLogic
    {
        private readonly Server _server;

        private readonly Dictionary<int, Player> _connectedUsers = new();
        private readonly GameState _gameState = new();

        public ServerGameLogic(Server server)
        {
            _server = server;
        }

        public void OnClientConnected(NetPeer peer)
        {
            // Register a new player for the client
            _connectedUsers.Add(peer.Id, new Player());
            Console.WriteLine($"[Server] Player connected. ID: {peer.Id}");
        }

        public void OnMessageReceived(NetPeer peer, GameCommand input)
        {
            if (!_connectedUsers.TryGetValue(peer.Id, out Player player))
            {
                Console.WriteLine($"[Server] Player {peer.Id} : Received a message from an unknown player.");
                return;
            }

            if (input.Command != "join" && string.IsNullOrEmpty(player.Guid))
            {
                Console.WriteLine($"[Server] Player {peer.Id} joined no games.");
                return;
            }

            var synchronizer = new StateSynchronizer<GameState>(_gameState);
            var stateUpdated = false;
            var peerFullUpdate = false;

            if (input.Command == "join")
            {
                player.Guid = input.Args["guid"];
                player.Name = input.Args["name"];

                _gameState.Players.Add(peer.Id, new PlayerState(peer.Id));
                _gameState.Players[peer.Id].Name = player.Name;
                _gameState.Players[peer.Id].Guid = player.Guid;
                stateUpdated = true;
                peerFullUpdate = true;
                Console.WriteLine($"[Server] Player {peer.Id} Logged as {player.Name}");
            }
            else if (input.Command == "exit")
            {
                Console.WriteLine($"[Server] Player {player.Name} requested to stop. Disconnecting...");
                peer.Disconnect();
                _gameState.Players.Remove(peer.Id);
                _connectedUsers.Remove(peer.Id);
                stateUpdated = true;
            }
            else if (input.Command == "move")
            {
                var dxString = input.Args["dx"];
                var dyString = input.Args["dy"];
                var dzString = input.Args["dz"];

                if (float.TryParse(dxString, out float dx) &&
                    float.TryParse(dyString, out float dy) &&
                    float.TryParse(dzString, out float dz))
                {
                    var playerState = _gameState.Players[peer.Id];
                    if (dx != 0 && playerState.Position.X + dx >= _gameState.Map.MinX && playerState.Position.X + dx <= _gameState.Map.MaxX)
                    {
                        playerState.Position.X += dx;
                        stateUpdated = true;
                    }

                    if (dy != 0 && playerState.Position.Y + dy >= _gameState.Map.MinY && playerState.Position.Y + dy <= _gameState.Map.MaxY)
                    {
                        playerState.Position.Y += dy;
                        stateUpdated = true;
                    }

                    if (dz != 0 && playerState.Position.Z + dz >= _gameState.Map.MinZ && playerState.Position.Z + dz <= _gameState.Map.MaxZ)
                    {
                        playerState.Position.Z += dz;
                        stateUpdated = true;
                    }

                    if (stateUpdated)
                    {
                        Console.WriteLine($"[Server] Player {playerState.Name} moved to: {playerState.Position.X}:{playerState.Position.Y}:{playerState.Position.Z}");
                    }
                }
            }

            if (peerFullUpdate)
            {
                var initCmd = new GameCommand("sync");
                initCmd.Args["entity"] = "gamestate";
                initCmd.Args["mode"] = "full";
                initCmd.Args["value"] = synchronizer.GetJson();
                _server.SendMessage(peer, initCmd);
            }

            if (stateUpdated)
            {
                var updateCmd = new GameCommand("sync");
                updateCmd.Args["entity"] = "gamestate";
                updateCmd.Args["mode"] = "patch";
                updateCmd.Args["value"] = synchronizer.GetJsonPatch();
                BroadcastMessage(updateCmd);
            }
        }

        private void BroadcastMessage(GameCommand cmd)
        {
            foreach (var peer in _server.GetConnectedPeers())
            {
                _server.SendMessage(peer, cmd);
            }
        }
    }
}
