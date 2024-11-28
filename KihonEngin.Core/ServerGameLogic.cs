namespace KihonEngin.Core
{
    using LiteNetLib;
    using System;

    internal class ServerGameLogic
    {
        private readonly Server _server;

        public ServerGameLogic(Server server)
        {
            _server = server;
        }

        public void OnClientConnected(NetPeer peer)
        {
            _server.SendMessage(peer, "Welcome to the server!");
        }

        public void OnMessageReceived(NetPeer peer, string message)
        {
            if (message == "STOP")
            {
                Console.WriteLine("[Server] Client requested to stop. Disconnecting...");
                peer.Disconnect();
            }
            else if (message.StartsWith("KEY:"))
            {
                string key = message.Substring(4);
                Console.WriteLine($"[Server] Key pressed by client: {key}");
            }
        }
    }
}
