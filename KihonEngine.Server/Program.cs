namespace KihonEngine.Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting server...");
            var server = new KihonEngine.Core.Server.Server();
            server.Run();
        }
    }
}
