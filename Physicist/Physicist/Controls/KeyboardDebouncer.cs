namespace Physicist.Controls
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Xna.Framework.Input;
    using Physicist.Controls;

    public class KeyboardDebouncer
    {
        private Dictionary<Keys, DebouncerKeyState> trackedKeys = new Dictionary<Keys, DebouncerKeyState>();       

        public bool IsKeyUp(Keys key, bool debounceKey)
        {
            bool isKeyUp = !this.trackedKeys[key].IsPressed;

            if (!debounceKey)
            {
                isKeyUp = Keyboard.GetState().IsKeyUp(key);
            }

            return isKeyUp;
        }

        public bool IsKeyDown(Keys key)
        {
            return this.IsKeyDown(key, false);
        }

        public bool IsKeyDown(Keys key, bool debounceKey)
        {
            if (!this.trackedKeys.ContainsKey(key))
            {
                this.trackedKeys.Add(key, new DebouncerKeyState() { IsPressed = false, PreviousState = KeyState.Down });
            }

            bool isKeyDown = this.trackedKeys[key].IsPressed;

            if (!debounceKey)
            {
                isKeyDown = Keyboard.GetState().IsKeyDown(key);
            }

            return isKeyDown;
        }

        public void UpdateKeys()
        {
            var pressedKeys = Keyboard.GetState().GetPressedKeys();
            foreach (var key in pressedKeys)
            {
                if (!this.trackedKeys.ContainsKey(key))
                {
                    this.trackedKeys.Add(key, new DebouncerKeyState() { IsPressed = true, PreviousState = KeyState.Up });
                }
                else
                {
                    if (this.trackedKeys[key].PreviousState == KeyState.Up)
                    {
                        this.trackedKeys[key].IsPressed = false;
                    }
                    else
                    {
                        this.trackedKeys[key].IsPressed = true;
                        this.trackedKeys[key].PreviousState = KeyState.Up;
                    }
                }
            }

            foreach (var key in this.trackedKeys)
            {
                if (!pressedKeys.Contains(key.Key))
                {
                    this.trackedKeys[key.Key].IsPressed = false;
                    this.trackedKeys[key.Key].PreviousState = KeyState.Down;
                }
            }
        }
    }
}
