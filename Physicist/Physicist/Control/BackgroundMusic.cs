namespace Physicist.Controls
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Physicist.Actors;
    using Physicist.Extensions;

    public class BackgroundMusic : IXmlSerializable
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

        public XElement Serialize()
        {
            XElement element = new XElement(
                "Backdrop",
                this.Location.Serialize("Location"),
                this.Dimensions.Serialize(),
                new XElement("SoundRef", this.SoundEffect.Name));

            return element;
        }

        public void Deserialize(XElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            this.Location = ExtensionMethods.DeserializeVector2(element.Element("Location"));
            this.Dimensions = new Size();
            this.Dimensions.Deserialize(element.Element("Dimensions"));

            // TODO: Pull sound effect from global content
        }
    }
}
