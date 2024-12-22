﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KittyEngine.Core.Client.Input.Keyboard
{
    internal interface IKeyboardEventHandler
    {
        void OnKeyboardEvent(string keyPressed, InputEventArgument e);
    }
}
