using KihonEngine.Core.Server;

namespace KihonEngine.Core.Client.Input.Keyboard
{
    internal class ExitEventHandler : IKeyboardEventHandler
    {
        public void OnKeyboardEvent(string keyPressed, InputEventArgument e)
        {
            if (keyPressed == "Escape")
            {
                Console.WriteLine("[Client] ESC key pressed. Stopping client...");
                e.Inputs.Add(new GameCommandInput("exit"));
            }
        }
    }
}
