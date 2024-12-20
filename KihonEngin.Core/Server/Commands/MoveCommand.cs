using KihonEngine.Core.State;

namespace KihonEngine.Core.Server.Commands
{
    internal class MoveCommand : GameCommandBase
    {
        private float _dx;
        private float _dy;
        private float _dz;

        public override bool ValidateParameters(GameCommandInput input)
        {
            var dxString = input.Args["dx"];
            var dyString = input.Args["dy"];
            var dzString = input.Args["dz"];

            return
                float.TryParse(dxString, out _dx) &&
                float.TryParse(dyString, out _dy) &&
                float.TryParse(dzString, out _dz);
        }

        public override GameCommandResult Execute(GameState gameState, Player player, int peerId)
        {
            var playerState = gameState.Players[peerId];
            var results = new GameCommandResult();
            if (_dx != 0 && playerState.Position.X + _dx >= gameState.Map.MinX && playerState.Position.X + _dx <= gameState.Map.MaxX)
            {
                playerState.Position.X += _dx;
                results.StateUpdated = true;
            }

            if (_dy != 0 && playerState.Position.Y + _dy >= gameState.Map.MinY && playerState.Position.Y + _dy <= gameState.Map.MaxY)
            {
                playerState.Position.Y += _dy;
                results.StateUpdated = true;
            }

            if (_dz != 0 && playerState.Position.Z + _dz >= gameState.Map.MinZ && playerState.Position.Z + _dz <= gameState.Map.MaxZ)
            {
                playerState.Position.Z += _dz;
                results.StateUpdated = true;
            }

            if (results.StateUpdated)
            {
                Console.WriteLine($"[Server] Player {playerState.Name} moved to: {playerState.Position.X}:{playerState.Position.Y}:{playerState.Position.Z}");
            }

            return results;
        }
    }
}
