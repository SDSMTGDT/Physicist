﻿namespace Physicist.Types.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Physicist.Types.Controllers;
    using Physicist.Types.Interfaces;
    using Physicist.Types.Enums;
    using Physicist.Types.Util;

    public class GameSprite : IXmlSerializable
    {
        // animation fields
        private float frameLength = 0.2f;
        private uint currentFrame = 0;
        private string currentAnimationString = string.Empty;
        private Dictionary<string, SpriteAnimation> animations;
        private float markedTime = 0f;
        private float depth = 0f;

        public GameSprite()
        {
        }

        public GameSprite(Texture2D spriteSheet, Size frameSize, string spriteName)
        {
            this.SpriteSheet = spriteSheet;
            this.FrameSize = frameSize;
            this.animations = new Dictionary<string, SpriteAnimation>((int)(this.SpriteSheet.Height / this.FrameSize.Height));
            this.Offset = Vector2.Zero;
            this.SpriteName = spriteName;
        }

        public event EventHandler<AnimationCompleteEventArgs> AnimationComplete;

        // sprite properties
        public Texture2D SpriteSheet { get; private set; }

        public Size FrameSize { get; private set; }

        public Vector2 Offset { get; set; }

        public string SpriteName { get; set; }

        public IEnumerable<string> AnimationKeys
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
                if (string.IsNullOrEmpty(this.currentAnimationString) && this.animations.Count > 0)
                {
                    this.currentAnimationString = this.animations.ElementAt(0).Key;
                }

                return this.currentAnimationString;
            }

            set
            {
                if (this.animations.ContainsKey(value))
                {
                    this.currentAnimationString = value;
                    this.Reset();
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

        public bool AnimationFinished
        {
            get;
            private set;
        }

        public void Reset()
        {
            this.CurrentFrame = this.CurrentAnimation.PlayInReverse ? this.MaxFrames - 1 : 0;
            this.AnimationFinished = false;
        }

        public void Update(GameTime time)
        {
            if (time != null)
            {
                this.markedTime += time.ElapsedGameTime.Milliseconds / 1000.0f;

                bool reachedEnd = this.CurrentAnimation.PlayInReverse ? this.CurrentFrame == 0 : this.CurrentFrame == this.MaxFrames - 1;

                // if the elapsed time since the last frame change indicates that it is time to animate the sprite, do so.
                if (this.markedTime > (this.FrameLength > 0 ? this.FrameLength : this.CurrentAnimation.DefaultFrameRate))
                {
                    if (this.CurrentAnimation.LoopAnimation || !reachedEnd)
                    {
                        if (this.CurrentAnimation.PlayInReverse)
                        {
                            int result = (int)((int)this.CurrentFrame - 1);
                            if (result < 0)
                            {
                                result += (int)this.MaxFrames;
                            }

                            this.CurrentFrame = (uint)result;
                        }
                        else
                        {
                            this.CurrentFrame = (this.CurrentFrame + 1) % this.MaxFrames;
                        }
                    }
                    else
                    {
                        if (this.AnimationComplete != null)
                        {
                            this.AnimationFinished = true;
                            this.AnimationComplete(this, new AnimationCompleteEventArgs(this.CurrentAnimation));
                        }
                    }

                    this.markedTime = 0;
                }
            }
        }

        public void AddAnimation(string animationName, SpriteAnimation animation)
        {
            this.animations.Add(animationName, animation);
        }

        public void AddAnimation(StandardAnimation animationName, SpriteAnimation animation)
        {
            if (animation != null)
            {
                this.AddAnimation(animationName.ToString(), animation);
            }
        }

        public void ChangeAnimation(string animationName, SpriteAnimation animation)
        {
            this.animations[animationName] = animation;
        }

        public XElement XmlSerialize()
        {
            XElement spriteElement = new XElement(
                "GameSprite",
                new XAttribute("spriteName", this.SpriteName),
                this.Offset.XmlSerialize("Offset"),
                new XAttribute("textureRef", ContentController.Instance.GetMediaReference<Texture2D>(this.SpriteSheet).Name),
                new XAttribute("frameLength", this.frameLength),
                new XAttribute("depth", this.depth),
                this.FrameSize.XmlSerialize("FrameSize"),
                new XElement(
                    "Animations",
                    this.animations.Select(animation => animation.Value.XmlSerialize(animation.Key))));

            return spriteElement;
        }

        public void XmlDeserialize(XElement element)
        {
            if (element != null)
            {
                // Pull Texture2D information
                this.SpriteSheet = ContentController.Instance.GetContent<Texture2D>(element.Attribute("textureRef").Value);

                // Pull Size information from the framsize element
                this.FrameSize = XmlDeserializeHelper.XmlDeserialize<Size>(element.Element("FrameSize"));

                // Create animation dictionay
                this.animations = new Dictionary<string, SpriteAnimation>((int)(this.SpriteSheet.Height / this.FrameSize.Height));

                this.frameLength = element.GetAttribute("frameLength", -1f);
                this.depth = element.GetAttribute("depth", 0.9f);

                // Get the name
                this.SpriteName = element.GetAttribute("spriteName", string.Empty);

                // Get the offset
                this.Offset = XmlDeserializeHelper.XmlDeserialize<Vector2>(element.Element("Offset"));

                // Create SpriteAnimations out of the Deserialze functions in SpriteAnimation
                var animationsElement = element.Element("Animations");
                if (animationsElement != null)
                {
                    foreach (XElement animationEle in animationsElement.Elements())
                    {
                        string name = animationEle.GetAttribute<string>("name", animationEle.Name.LocalName);

                        this.AddAnimation(name, XmlDeserializeHelper.XmlDeserialize<SpriteAnimation>(animationEle));
                    }
                }
            }
        }
    }
}
