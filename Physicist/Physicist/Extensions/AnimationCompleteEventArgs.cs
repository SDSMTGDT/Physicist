namespace Physicist.Extensions
{
    using System;
    using Physicist.Actors;

    public class AnimationCompleteEventArgs : EventArgs
    {
        public AnimationCompleteEventArgs(SpriteAnimation animation)
        {
            this.Animation = animation;
        }

        public SpriteAnimation Animation { get; private set; }        
    }
}
