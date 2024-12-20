using KihonEngine.Core.State;

namespace KihonEngine.Core.Server.Commands
{
    internal class ExitCommand : GameCommandBase
    {
        public override bool ValidateParameters(GameCommandInput cmd)
        {
            return true;
        }

        public override GameCommandResult Execute(GameState gameState, Player player, int peerId)
        {
            Console.WriteLine($"[Server] Player {player.Name} requested to stop. Remove from game.");
            gameState.Players.Remove(peerId);

            return new GameCommandResult
            {
                StateUpdated = true,
                PeerInitializated = false,
            };
        }
    }
}
