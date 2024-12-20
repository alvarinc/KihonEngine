namespace KihonEngin.Core
{
    using System;

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting server...");
            var server = new Server.Server();
            var serverThread = new Thread(server.Run);
            serverThread.Start();

            Console.WriteLine("Starting client...");
            var client = new Client.Client();
            client.Run();

            Console.WriteLine("Waiting for server shutdown...");
            server.Stop();
            serverThread.Join();
        }
    }
}
