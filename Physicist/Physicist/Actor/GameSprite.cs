namespace Physicist.Actors
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Physicist.Enums;
    using Physicist.Extensions;

    public class GameSprite : IXmlSerializable
    {
        // animation fields
        private float frameLength = 0.1f;
        private uint currentFrame = 0;
        private string currentAnimationString;
        private Dictionary<string, SpriteAnimation> animations;
        private float markedTime = 0;
        private float depth = 0f;

        // Note: Empty constructor for use in deserialization only!
        public GameSprite(XElement element)
        {
            this.XmlDeserialize(element);
        }

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

        public XElement XmlSerialize()
        {
            XElement spriteElement = new XElement(XName.Get("GameSprite"));

            XElement offsetElement = new XElement(XName.Get("Offset"));
            XElement animationsElement = new XElement(XName.Get("Animations"));
            XElement frameSizeElement = new XElement(XName.Get("FrameSize"));

            // get the offset
            offsetElement.Add(new XAttribute(XName.Get("X"), this.Offset.X));
            offsetElement.Add(new XAttribute(XName.Get("Y"), this.Offset.Y));
            spriteElement.Add(offsetElement);

            // get the framesize
            frameSizeElement.Add(new XAttribute(XName.Get("width"), this.FrameSize.Width));
            frameSizeElement.Add(new XAttribute(XName.Get("height"), this.FrameSize.Height));
            spriteElement.Add(offsetElement);

            // add every animation to the animations element
            foreach (SpriteAnimation animation in this.animations.Values)
            {
                XElement animationElement = animation.XmlSerialize();
                animationsElement.Add(animationElement);
            }

            spriteElement.Add(animationsElement);

            // Create Texture2D information
            // spriteElement.Add(new XAttribute(XName.Get("TextureReference"), TextureReference ?);
            // TODO: TEXTURE REFERENCE

            // Now create the five attributes
            spriteElement.Add(new XAttribute(XName.Get("frameLength"), this.frameLength));
            spriteElement.Add(new XAttribute(XName.Get("currentFrame"), this.currentFrame));
            spriteElement.Add(new XAttribute(XName.Get("currentAnimationString"), this.currentAnimationString));
            spriteElement.Add(new XAttribute(XName.Get("markedTime"), this.markedTime));
            spriteElement.Add(new XAttribute(XName.Get("depth"), this.depth));

            return spriteElement;
        }

        public void XmlDeserialize(XElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            // 4 elements and 5 attributes in the top level
            //--------------------------------------------
            // Find all child elements
            XElement offsetElement = element.Element("Offset");
            XElement frameSizeElement = element.Element("FrameSize");

            // Get the offset
            this.Offset = new Vector2(
                                    float.Parse(offsetElement.Attribute("X").Value, CultureInfo.CurrentCulture),
                                    float.Parse(offsetElement.Attribute("Y").Value, CultureInfo.CurrentCulture));

            // Find all elements in the Dictionary element
            IEnumerable<XElement> animationElements = element.Descendants("SpriteAnimation");

            // Create SpriteAnimations out of the Deserialze functions in SpriteAnimation
            foreach (XElement animationElement in animationElements)
            {
                SpriteAnimation animation = new SpriteAnimation(animationElement);
                this.AddAnimation(animationElement.Name.ToString(), animation);
            }

            // Pull Size information from the framsize element
            this.FrameSize = new Size(int.Parse(frameSizeElement.Attribute("width").Value, CultureInfo.CurrentCulture), int.Parse(frameSizeElement.Attribute("height").Value, CultureInfo.CurrentCulture));

            // Pull Texture2D information
            string textureReference = element.Attribute("TextureReference").Value;
            //// TODO: USE CONTENT MANAGER TO FIND THE TEXTURE2D AND ASSIGN THE SPRITE SHEET

            // Now find the 5 attributes and assign them
            this.frameLength = float.Parse(element.Attribute("frameLength").Value, CultureInfo.CurrentCulture);
            this.currentFrame = uint.Parse(element.Attribute("currentFrame").Value, CultureInfo.CurrentCulture);
            this.currentAnimationString = element.Attribute("currentAnimationString").Value;
            this.markedTime = float.Parse(element.Attribute("markedTime").Value, CultureInfo.CurrentCulture);
            this.depth = float.Parse(element.Attribute("depth").Value, CultureInfo.CurrentCulture);

            return;
        }
    }
}
