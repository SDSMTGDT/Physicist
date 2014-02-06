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
        public BackgroundMusic(XElement classData)
        {
            this.XmlDeserialize(classData);
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

        public XElement XmlSerialize()
        {
            XElement classData = new XElement(
                "backdrop",
                this.Location.Serialize("location"),
                this.Dimensions.XmlSerialize(),
                new XElement("soundref", this.SoundEffect.Name),
                new XAttribute("class", this.GetType().ToString()));

            return classData;
        }

        public void XmlDeserialize(XElement classData)
        {
            if (classData == null)
            {
                throw new ArgumentNullException("classData");
            }

            this.Location = ExtensionMethods.DeserializeVector2(classData.Element("location"));
            this.Dimensions = new Size();
            this.Dimensions.XmlDeserialize(classData.Element("dimensions"));

            // TODO: Pull sound effect from global content
        }
    }
}
