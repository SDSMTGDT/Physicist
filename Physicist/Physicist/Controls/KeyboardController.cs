namespace Physicist.Controls
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using Physicist.Controls;
    using Physicist.Enums;

    public static class KeyboardController
    {
        private static Dictionary<StandardKeyAction, Keys> mappedKeys = null;

        private static KeyboardDebouncer keyDebouncer = new KeyboardDebouncer();

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

        public static Keys RotateRightKey
        {
            get
            {
                return KeyboardController.MappedKeys[StandardKeyAction.RotateRight];
            }
        }

        public static Keys RotateLeftKey
        {
            get
            {
                return KeyboardController.MappedKeys[StandardKeyAction.RotateLeft];
            }
        }

        private static Dictionary<StandardKeyAction, Keys> MappedKeys
        {
            get
            {
                if (KeyboardController.mappedKeys == null)
                {
                    KeyboardController.mappedKeys = new Dictionary<StandardKeyAction, Keys>();
                    KeyboardController.mappedKeys.Add(StandardKeyAction.Up, Keys.W);
                    KeyboardController.mappedKeys.Add(StandardKeyAction.Down, Keys.S);
                    KeyboardController.mappedKeys.Add(StandardKeyAction.Left, Keys.A);
                    KeyboardController.mappedKeys.Add(StandardKeyAction.Right, Keys.D);
                    KeyboardController.mappedKeys.Add(StandardKeyAction.Jump, Keys.Space);
                    KeyboardController.mappedKeys.Add(StandardKeyAction.RotateRight, Keys.Right);
                    KeyboardController.mappedKeys.Add(StandardKeyAction.RotateLeft, Keys.Left);
                }

                return KeyboardController.mappedKeys;
            }
        }

        public static bool TrySetKey(StandardKeyAction keyAction, Keys newKey)
        {
            bool canset = KeyboardController.mappedKeys.ContainsKey(keyAction) && !KeyboardController.mappedKeys.ContainsValue(newKey);
            if (canset)
            {
                KeyboardController.MappedKeys[keyAction] = newKey;
            }

            return canset;
        }

        public static Keys KeyForAction(StandardKeyAction keyAction)
        {
            return KeyboardController.MappedKeys[keyAction];
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Consistent from XNA framework structure")]
        public static KeyboardDebouncer GetState()
        {
            return KeyboardController.keyDebouncer;
        }

        public static void Update(GameTime gameTime)
        {
            if (gameTime != null)
            {
                KeyboardController.keyDebouncer.UpdateKeys();
            }
        }
    }
}
