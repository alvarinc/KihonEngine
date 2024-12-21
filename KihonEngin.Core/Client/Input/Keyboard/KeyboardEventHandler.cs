using KihonEngine.Core.Server;

namespace KihonEngine.Core.Client.Input.Keyboard
{
    public class KeyboardEventHandler
    {
        private List<IKeyboardEventHandler> _handlers = new List<IKeyboardEventHandler>();

        public KeyboardEventHandler() 
        {
            _handlers.Add(new ExitEventHandler());
            _handlers.Add(new MoveEventHandler());
        }

        public List<GameCommandInput> HandleEvents()
        {
            var results = new List<GameCommandInput>();
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
                string keyPressed = keyInfo.Key.ToString();

                var e = new InputEventArgument();
                OnKeyboardEvent(keyPressed, e);
                results = e.Inputs;
            }

            return results;
        }

        private void OnKeyboardEvent(string keyPressed, InputEventArgument e)
        {
            foreach(var handler in _handlers)
            {
                handler.OnKeyboardEvent(keyPressed, e);
                if (e.Inputs.Count != 0)
                {
                    break;
                }
            }
        }
    }
}
