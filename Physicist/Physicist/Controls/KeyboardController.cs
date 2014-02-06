namespace Physicist.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework.Input;
    using Physicist.Enums;

    public static class KeyboardController
    {
        private static Dictionary<StandardKeyAction, Keys> mappedKeys = null;

        public static Keys UpKey
        {
            get
            {
                return KeyboardController.MappedKeys[StandardKeyAction.Up];
            }
        }

        public static Keys DownKey
        {
            get
            {
                return KeyboardController.MappedKeys[StandardKeyAction.Down];
            }
        }

        public static Keys RightKey
        {
            get
            {
                return KeyboardController.MappedKeys[StandardKeyAction.Right];
            }
        }

        public static Keys LeftKey
        {
            get
            {
                return KeyboardController.MappedKeys[StandardKeyAction.Left];
            }
        }

        public static Keys JumpKey
        {
            get
            {
                return KeyboardController.MappedKeys[StandardKeyAction.Jump];
            }
        }

        private static Dictionary<StandardKeyAction, Keys> MappedKeys
        {
            get
            {
                if (KeyboardController.mappedKeys == null)
                {
                    KeyboardController.mappedKeys = new Dictionary<StandardKeyAction, Keys>();
                    KeyboardController.mappedKeys.Add(StandardKeyAction.Up, Keys.Up);
                    KeyboardController.mappedKeys.Add(StandardKeyAction.Down, Keys.Down);
                    KeyboardController.mappedKeys.Add(StandardKeyAction.Left, Keys.Left);
                    KeyboardController.mappedKeys.Add(StandardKeyAction.Right, Keys.Right);
                    KeyboardController.mappedKeys.Add(StandardKeyAction.Jump, Keys.Space);
                }

                return KeyboardController.mappedKeys;
            }
        }

        public static bool TrySetKey(StandardKeyAction keyAction, Keys newKey)
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
