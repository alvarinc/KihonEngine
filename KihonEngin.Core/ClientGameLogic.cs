namespace KihonEngin.Core
{
    using System;

    internal class ClientGameLogic
    {
        private readonly Client _client;
        private float _x, _y, _z; // Player position

        public ClientGameLogic(Client client)
        {
            _client = client;
        }

        public void HandleInput()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
                string keyPressed = keyInfo.Key.ToString();

                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine("[Client] ESC key pressed. Stopping client...");
                    _client.SendMessage("STOP");
                    _client.Stop();
                    return;
                }

                float dx = 0, dy = 0, dz = 0;

                // Map keys to movement
                switch (keyPressed)
                {
                    case "Z": dz = 1; break;  // Forward
                    case "S": dz = -1; break; // Backward
                    case "Q": dx = -1; break; // Left
                    case "D": dx = 1; break;  // Right
                    case "Spacebar": dy = 1; break; // Up
                    case "C": dy = -1; break;     // Down
                }

                // Send movement command to the server
                _client.SendMessage($"MOVE:{dx},{dy},{dz}");
            }
        }

        public void OnMessageReceived(string message)
        {
            if (message.StartsWith("CONNECTED:"))
            {
                string[] parts = message.Substring(10).Split(',');
                if (parts.Length == 3 &&
                    float.TryParse(parts[0], out _x) &&
                    float.TryParse(parts[1], out _y) &&
                    float.TryParse(parts[2], out _z))
                {
                    Console.WriteLine($"[Client] Connected. Initial position: {_x}, {_y}, {_z}");
                }
            }
            else if (message.StartsWith("UPDATE:"))
            {
                string[] parts = message.Substring(7).Split(':');
                if (parts.Length == 2 && Guid.TryParse(parts[0], out Guid playerId))
                {
                    string position = parts[1];
                    Console.WriteLine($"[Client] Player {playerId} moved to: {position}");
                }
            }
        }
    }
}
