namespace KihonEngine.Core.Server
{
    using KihonEngine.Core.Common;
    using KihonEngine.Core.State;
    using LiteNetLib;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Text.Json;

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
            var player = new PlayerState(peer.Id);
            var previous = JsonConvert.SerializeObject(_gameState);
            _gameState.Players[peer.Id] = player;
            var updated = JsonConvert.SerializeObject(_gameState);
            var patch = JsonDiffPatch.Diff(previous, updated);
            var jsonPatch = JsonConvert.SerializeObject(patch);

            Console.WriteLine($"[Server] Player connected. ID: {peer.Id}");
            var cmd = new GameCommand("sync");
            cmd.Args["entity"] = "gamestate";
            cmd.Args["mode"] = "full";
            cmd.Args["value"] = updated;
            _server.SendMessage(peer, cmd);

            var updateCmd = new GameCommand("sync");
            updateCmd.Args["entity"] = "gamestate";
            updateCmd.Args["mode"] = "full";
            updateCmd.Args["value"] = updated;
            BroadcastMessage(updateCmd);
        }

        public void OnMessageReceived(NetPeer peer, GameCommand input)
        {
            if (!_gameState.Players.TryGetValue(peer.Id, out PlayerState player))
            {
                Console.WriteLine($"[Server] Player {peer.Id} : Received a message from an unknown player.");
                return;
            }

            if (input.Command != "login" && string.IsNullOrEmpty(player.Guid))
            {
                Console.WriteLine($"[Server] Player {peer.Id} Not logged in");
                return;
            }

            var previous = JsonConvert.SerializeObject(_gameState);
            var stateUpdated = false;

            if (input.Command == "login")
            {
                player.Guid = input.Args["guid"];
                player.Name = input.Args["name"];
                stateUpdated = true;
                Console.WriteLine($"[Server] Player {peer.Id} Logged as {player.Name}");
            }
            else if (input.Command == "exit")
            {
                Console.WriteLine($"[Server] Player {player.Name} requested to stop. Disconnecting...");
                peer.Disconnect();
                _gameState.Players.Remove(peer.Id);
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
                    if (dx != 0 && player.Position.X + dx >= _gameState.Map.MinX && player.Position.X + dx <= _gameState.Map.MaxX)
                    {
                        player.Position.X += dx;
                        stateUpdated = true;
                    }

                    if (dy != 0 && player.Position.Y + dy >= _gameState.Map.MinY && player.Position.Y + dy <= _gameState.Map.MaxY)
                    {
                        player.Position.Y += dy;
                        stateUpdated = true;
                    }

                    if (dz != 0 && player.Position.Z + dz >= _gameState.Map.MinZ && player.Position.Z + dz <= _gameState.Map.MaxZ)
                    {
                        player.Position.Z += dz;
                        stateUpdated = true;
                    }

                    if (stateUpdated)
                    {
                        Console.WriteLine($"[Server] Player {player.Name} moved to: {player.Position.X}:{player.Position.Y}:{player.Position.Z}");
                    }
                }
            }

            if (stateUpdated)
            {
                var updated = JsonConvert.SerializeObject(_gameState);
                //var patch = JsonDiffPatch.Diff(previous, updated);
                //var jsonPatch = JsonConvert.SerializeObject(patch);

                var updateCmd = new GameCommand("sync");
                updateCmd.Args["entity"] = "gamestate";
                updateCmd.Args["mode"] = "full";
                updateCmd.Args["value"] = updated;
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
