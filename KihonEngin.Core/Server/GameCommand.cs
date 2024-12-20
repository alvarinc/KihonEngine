namespace KihonEngine.Core.Server
{
    public class GameCommand
    {
        public string Command { get; set; }
        public Dictionary<string, string> Args { get; set; }

        public GameCommand(string commandName)
        {
            Command = commandName;
            Args = new Dictionary<string, string>();
        }
    }
}
