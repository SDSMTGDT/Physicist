namespace Physicist.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Physicist.Actors;
    using Physicist.Extensions;

    public class Backdrop : IXmlSerializable
    {
        public Backdrop(XElement classData)
        {
            this.XmlDeserialize(classData);
        }

        public Backdrop(Vector2 location, Size dimensions, float depth, Texture2D texture)
        {
            this.Location = location;
            this.Dimensions = dimensions;
            this.Depth = depth;
            this.Texture = texture;
        }

        public Vector2 Location { get; set; }

        public Size Dimensions { get; set; }

        public float Depth { get; set; }

        public Texture2D Texture { get; set; }

        public XElement XmlSerialize()
        {
            XElement classData = new XElement(
                "backdrop",
                this.Location.Serialize("location"),
                this.Dimensions.XmlSerialize(),
                new XElement("depth", this.Depth),
                new XElement("textureref", this.Texture.Name),
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
            this.Depth = float.Parse(classData.Element("depth").Value, CultureInfo.CurrentCulture);

            // TODO: Pull sound effect from global content
        }
    }
}
