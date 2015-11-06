namespace Physicist.Types.Controllers
{
    using Microsoft.Xna.Framework.Input;

    public class DebouncerKeyState
    {
        public KeyState PreviousState { get; set; }

        public bool IsPressed { get; set; }
    }
}
