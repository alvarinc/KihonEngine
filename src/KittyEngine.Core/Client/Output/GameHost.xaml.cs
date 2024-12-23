﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace KittyEngine.Core.Client.Outputs
{
    /// <summary>
    /// Interaction logic for GameHost.xaml
    /// </summary>
    public partial class GameHost : UserControl, IGameHost
    {
        //private IGameStateClient State
        //    => Container.Get<IGameStateClient>();

        //private IOutputFactory OutputFactory
        //    => Container.Get<IOutputFactory>();

        //private IKeyboardListener KeyboardListener
        //    => Container.Get<IKeyboardListener>();

        //private IMouseListener MouseListener
        //    => Container.Get<IMouseListener>();

        //private GameMode? _previousGameMode;

        private ISynchronizableOutput _hud;

        public GameHost()
        {
            InitializeComponent();
            Focusable = true;

            if (string.IsNullOrEmpty(Thread.CurrentThread.Name))
            {
                Thread.CurrentThread.Name = "UIThread";
            }
        }

        public void AttachViewport(Viewport3D viewport)
        {
            gameViewportHost.Children.Clear();
            gameViewportHost.Children.Add(viewport);

            Synchronize();
        }

        public void Synchronize()
        {
            //if (_previousGameMode.HasValue && _previousGameMode == State.Shared.Mode)
            //{
            //    if (State.Shared.Mode == GameMode.ServerLoading || State.Shared.Mode == GameMode.ServerLoaded)
            //    {
            //        var synchronizable = menuHost.Children[0] as ISynchronizableOutput;
            //        if (synchronizable != null)
            //        {
            //            synchronizable.Synchronize();
            //        }
            //    }

            //    return;
            //}

            //ImageSource inGameScreenshot = null;
            //if (State.Shared.Mode == GameMode.InGameMenu)
            //{
            //    inGameScreenshot = TakeScreenshot();
            //}

            menuHost.Visibility = Visibility.Collapsed;
            gameViewportHost.Visibility = Visibility.Collapsed;
            gameHeadUpDisplayHost.Visibility = Visibility.Collapsed;

            //switch (State.Shared.Mode)
            //{
            //    case GameMode.Startup:
            //        _hud = null;
            //        menuHost.Children.Clear();
            //        menuHost.Children.Add(OutputFactory.CreateOutput(OutputTypes.StartupScreen));
            //        menuHost.Visibility = Visibility.Visible;
            //        break;
            //    case GameMode.OutGameMenu:
            //        _hud = null;
            //        menuHost.Children.Clear();
            //        menuHost.Children.Add(OutputFactory.CreateOutput(OutputTypes.OutGameMenu));
            //        menuHost.Visibility = Visibility.Visible;
            //        break;
            //    case GameMode.ServerLoading:
            //    case GameMode.ServerLoaded:
            //        if (_previousGameMode == GameMode.ServerLoading || _previousGameMode == GameMode.ServerLoaded)
            //        {
            //            menuHost.Visibility = Visibility.Visible;
            //            var synchronizable = menuHost.Children[0] as ISynchronizableOutput;
            //            if (synchronizable != null)
            //            {
            //                synchronizable.Synchronize();
            //            }

            //            break;
            //        }

            //        _hud = null;
            //        menuHost.Children.Clear();
            //        menuHost.Children.Add(OutputFactory.CreateOutput(OutputTypes.LoadScreen));
            //        menuHost.Visibility = Visibility.Visible;
            //        break;
            //    case GameMode.Play:
                    menuHost.Children.Clear();
                    gameViewportHost.Visibility = Visibility.Visible;

                    gameHeadUpDisplayHost.Children.Clear();
                    _hud = null;

                    //if (State.HUD.IsEnabled)
                    //{
                    //    var hudControl = OutputFactory.CreateOutput(OutputTypes.HeadUpDisplay);
                    //    _hud = hudControl as ISynchronizableOutput;
                    //    gameHeadUpDisplayHost.Children.Add(hudControl);
                    //    gameHeadUpDisplayHost.Visibility = Visibility.Visible;
                    //}
                    Focus();
            //        break;
            //    case GameMode.InGameMenu:
            //        menuHost.Children.Clear();
            //        var inGameMenu = OutputFactory.CreateOutput(OutputTypes.InGameMenu);
            //        var imageMenu = inGameMenu as Menus.InGameMenu.IImageMenu;
            //        if (imageMenu != null)
            //        {
            //            imageMenu.InGameScreenShot = inGameScreenshot;
            //        }

            //        menuHost.Children.Add(inGameMenu);
            //        menuHost.Visibility = Visibility.Visible;
            //        break;
            //    case GameMode.Exit:
            //        _hud = null;
            //        menuHost.Children.Clear();
            //        menuHost.Children.Add(OutputFactory.CreateOutput(OutputTypes.ExitScreen));
            //        break;
            //}

            //State.HUD.Control = _hud;
            //_previousGameMode = State.Shared.Mode;
            Focus();
        }

        private ImageSource TakeScreenshot()
        {
            var screenshoot = new RenderTargetBitmap(
               (int)canvas.ActualWidth,
               (int)canvas.ActualHeight, 96, 96, PixelFormats.Pbgra32);

            screenshoot.Render(canvas);

            return screenshoot;
        }

        private void UserControl_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //if (e.Key == System.Windows.Input.Key.F11)
            //{
            //    var parentWindow = Window.GetWindow(this);
            //    if (State.Client.IsInFullScreenMode)
            //    {
            //        MouseListener.IsEnabled = false;
            //        parentWindow.WindowStyle = State.Client.NormalWindowsStyle;
            //        parentWindow.Topmost = State.Client.NormalTopmost;
            //        parentWindow.WindowState = State.Client.NormalWindowsState;
            //        State.Client.IsInFullScreenMode = false;
            //        MouseListener.Reset();
            //        MouseListener.IsEnabled = true;
            //    }
            //    else
            //    {
            //        MouseListener.IsEnabled = false;
            //        State.Client.NormalWindowsStyle = parentWindow.WindowStyle;
            //        State.Client.NormalWindowsState = parentWindow.WindowState;
            //        State.Client.NormalTopmost = parentWindow.Topmost;
            //        parentWindow.WindowStyle = WindowStyle.None;
            //        parentWindow.Topmost = true;
            //        parentWindow.WindowState = WindowState.Normal;
            //        parentWindow.WindowState = WindowState.Maximized;
            //        State.Client.IsInFullScreenMode = true;
            //        MouseListener.Reset();
            //        MouseListener.IsEnabled = true;
            //    }
            //}
            //else if (e.Key == System.Windows.Input.Key.F12)
            //{
            //    if (terminalHost.Children.Count > 0)
            //    {
            //        terminalHost.Children.Clear();
            //        Focus();
            //        KeyboardListener.IsEnabled = true;
            //    }
            //    else if (State.Shared.Terminal.IsEnabled)
            //    {
            //        KeyboardListener.IsEnabled = false;
            //        var control = new TerminalControl();
            //        control.Height = ActualHeight / 2 - 10;
            //        terminalHost.Children.Add(control);
            //    }
            //}
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //if (terminalHost.Children.Count > 0)
            //{
            //    var control = terminalHost.Children[0] as TerminalControl;
            //    control.Height = ActualHeight / 2 - 10;
            //}
        }
    }
}
