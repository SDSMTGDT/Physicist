namespace Physicist.Controls
{
    using System;
    using System.Xml.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Physicist.Types.Common;
    using Physicist.Types.Controllers;
    using Physicist.Types.Interfaces;
    using Physicist.Types.Util;

    public class BackgroundMusic : IBackgroundObject, IUpdate
    {
        public BackgroundMusic()
        {
        }

        public BackgroundMusic(Vector2 location, Size dimensions, SoundEffect soundEffect)
        {
            this.Location = location;
            this.Dimensions = dimensions;
            this.SoundEffect = soundEffect;
        }

        public Vector2 Location { get; set; }

        public Size Dimensions { get; set; }

        public SoundEffect SoundEffect { get; set; }

        public void Update(GameTime gameTime)
        {
            if (gameTime != null)
            {
                if (gameTime.ElapsedGameTime > this.SoundEffect.Duration)
                {
                    this.SoundEffect.Play();
                }
            }

            throw new NotImplementedException("Updates on play/pause?");
        }

        public XElement XmlSerialize()
        {
            XElement element = new XElement(
                "backdrop",
                this.Location.XmlSerialize("location"),
                this.Dimensions.XmlSerialize("dimensions"),
                new XElement("soundref", this.SoundEffect.Name),
                new XAttribute("class", this.GetType().ToString()));

            return element;
        }

        public void XmlDeserialize(XElement element)
        {
            if (element != null)
            {
                this.Location = XmlDeserializeHelper.XmlDeserialize<Vector2>(element.Element("Location"));
                this.Dimensions = XmlDeserializeHelper.XmlDeserialize<Size>(element.Element("Dimensions"));

                this.SoundEffect = ContentController.Instance.GetContent<SoundEffect>(element.Attribute("soundref").Value);
            }
        }
    }
}
