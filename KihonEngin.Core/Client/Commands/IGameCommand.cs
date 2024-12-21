using KihonEngine.Core.Server;

namespace KihonEngine.Core.Client.Commands
{
    internal interface IGameCommand
    {
        public bool ValidateParameters(GameCommandInput cmd);
        public void Execute(GameCommandContext context);
    }
}
