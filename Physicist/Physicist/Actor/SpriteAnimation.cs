namespace Physicist.Actors
{
    using System;

    public struct SpriteAnimation 
    {
        private uint rowIndex;
        private uint frameCount;
        private float defaultFrameRate;
        private bool playInReverse;
        private bool flipVertical;
        private bool flipHorizontal;

        public SpriteAnimation(uint rowIndex, uint frameCount, float defaultFrameRate)
        {
            this.rowIndex = rowIndex;
            this.frameCount = frameCount;
            this.defaultFrameRate = defaultFrameRate;
            this.playInReverse = false;
            this.flipVertical = false;
            this.flipHorizontal = false;
        }

        public SpriteAnimation(uint rowIndex, uint frameCount, float defaultFrameRate, bool playInReverse, bool flipVertical, bool flipHorizontal)
        {
            this.rowIndex = rowIndex;
            this.frameCount = frameCount;
            this.defaultFrameRate = defaultFrameRate;
            this.playInReverse = playInReverse;
            this.flipVertical = flipVertical;
            this.flipHorizontal = flipHorizontal;
        }

        public uint RowIndex
        {
            get
            {
                return this.rowIndex;
            }

            set
            {
                this.rowIndex = value;
            }
        }

        public uint FrameCount
        {
            get
            {
                return this.frameCount;
            }

            set
            {
                this.frameCount = value;
            }
        }

        public float DefaultFrameRate
        {
            get
            {
                return this.defaultFrameRate;
            }

            set
            {
                this.defaultFrameRate = value;
            }
        }

        public bool FlipHorizontal
        {
            get
            {
                return this.flipHorizontal;
            }

            set
            {
                this.flipHorizontal = value;
            }
        }

        public bool FlipVertical
        {
            get
            {
                return this.flipVertical;
            }

            set
            {
                this.flipVertical = value;
            }
        }

        public bool PlayInReverse
        {
            get
            {
                return this.playInReverse;
            }

            set
            {
                this.playInReverse = value;
            }
        }

        public static bool operator ==(SpriteAnimation animation1, SpriteAnimation animation2)
        {
            return animation1.RowIndex == animation2.RowIndex && animation1.FrameCount == animation2.FrameCount;
        }

        public static bool operator !=(SpriteAnimation animation1, SpriteAnimation animation2)
        {
            return !(animation1 == animation2);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
