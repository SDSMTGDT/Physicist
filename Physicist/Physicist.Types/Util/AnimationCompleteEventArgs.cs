namespace Physicist.Types.Util
{
    using System;
    using Physicist.Types.Common;

    public class AnimationCompleteEventArgs : EventArgs
    {
        public AnimationCompleteEventArgs(SpriteAnimation animation)
        {
            this.Animation = animation;
        }

        public SpriteAnimation Animation { get; private set; }        
    }
}
