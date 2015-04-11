namespace Physicist.Controls
{
    using System.Xml.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Physicist.Extensions;

    public class Backdrop : IBackgroundObject, IDraw
    {
        public Backdrop()
        {
        }

        public Backdrop(Vector2 location, Size dimensions, float depth, Texture2D texture)
        {
            this.Location = location;
            this.Dimensions = dimensions;
            this.Depth = depth;
            this.Texture = texture;
            this.Scale = new Vector2(this.Dimensions.Width / (float)this.Texture.Width, this.Dimensions.Height / (float)this.Texture.Height);
            this.TileToBounds = false;
        }

        public Vector2 Location { get; set; }

        public Size Dimensions { get; set; }

        public float Depth { get; set; }

        public Texture2D Texture { get; set; }

        public Vector2 Scale { get; private set; }

        public bool TileToBounds { get; private set; }

        public void Draw(ISpritebatch sb)
        {
            if (sb != null)
            {
                sb.Draw(
                    this.Texture,
                    this.Location,
                    null,
                    Color.White,
                    0f,
                    Vector2.Zero,
                    this.Scale,
                    SpriteEffects.None,
                    this.Depth);
            }
        }

        public XElement XmlSerialize()
        {
            XElement element = new XElement(
                "Backdrop",
                this.Location.XmlSerialize("location"),
                this.Dimensions.XmlSerialize("dimension"),
                new XElement("depth", this.Depth),
                new XAttribute("textureRef", this.Texture.Name),
                new XAttribute("class", this.GetType().ToString()),
                new XAttribute("tile", this.TileToBounds));

            return element;
        }

        public void XmlDeserialize(XElement element)
        {
            if (element != null)
            {
                this.Location = ExtensionMethods.XmlDeserializeVector2(element.Element("Location"));
                this.Dimensions = ExtensionMethods.XmlDeserializeSize(element.Element("Dimension"));
                this.Depth = element.GetAttribute("depth", 0f);
                this.Texture = ContentController.Instance.GetContent<Texture2D>(element.Attribute("textureRef").Value);

                this.Scale = new Vector2(this.Dimensions.Width / (float)this.Texture.Width, this.Dimensions.Height / (float)this.Texture.Height);

                this.TileToBounds = element.GetAttribute("tile", false);

                if (this.TileToBounds)
                {
                    this.Texture = this.Texture.TileTexture(this.Dimensions);
                    this.Scale = new Vector2(1f, 1f);
                }
            }
        }
    }
}
