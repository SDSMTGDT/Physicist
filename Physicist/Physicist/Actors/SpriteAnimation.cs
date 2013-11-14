namespace Physicist.Actors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;

    public struct SpriteAnimation
    {
        public uint RowIndex;
        public uint NumFrames;
        public float DefaultFrameRate;

        public SpriteAnimation(uint rowIndex, uint numFrames, float defaultFrameRate)
        {
            this.RowIndex = rowIndex;
            this.NumFrames = numFrames;
            this.DefaultFrameRate = defaultFrameRate;
        }
    }
}
