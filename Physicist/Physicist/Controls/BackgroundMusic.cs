﻿namespace Physicist.Controls
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
        public BackgroundMusic(XElement element)
        {
            this.XmlDeserialize(element);
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
            XElement element = new XElement(
                "backdrop",
                this.Location.Serialize("location"),
                this.Dimensions.XmlSerialize(),
                new XElement("soundref", this.SoundEffect.Name),
                new XAttribute("class", this.GetType().ToString()));

            return element;
        }

        public void XmlDeserialize(XElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            this.Location = ExtensionMethods.DeserializeVector2(element.Element("location"));
            this.Dimensions = new Size();
            this.Dimensions.XmlDeserialize(element.Element("dimensions"));

            this.SoundEffect = ContentController.Instance.GetContent<SoundEffect>(
                element.Element("soundref").Value);
        }
    }
}
