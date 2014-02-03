namespace Physicist.Actors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;
    using Microsoft.Xna.Framework;

    public struct SpriteAnimation : IXmlSerializable
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
            this.flipHorizontal = false;
            this.flipVertical = false;
            this.playInReverse = false;
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

        public XElement Serialize()
        {
            XElement animationElement = new XElement(XName.Get("SpriteAnimation"));
            animationElement.Add(new XAttribute(XName.Get("class"), typeof(SpriteAnimation).ToString()));

            animationElement.Add(new XAttribute(XName.Get("rowIndex"), this.rowIndex));
            animationElement.Add(new XAttribute(XName.Get("frameCount"), this.frameCount));
            animationElement.Add(new XAttribute(XName.Get("defaultFrameRate"), this.defaultFrameRate));
            animationElement.Add(new XAttribute(XName.Get("playInReverse"), this.playInReverse));
            animationElement.Add(new XAttribute(XName.Get("flipVertical"), this.flipVertical));
            animationElement.Add(new XAttribute(XName.Get("flipHorizontal"), this.flipHorizontal));

            return animationElement;
        }

        public void Deserialize(XElement element)
        {
            this.rowIndex = uint.Parse(element.Attribute("frameLength").Value);
            this.frameCount = uint.Parse(element.Attribute("currentFrame").Value);
            this.defaultFrameRate = float.Parse(element.Attribute("currentAnimationString").Value);
            this.playInReverse = bool.Parse(element.Attribute("markedTime").Value);
            this.flipVertical = bool.Parse(element.Attribute("depth").Value);
            this.flipHorizontal = bool.Parse(element.Attribute("frameLength").Value);
        }
    }
}
