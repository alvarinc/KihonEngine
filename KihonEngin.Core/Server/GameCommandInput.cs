namespace KihonEngine.Core.Server
{
    public class GameCommandInput
    {
        public string Command { get; set; }
        public Dictionary<string, string> Args { get; set; }

        public GameCommandInput(string commandName)
        {
            Command = commandName;
            Args = new Dictionary<string, string>();
        }
    }
}
