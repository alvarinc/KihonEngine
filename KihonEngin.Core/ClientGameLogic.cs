namespace KihonEngin.Core
{
    using System;

    internal class ClientGameLogic
    {
        private readonly Client _client;

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

                _client.SendMessage($"KEY:{keyPressed}");
                Console.WriteLine($"[Client] Key sent to server: {keyPressed}");
            }
        }

        public void OnMessageReceived(string message)
        {
            // Add any specific client-side logic for handling server messages here
        }
    }
}
