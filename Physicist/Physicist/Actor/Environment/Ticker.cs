namespace Physicist.Actors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Physicist.Controls;
    using Physicist.Extensions;

    public class Ticker : Actor
    {
        private List<Tuple<string, Color>> messages = new List<Tuple<string, Color>>();
        private bool playing = false;
        private float messagePosition = 0;
        private int messageIndex = 0;
        private float transitTime = 0;
        private float messageTime = 0;
        private int width = 0;
        private int height = 0;
        private SpriteFont font;
        private string fontRef = string.Empty;
        private uint currentCharacterIndex = 0;
        private uint cullCharacterIndex = 0;
        private float previousPosition = 0;
        private float previousCullPosition = 0;
        private float speed = 0;
        private float discreteAccumError = 0;
        private float discreteCullError = 0;
        private float cullOverreachError = 0;

        public Ticker()
            : base()
        {
            this.Scale = 1f;
        }

        public IEnumerable<string> Messages 
        {
            get
            {
                foreach (var m in this.messages)
                {
                    yield return m.Item1;
                }
            }
        }

        public float MessageDelay { get; set; }

        public SpriteFont MessageFont 
        {
            get
            {
                return this.font;
            }

            private set
            {
                if (value != null)
                {
                    this.font = value;
                    this.Scale = this.height / this.font.MeasureString("W").Y;
                }
            }
        }

        public float TransitTime
        {
            get
            {
                return this.transitTime;
            }

            set
            {
                this.transitTime = MathHelper.Clamp(value, 0, float.MaxValue);
                this.speed = this.width / this.transitTime;
            }
        }

        protected string CurrentMessage
        {
            get
            {
                return this.messages[this.MessageIndex].Item1;
            }
        }

        protected float StartingCharLength { get; private set; }

        protected float Scale { get; private set; }        

        protected int MessageIndex
        {
            get
            {
                return this.messageIndex;
            }

            set
            {
                this.messageIndex = value;
                this.CurrentCharacterIndex = 0;
                this.CullCharacterIndex = 0;
                if (this.MessageFont != null)
                {
                    this.CurrentMessageLength = this.MessageFont.MeasureString(this.messages[value].Item1).X * this.Scale;
                    this.StartingCharLength = this.MessageFont.MeasureString(this.messages[value].Item1[0].ToString()).X * this.Scale;
                }
                else
                {
                    this.CurrentMessageLength = 0f;
                }
            }
        }

        protected uint CurrentCharacterIndex 
        {
            get
            {
                return this.currentCharacterIndex;
            }

            private set
            {
                uint index = (uint)MathHelper.Clamp((int)value, 0, this.messages[this.messageIndex].Item1.Length);
                this.currentCharacterIndex = index;
                index = index >= this.CurrentMessage.Length ? (uint)this.CurrentMessage.Length - 1 : index;
                this.CurrentCharacterLength = this.MessageFont.MeasureString(this.CurrentMessage[(int)index].ToString()).X * this.Scale;
            }
        }

        protected uint CullCharacterIndex 
        {
            get
            {
                return this.cullCharacterIndex;
            }

            private set
            {
                this.cullCharacterIndex = value;
                this.CullCharacterLength = (int)value - 1 < 0 ? 0 : this.MessageFont.MeasureString(this.messages[this.messageIndex].Item1[(int)value - 1].ToString()).X * this.Scale;
            }
        }

        protected float CullCharacterLength { get; private set; }

        protected float CurrentMessageLength { get; private set; }

        protected float CurrentCharacterLength { get; private set; }

        public override void Update(GameTime gameTime)
        {
            if (gameTime != null)
            {
                if (!this.playing)
                {
                    this.messageTime += gameTime.ElapsedGameTime.Milliseconds / 1000f;
                    if (this.messageTime >= this.MessageDelay)
                    {
                        this.messageTime = 0;
                        this.playing = true;
                        this.previousPosition = 0;
                        this.previousCullPosition = this.width;
                        this.discreteAccumError = 0;
                        this.MessageIndex = (this.MessageIndex + 1) % this.messages.Count();
                        this.discreteCullError = 0;
                        this.cullOverreachError = 0;
                    }
                }
                else
                {
                    var delta = this.speed * gameTime.ElapsedGameTime.Milliseconds / 1000f;
                    this.messagePosition += delta;
                    if (this.messagePosition >= (this.CurrentMessageLength + this.width))
                    {
                        this.playing = false;
                        this.messagePosition = 0;
                    }
                    else if ((this.messagePosition - this.previousPosition >= this.CurrentCharacterLength) && this.CurrentCharacterIndex != this.CurrentMessage.Length + 1)
                    {
                        this.discreteAccumError += this.messagePosition - this.previousPosition - this.CurrentCharacterLength;
                        this.CurrentCharacterIndex++;
                        this.previousPosition = this.messagePosition;
                    }
                    
                    if (this.messagePosition >= (this.width - this.StartingCharLength))
                    {
                        if (this.messagePosition - this.previousCullPosition > (this.CullCharacterLength - this.cullOverreachError))
                        {
                            this.cullOverreachError = this.messagePosition - this.previousCullPosition - this.CullCharacterLength;
                            this.CullCharacterIndex++;
                            this.discreteCullError += this.CullCharacterLength - this.cullOverreachError;
                            this.previousCullPosition = this.messagePosition;
                        }
                    }
                }

                base.Update(gameTime);
            }
        }

        public override void Draw(ISpritebatch sb)
        {
            if (sb != null)
            {
                if (this.playing)
                {
                    var messageSub = this.messages[this.messageIndex].Item1.Substring((int)this.CullCharacterIndex, (int)(this.CurrentCharacterIndex - this.CullCharacterIndex));
                    var len = new Vector2(this.messagePosition + (this.StartingCharLength / 2) - this.discreteAccumError - this.discreteCullError, 0);
                    sb.DrawString(
                                  this.MessageFont,
                                  messageSub,
                                  this.Position + new Vector2(this.width / 2, -this.height / 2) - len,
                                  this.messages[this.messageIndex].Item2,
                                  this.Rotation,
                                  Vector2.Zero,
                                  this.Scale,
                                  SpriteEffects.None,
                                  this.Sprites["Ticker"].Depth * 1.1f);
                }

                base.Draw(sb);
            }
        }

        public override XElement XmlSerialize()
        {
            return new XElement(
                "Ticker",
                new XAttribute("speed", this.transitTime),
                new XAttribute("messageDelay", this.MessageDelay),
                new XAttribute("width", this.width),
                new XAttribute("height", this.height),
                new XAttribute("fontRef", this.fontRef),
                new XElement("Messages", this.messages.Select(msg => new XElement("Message", new XAttribute("color", msg.Item2.ToString()), msg.Item1))),
                base.XmlSerialize());
        }

        public override void XmlDeserialize(XElement element)
        {
            if (element != null)
            {
                base.XmlDeserialize(element.Element("Actor"));

                this.width = element.GetAttribute("width", 0);
                this.height = element.GetAttribute("height", 0);

                this.TransitTime = element.GetAttribute("transitTime", 0);
                this.previousCullPosition = this.width;

                this.fontRef = element.GetAttribute("fontRef", string.Empty);
                this.MessageFont = ContentController.Instance.GetContent<SpriteFont>(this.fontRef);

                var messagesEle = element.Element("Messages");
                if (messagesEle != null)
                {
                    foreach (var message in messagesEle.Elements("Message"))
                    {
                        var color = message.GetAttribute("color", PhysicistColor.Yellow).ToXnaColor();
                        this.messages.Add(new Tuple<string, Color>(message.Value, color));
                    }
                }

                this.MessageDelay = element.GetAttribute("messageDelay", 1f);

                this.MessageIndex = 0;
                this.playing = true;
            }
        }
    }
}
