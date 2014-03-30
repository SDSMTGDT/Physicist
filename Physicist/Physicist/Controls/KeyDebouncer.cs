namespace Physicist.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Xna.Framework.Input;

    public class KeyDebouncer
    {
        public KeyState PreviousState { get; set; }

        public bool IsPressed { get; set; }
    }
}
