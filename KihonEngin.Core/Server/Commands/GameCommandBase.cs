using KihonEngine.Core.State;

namespace KihonEngine.Core.Server.Commands
{
    internal abstract class GameCommandBase : IGameCommand
    {
        public abstract bool ValidateParameters(GameCommandInput input);
        public abstract GameCommandResult Execute(GameState gameState, Player player, int peerId);
    }
}
