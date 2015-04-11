namespace Physicist.Controls
{
    using System;
    using System.Xml.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Physicist.Actors;
    using Physicist.Extensions;

    public class BackgroundMusic : PhysicistGameScreenItem, IBackgroundObject, IUpdate
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

        public override XElement XmlSerialize()
        {
            XElement element = new XElement(
                "backdrop",
                this.Location.XmlSerialize("location"),
                this.Dimensions.XmlSerialize("dimensions"),
                new XElement("soundref", this.SoundEffect.Name),
                new XAttribute("class", this.GetType().ToString()));

            return element;
        }

        public override void XmlDeserialize(XElement element)
        {
            if (element != null)
            {
                this.Location = ExtensionMethods.XmlDeserializeVector2(element.Element("Location"));
                this.Location = new Vector2(this.Location.X, this.Map.Height - this.Location.Y);
                this.Dimensions = ExtensionMethods.XmlDeserializeSize(element.Element("Dimensions"));

                this.SoundEffect = ContentController.Instance.GetContent<SoundEffect>(element.Attribute("soundref").Value);
            }
        }
    }
}
