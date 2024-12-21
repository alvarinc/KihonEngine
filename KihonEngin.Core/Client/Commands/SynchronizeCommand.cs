using KihonEngine.Core.Server;
using KihonEngine.Core.State;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;

namespace KihonEngine.Core.Client.Commands
{
    internal class SynchronizeCommand : IGameCommand
    {
        private string _entity;
        private string _mode;
        private string _value;

        public bool ValidateParameters(GameCommandInput cmd)
        {
            _entity = cmd.Args["entity"];
            _mode = cmd.Args["mode"];
            _value = cmd.Args["value"];

            return true;
        }

        public void Execute(GameCommandContext context)
        {
            if (_mode == "full")
            {
                context.GameState = JsonConvert.DeserializeObject<GameState>(_value);
                context.StateUpdated = true;
            }
            else if (_mode == "patch")
            {
                var jsonPatch = JsonConvert.DeserializeObject<JsonPatchDocument>(_value);
                jsonPatch.ApplyTo(context.GameState);
                context.StateUpdated = true;
            }

            var player = context.GameState?.Players.Values.FirstOrDefault(x => x.Guid == context.PlayerId);
            if (player != null)
            {
                Console.WriteLine($"[Client] Connected. Position: {player.Position.X}, {player.Position.Y}, {player.Position.Z}");
            }
            else
            {
                Console.WriteLine($"[Client] Connected. No position yet.");
            }
        }
    }
}
