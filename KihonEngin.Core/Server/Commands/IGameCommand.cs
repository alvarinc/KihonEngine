using KihonEngine.Core.State;

namespace KihonEngine.Core.Server.Commands
{
    internal interface IGameCommand
    {
        public bool ValidateParameters(GameCommandInput cmd);
        public GameCommandResult Execute(GameState gameState, Player player, int peerId);
    }
}
