namespace Physicist.Controls
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    public static class MouseController
    {
        private static MouseDebouncer mouseDebouncer = new MouseDebouncer();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Justification = "Extension of Monogame")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Extension of Monogame")]
        public static bool GetCursorPos(out Mouse.POINT point)
        {
            return Mouse.GetCursorPos(out point);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Extension of Monogame")]
        public static bool SetCursorPos(int x, int y)
        {
            return Mouse.SetCursorPos(x, y);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", Justification = "Extension of Monogame")]
        public static void SetPosition(int x, int y)
        {
            Mouse.SetPosition(x, y);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Consistent from XNA framework structure")]
        public static MouseDebouncer GetState()
        {
            return MouseController.mouseDebouncer;
        }

        public static void Update(GameTime gameTime)
        {
            if (gameTime != null)
            {
                MouseController.mouseDebouncer.UpdateButtons();
            }
        }
    }
}
