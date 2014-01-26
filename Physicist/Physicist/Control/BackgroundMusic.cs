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
        public Vector2 Location { get; set; }
        private Size Dimensions { get; set; }
        private SoundEffect soundEffect { get; set; }

        public BackgroundMusic() { }
        public BackgroundMusic(Vector2 location, Size dimensions, SoundEffect soundEffect)
        {
            this.Location = location;
            this.Dimensions = dimensions;
            this.soundEffect = soundEffect;
        }

        public XElement Serialize()
        {
            XElement element = new XElement("Backdrop",
                this.Location.Serialize("Location"),
                this.Dimensions.Serialize(),
                new XElement("SoundRef", this.soundEffect.Name));

            return element;
        }

        public void Deserialize(XElement element)
        {
            Vector2 location;

            // Location is a property, so it can't be passed by reference.
            ExtensionMethods.Deserialize(element.Element("Location"), out location);
            this.Location = location;

            this.Dimensions = new Size();
            this.Dimensions.Deserialize(element.Element("Dimensions"));

            //TODO: Pull sound effect from global content
            //backgroundMusic.soundEffect.
        }
    }
}
