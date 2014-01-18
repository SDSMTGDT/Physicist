namespace Physicist.Actors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Physicist.Enums;
    using Physicist.Extensions;

    public class GameSprite
    {
        // animation fields
        private float frameLength = 0.1f;
        private uint currentFrame = 0;
        private string currentAnimationString;
        private Dictionary<string, SpriteAnimation> animations;
        private float markedTime = 0;
        private float depth = 0f;

        public GameSprite(Texture2D spriteSheet, Size frameSize)
        {
            this.SpriteSheet = spriteSheet;
            this.FrameSize = frameSize;
            this.FrameLength = 0.2f;
            this.animations = new Dictionary<string, SpriteAnimation>((int)(this.SpriteSheet.Height / this.FrameSize.Height));
        }

        // sprite properties
        public Texture2D SpriteSheet { get; private set; }

        public Size FrameSize { get; private set; }

        public Vector2 Offset { get; set; }

        public List<string> AnimationKeys
        {
            get
            {
                return new List<string>(this.animations.Keys);
            }
        }

        public float Depth
        {       
            get
            {
                return this.depth;
            }

            set
            {
                this.depth = MathHelper.Clamp(value, 0f, 1f);
            }
        }

        public Rectangle CurrentSprite
        {
            get
            {              
                return new Rectangle(
                    (int)(this.CurrentFrame * this.FrameSize.Width), 
                    (int)(this.CurrentAnimationIndex * this.FrameSize.Height), 
                    (int)this.FrameSize.Width, 
                    (int)this.FrameSize.Height);
            }
        }

        public string CurrentAnimationString
        {
            get
            {
                return this.currentAnimationString;
            }

            set
            {
                if (this.animations.ContainsKey(value))
                {
                    this.currentAnimationString = value;
                }
            }
        }
        
        // animation state variables
        public SpriteAnimation CurrentAnimation 
        {
            get
            {
                return this.animations[this.CurrentAnimationString];
            }
        }

        public uint CurrentAnimationIndex
        {
            get
            {
                return this.animations[this.CurrentAnimationString].RowIndex;
            }
        }

        public uint AnimationCount
        {
            get
            {
                return (uint)this.animations.Values.Count;
            }
        }
        
        public uint MaxFrames
        {
            get
            {
                return this.CurrentAnimation.FrameCount;
            }
        }

        public float FrameLength
        {
            get
            {
                return this.frameLength;
            }

            set
            {
                if (value >= 0)
                {
                    this.frameLength = value;
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
            if (time != null)
            {
                this.markedTime += time.ElapsedGameTime.Milliseconds / 1000.0f;

                // if the elapsed time since the last frame change indicates that it is time to animate the sprite, do so.
                if (this.markedTime > this.FrameLength)
                {
                    this.CurrentFrame = (this.CurrentFrame + 1) % this.MaxFrames;
                    this.markedTime = 0;
                }
            }
        }

        public void AddAnimation(string animationName, SpriteAnimation animation)
        {
            this.animations.Add(animationName, animation);
        }

        public void AddAnimation(Enum animationName, SpriteAnimation animation)
        {
            if (animationName != null && animation != null)
            {
                this.AddAnimation(animationName.ToString(), animation);
            }
        }

        public void ChangeAnimation(string animationName, SpriteAnimation animation)
        {
            this.animations[animationName] = animation;
        }
    }
}
