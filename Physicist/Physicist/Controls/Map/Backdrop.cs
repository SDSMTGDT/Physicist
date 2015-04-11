namespace Physicist.Controls
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Physicist.Actors;
    using Physicist.Extensions;

    public class Backdrop : PhysicistGameScreenItem, IBackgroundObject, IDraw, IUpdate
    {
        private Dictionary<string, GameSprite> sprites = new Dictionary<string, GameSprite>();

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

        public Dictionary<string, GameSprite> Sprites
        {
            get
            {
                return this.sprites;
            }
        }

        public void Update(GameTime gameTime)
        {
            // update every sprite in the sprite collection
            foreach (var sprite in this.Sprites.Values)
            {
                sprite.Update(gameTime);
            }
        }

        public void Draw(ISpritebatch sb)
        {
            if (sb != null)
            {
                if (this.Texture != null)
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

                foreach (var sprite in this.Sprites.Values)
                {
                    var effect = SpriteEffects.None;
                    if (sprite.CurrentAnimation.FlipHorizontal)
                    {
                        effect |= SpriteEffects.FlipHorizontally;
                    }

                    if (sprite.CurrentAnimation.FlipVertical)
                    {
                        effect |= SpriteEffects.FlipVertically;
                    }

                    sb.Draw(
                        sprite.SpriteSheet,
                        this.Location - (new Vector2(this.Dimensions.Width, this.Dimensions.Height) / 2f) + sprite.Offset,
                        sprite.CurrentSprite,
                        Color.White,
                        0f,
                        Vector2.Zero,
                        1f,
                        effect,
                        sprite.Depth);
                }
            }
        }

        public override XElement XmlSerialize()
        {
            XElement element = new XElement(
                "Backdrop",
                this.Location.XmlSerialize("location"),
                this.Dimensions.XmlSerialize("dimension"),
                new XElement("depth", this.Depth),
                new XAttribute("textureRef", this.Texture.Name),
                new XAttribute("class", this.GetType().ToString()),
                new XElement("Sprites", this.sprites.Values.Select(sprite => sprite.XmlSerialize()).ToArray()),
                new XAttribute("tile", this.TileToBounds));

            return element;
        }

        public override void XmlDeserialize(XElement element)
        {
            if (element != null)
            {
                this.Location = ExtensionMethods.XmlDeserializeVector2(element.Element("Location"));
                this.Location = new Vector2(this.Location.X, this.Map.Height - this.Location.Y);

                this.Dimensions = ExtensionMethods.XmlDeserializeSize(element.Element("Dimension"));
                this.Depth = element.GetAttribute("depth", 0f);

                var textureRef = element.GetAttribute("textureRef", string.Empty);
                if (!string.IsNullOrEmpty(textureRef))
                {
                    this.Texture = ContentController.Instance.GetContent<Texture2D>(textureRef);

                    if (this.Texture != null)
                    {
                        this.Scale = new Vector2(this.Dimensions.Width / (float)this.Texture.Width, this.Dimensions.Height / (float)this.Texture.Height);

                        this.TileToBounds = element.GetAttribute("tile", false);

                        if (this.TileToBounds)
                        {
                            this.Texture = this.Texture.TileTexture(this.Dimensions);
                            this.Scale = new Vector2(1f, 1f);
                        }
                    }
                }

                var spriteElements = element.Element("Sprites");
                if (spriteElements != null)
                {
                    foreach (XElement gameSpriteEle in spriteElements.Elements("GameSprite"))
                    {
                        GameSprite gameSprite = new GameSprite();
                        gameSprite.XmlDeserialize(gameSpriteEle);
                        this.sprites.Add(gameSprite.SpriteName, gameSprite);
                    }
                }
            }
        }
    }
}
