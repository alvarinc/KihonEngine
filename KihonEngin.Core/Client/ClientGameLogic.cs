namespace KihonEngine.Core.Client
{
    using KihonEngine.Core.Client.Commands;
    using KihonEngine.Core.Client.Input.Keyboard;
    using KihonEngine.Core.Server;
    using KihonEngine.Core.State;

    internal class ClientGameLogic
    {
        private string _playerId;
        private GameState _gameState = null;
        private bool _gameStateUpdated = false;
        private KeyboardEventHandler _keyboardEventHandler;

        public ClientGameLogic()
        {
            _keyboardEventHandler = new KeyboardEventHandler();
        }

        public void ViewAs(string playerId)
        {
            _playerId = playerId;
        }

        public List<GameCommandInput> HandleInputEvents()
        {
            return _keyboardEventHandler.HandleEvents();
        }

        public void HandleServerMessage(GameCommandInput input)
        {
            IGameCommand cmd = null;
            if (input.Command == "sync")
            {
                cmd = new SynchronizeCommand();
            }

            if (cmd != null)
            {
                if (cmd.ValidateParameters(input))
                {
                    var context = new GameCommandContext { GameState = _gameState, PlayerId = _playerId };
                    cmd.Execute(context);

                    if (context.StateUpdated)
                    {
                        _gameState = context.GameState;
                        _gameStateUpdated |= context.StateUpdated;
                    }
                }
            }
        }

        public void RenderOutput(bool force = false)
        {
            if (force || _gameStateUpdated)
            {
                ClientRenderer.Render(_gameState, _playerId);
                _gameStateUpdated = false;
            }
        }
    }
}
