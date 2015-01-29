namespace Physicist.Controls
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework.Input;

    public class MouseDebouncer
    {
        private Dictionary<string, DebouncerKeyState> buttons = new Dictionary<string, DebouncerKeyState>();

        public MouseDebouncer()
        {
            this.buttons.Add("left", new DebouncerKeyState() { IsPressed = false, PreviousState = KeyState.Down });
            this.buttons.Add("right", new DebouncerKeyState() { IsPressed = false, PreviousState = KeyState.Down });
            this.buttons.Add("middle", new DebouncerKeyState() { IsPressed = false, PreviousState = KeyState.Down });
            this.buttons.Add("xbutton1", new DebouncerKeyState() { IsPressed = false, PreviousState = KeyState.Down });
            this.buttons.Add("xbutton2", new DebouncerKeyState() { IsPressed = false, PreviousState = KeyState.Down });
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Extension of Monogame")]
        public ButtonState LeftButton
        {
            get
            {
                return Mouse.GetState().LeftButton;
            }
        }

        public ButtonState LeftButtonDebounce
        {
            get
            {
                return this.buttons["left"].IsPressed ? ButtonState.Pressed : ButtonState.Released;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Extension of Monogame")]
        public ButtonState RightButton
        {
            get
            {
                return Mouse.GetState().RightButton;
            }
        }

        public ButtonState RightButtonDebounce
        {
            get
            {
                return this.buttons["right"].IsPressed ? ButtonState.Pressed : ButtonState.Released;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Extension of Monogame")]
        public ButtonState MiddleButton
        {
            get
            {
                return Mouse.GetState().MiddleButton;
            }
        }

        public ButtonState MiddleButtonDebounce
        {
            get
            {
                return this.buttons["middle"].IsPressed ? ButtonState.Pressed : ButtonState.Released;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Extension of Monogame")]
        public int ScrollWheelValue
        {
            get
            {
                return Mouse.GetState().ScrollWheelValue;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Extension of Monogame")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Extension of Monogame")]
        public int X
        {
            get
            {
                return Mouse.GetState().X;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Extension of Monogame")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Extension of Monogame")]
        public int Y
        {
            get
            {
                return Mouse.GetState().Y;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Extension of Monogame")]
        public ButtonState XButton1
        {
            get
            {
                return Mouse.GetState().XButton1;
            }
        }

        public ButtonState XButton1Debounce
        {
            get
            {
                return this.buttons["xbutton1"].IsPressed ? ButtonState.Pressed : ButtonState.Released;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Extension of Monogame")]
        public ButtonState XButton2
        {
            get
            {
                return Mouse.GetState().XButton2;
            }
        }

        public ButtonState XButton2Debounce
        {
            get
            {
                return this.buttons["xbutton2"].IsPressed ? ButtonState.Pressed : ButtonState.Released;
            }
        }

        public void UpdateButtons()
        {
            foreach (var button in this.buttons.Values)
            {
                if (button.PreviousState == KeyState.Up)
                {
                    button.IsPressed = false;
                }
                else
                {
                    button.IsPressed = true;
                    button.PreviousState = KeyState.Up;
                }
            }

            if (this.LeftButton != ButtonState.Pressed)
            {
                this.buttons["left"].IsPressed = false;
                this.buttons["left"].PreviousState = KeyState.Down;
            }

            if (this.RightButton != ButtonState.Pressed)
            {
                this.buttons["right"].IsPressed = false;
                this.buttons["right"].PreviousState = KeyState.Down;
            }

            if (this.MiddleButton != ButtonState.Pressed)
            {
                this.buttons["middle"].IsPressed = false;
                this.buttons["middle"].PreviousState = KeyState.Down;
            }

            if (this.XButton1 != ButtonState.Pressed)
            {
                this.buttons["xbutton1"].IsPressed = false;
                this.buttons["xbutton1"].PreviousState = KeyState.Down;
            }

            if (this.XButton2 != ButtonState.Pressed)
            {
                this.buttons["xbutton2"].IsPressed = false;
                this.buttons["xbutton2"].PreviousState = KeyState.Down;
            }
        }
    }
}
