namespace Physicist.Control
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework.Input;
    using Physicist.Enums;

    public static class KeyboardController
    {
        private static Dictionary<string, Keys> mappedKeys = null;

        public static Keys UpKey
        {
            get
            {
                return KeyboardController.MappedKeys[StandardKeyAction.Up.ToString()];
            }
        }

        public static Keys DownKey
        {
            get
            {
                return KeyboardController.MappedKeys[StandardKeyAction.Down.ToString()];
            }
        }

        public static Keys RightKey
        {
            get
            {
                return KeyboardController.MappedKeys[StandardKeyAction.Right.ToString()];
            }
        }

        public static Keys LeftKey
        {
            get
            {
                return KeyboardController.MappedKeys[StandardKeyAction.Left.ToString()];
            }
        }

        public static Keys JumpKey
        {
            get
            {
                return KeyboardController.MappedKeys[StandardKeyAction.Jump.ToString()];
            }
        }

        private static Dictionary<string, Keys> MappedKeys
        {
            get
            {
                if (KeyboardController.mappedKeys == null)
                {
                    KeyboardController.mappedKeys = new Dictionary<string, Keys>();
                    KeyboardController.mappedKeys.Add(StandardKeyAction.Up.ToString(), Keys.Up);
                    KeyboardController.mappedKeys.Add(StandardKeyAction.Down.ToString(), Keys.Down);
                    KeyboardController.mappedKeys.Add(StandardKeyAction.Left.ToString(), Keys.Left);
                    KeyboardController.mappedKeys.Add(StandardKeyAction.Right.ToString(), Keys.Right);
                    KeyboardController.mappedKeys.Add(StandardKeyAction.Jump.ToString(), Keys.Space);
                }

                return KeyboardController.mappedKeys;
            }
        }

        public static bool TrySetKey(string keyAction, Keys newKey)
        {
            bool canset = KeyboardController.mappedKeys.ContainsKey(keyAction) && !KeyboardController.mappedKeys.ContainsValue(newKey);
            if (canset)
            {
                KeyboardController.mappedKeys[keyAction] = newKey;
            }

            return canset;
        }
    }
}
