namespace KihonEngine.Core.Server
{
    using KihonEngine.Core.Server.Commands;
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

        public void OnMessageReceived(NetPeer peer, GameCommandInput input)
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
            IGameCommand command = null;

            if (input.Command == "join")
            {
                command = new JoinCommand();
            }
            else if (input.Command == "exit")
            {
                command = new ExitCommand();
            }
            else if (input.Command == "move")
            {
                command = new MoveCommand();
            }
            else
            {
                Console.WriteLine($"Command not registered : {input.Command}");
            }

            if (command != null && command.ValidateParameters(input))
            {
                var result = command.Execute(_gameState, player, peer.Id);

                if (result.StateUpdated)
                {
                    var patchCmd = new GameCommandInput("sync");
                    patchCmd.Args["entity"] = "gamestate";
                    patchCmd.Args["mode"] = "patch";
                    patchCmd.Args["value"] = synchronizer.GetJsonPatch();

                    var fullCmd = new GameCommandInput("sync");
                    fullCmd.Args["entity"] = "gamestate";
                    fullCmd.Args["mode"] = "full";
                    fullCmd.Args["value"] = synchronizer.GetJson();

                    foreach (var connectedPeer in _server.GetConnectedPeers())
                    {
                        if (result.PeerInitializated && connectedPeer == peer)
                        {
                            _server.SendMessage(connectedPeer, fullCmd);
                        }
                        else
                        {
                            _server.SendMessage(connectedPeer, patchCmd);
                        }
                    }
                }
            }

            if (!_gameState.Players.ContainsKey(peer.Id))
            {
                peer.Disconnect();
                _connectedUsers.Remove(peer.Id);
            }
        }
    }
}
