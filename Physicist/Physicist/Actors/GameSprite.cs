namespace Physicist.Actors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class GameSprite
    {
        // animation fields
        private float frameRate = 0;
        private uint currentFrame = 0;
        private uint currentAnimationIndex = 0;
        private List<SpriteAnimation> animations;
        private float markedTime = 0;

        public GameSprite(Texture2D spriteSheet, Vector2 frameSize, float frameRate = 0)
        {
            this.SpriteSheet = spriteSheet;
            this.FrameSize = frameSize;
            this.FrameRate = frameRate;
            this.animations = new List<SpriteAnimation>((int)(this.SpriteSheet.Height / this.FrameSize.Y));
        }

        // sprite properties
        public Texture2D SpriteSheet { get; private set; }

        public Vector2 FrameSize { get; private set; }

        public Rectangle CurrentSprite
        {
            get
            {
                return new Rectangle((int)(this.CurrentFrame * this.FrameSize.X), (int)(this.CurrentAnimationIndex * this.FrameSize.Y), (int)this.FrameSize.X, (int)this.FrameSize.Y);
            }
        }

        // animation state variables
        public SpriteAnimation CurrentAnimation 
        {
            get
            {
                return this.animations.ElementAt((int)this.CurrentAnimationIndex);
            }
        }

        public uint CurrentAnimationIndex
        {
            get
            {
                return this.currentAnimationIndex;
            }

            set
            { 
                if (value < this.NumAnimations)
                {
                    this.currentAnimationIndex = value;
                }
            }
        }

        public uint NumAnimations
        {
            get
            {
                return (uint)this.animations.Count;
            }
        }
        
        public uint MaxFrames
        {
            get
            {
                return this.CurrentAnimation.NumFrames;
            }
        }

        public float FrameRate
        {
            get
            {
                return this.frameRate;
            }

            set
            {
                if (value >= 0)
                {
                    this.frameRate = value;
                }
            }
        }

        public uint CurrentFrame
        {
            get
            {
                return this.currentFrame;
            }

            set
            {
                if (value < this.MaxFrames)
                {
                    this.currentFrame = value;
                }
            }
        }
        
        public void Update(GameTime time)
        {
            this.markedTime += time.ElapsedGameTime.Milliseconds / 1000.0f;
     
            // if the elapsed time since the last frame change indicates that it is time to animate the sprite, do so.
            if (this.markedTime > this.FrameRate)
            {
                this.CurrentFrame = (this.CurrentFrame + 1) % this.MaxFrames;
                this.markedTime = 0;
            }
        }

        public void AddAnimation(SpriteAnimation animation)
        {
            this.animations.Add(animation);
        }

        public void ChangeAnimation(uint animationIndex, SpriteAnimation animation)
        {
            this.animations[(int)animationIndex] = animation;
        }
    }
}
