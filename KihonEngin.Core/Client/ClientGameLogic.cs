namespace KihonEngine.Core.Client
{
    using KihonEngine.Core.Server;
    using KihonEngine.Core.State;
    using Microsoft.AspNetCore.JsonPatch;
    using Newtonsoft.Json;
    using System;

    internal class ClientGameLogic
    {
        private readonly Client _client;
        private string _playerId;
        private GameState _gameState = null;

        public ClientGameLogic(Client client)
        {
            _client = client;
        }

        public void ViewAs(string playerId)
        {
            _playerId = playerId;
        }

        public GameCommandInput HandleInput()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
                string keyPressed = keyInfo.Key.ToString();

                if (keyPressed == "Escape")
                {
                    Console.WriteLine("[Client] ESC key pressed. Stopping client...");
                    return new GameCommandInput("exit");
                }
                else
                {
                    float dx = 0, dy = 0, dz = 0;

                    // Map keys to movement
                    switch (keyPressed)
                    {
                        case "UpArrow": dz = 1; break;  // Forward
                        case "Z": dz = 1; break;  // Forward
                        case "S": dz = -1; break; // Backward
                        case "DownArrow": dz = -1; break; // Backward
                        case "Q": dx = -1; break; // Left
                        case "LeftArrow": dx = -1; break; // Left
                        case "D": dx = 1; break;  // Right
                        case "RightArrow": dx = 1; break;  // Right
                        case "Spacebar": dy = 1; break; // Up
                        case "C": dy = -1; break;     // Down
                    }

                    // Send movement command to the server
                    var cmd = new GameCommandInput("move");
                    cmd.Args["dx"] = dx.ToString();
                    cmd.Args["dy"] = dy.ToString();
                    cmd.Args["dz"] = dz.ToString();
                    return cmd;
                }
            }

            return null;
        }

        public void OnMessageReceived(GameCommandInput cmd)
        {
            if (cmd.Command == "sync")
            {
                var entity = cmd.Args["entity"];
                var mode = cmd.Args["mode"];
                var value = cmd.Args["value"];

                if (mode == "full")
                {
                    _gameState = JsonConvert.DeserializeObject<GameState>(value);
                }
                else if (mode == "patch")
                {
                    var jsonPatch = JsonConvert.DeserializeObject<JsonPatchDocument>(value);
                    jsonPatch.ApplyTo(_gameState);
                }

                var player = _gameState.Players.Values.FirstOrDefault(x => x.Guid == _playerId);
                if ( player != null)
                {
                    Console.WriteLine($"[Client] Connected. Position: {player.Position.X}, {player.Position.Y}, {player.Position.Z}");
                }
                else
                {
                    Console.WriteLine($"[Client] Connected. No position yet.");
                }

                ClientRenderer.Render(_gameState, _playerId);
            }
        }
    }
}
