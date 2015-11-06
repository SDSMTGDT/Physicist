namespace Physicist.Types.Util
{
    using System;
    using Microsoft.Xna.Framework;

    public class LayerTransitionEventArgs : EventArgs
    {
        public LayerTransitionEventArgs(Vector2 position, string targetDoor)
        {
            this.Position = position;
            this.TargetDoor = targetDoor;
        }

        public Vector2 Position { get; private set; }

        public string TargetDoor { get; private set; }
    }
}
