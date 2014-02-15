namespace Physicist.Actors
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;
    using FarseerPhysics.Dynamics;
    using FarseerPhysics.Factories;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Physicist.Controls;

    public class Actor : IXmlSerializable, IPosition
    {
        private Dictionary<string, GameSprite> sprites = new Dictionary<string, GameSprite>();
        private Body body;
        private BodyInfo bodyInfo;

        public Actor(XElement element)
        {
            this.XmlDeserialize(element);
        }

        public Actor()
        {
            this.VisibleState = Visibility.Visible;
            this.IsEnabled = true;
            this.Health = 1;
            this.bodyInfo.Width = 0;
            this.bodyInfo.Height = 0;
            this.bodyInfo.CollidesWith = Category.None;
        }

        // Farseer Structures
        public Body Body
        {
            get
            {
                return this.body;
            }

            set
            {
                this.body = value;
            }
        }

        // 2space variables
        public Vector2 Position
        {
            get
            {
                return this.body.Position;
            }

            set
            {
                this.body.Position = value;
            }
        }

        public Vector2 Velocity { get; set; }

        public Vector2 Acceleration { get; set; }

        public float Rotation
        {
            get
            {
                return this.body.Rotation;
            }

            set
            {
                this.body.Rotation = value;
            }
        }

        public Vector2 MovementSpeed { get; set; }

        // gameplay state variables
        public int Health { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsDead
        {
            get
            {
                return this.Health <= 0;
            }
        }

        // draw properties
        public Dictionary<string, GameSprite> Sprites
        {
            get
            {
                return this.sprites;
            }
        }

        public Visibility VisibleState { get; set; }

        public virtual void Draw(SpriteBatch sb)
        {
            if (this.IsEnabled)
            {
                if (sb != null && this.VisibleState == Visibility.Visible)
                {
                    foreach (var sprite in this.Sprites.Values)
                    {
                        Vector2 shapeOffset = ((Vector2)sprite.FrameSize) / 2;

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
                            this.Position + sprite.Offset - shapeOffset,
                            sprite.CurrentSprite,
                            Color.White,
                            this.Rotation,
                            Vector2.Zero,
                            1f,
                            effect,
                            sprite.Depth);
                    }
                }
            }
        }

        public virtual void AddSprite(string name, GameSprite sprite)
        {
            if (sprite == null)
            {
                throw new ArgumentNullException("sprite");
            }

            this.Sprites.Add(name, sprite);
        }

        public virtual void Update(GameTime time)
        {
            // update every sprite in the sprite collection
            if (this.IsEnabled)
            {
                foreach (var sprite in this.Sprites.Values)
                {
                    sprite.Update(time);
                }
            }
        }

        // Implementing Interface   
        public XElement XmlSerialize()
        {
            // define the Actor element
            XElement actorElement = new XElement(XName.Get("Actor"));
            actorElement.Add(new XAttribute(XName.Get("class"), typeof(Actor).ToString()));
            actorElement.Add(new XAttribute(XName.Get("Health"), this.Health));
            actorElement.Add(new XAttribute(XName.Get("Rotation"), this.Rotation));
            actorElement.Add(new XAttribute(XName.Get("IsEnabled"), this.IsEnabled));
            actorElement.Add(new XAttribute(XName.Get("VisibleState"), Enum.GetName(typeof(Visibility), this.VisibleState)));

            // Add all Vector elements
            XElement movementSpeedElement = new XElement(XName.Get("MovementSpeed"));
            movementSpeedElement.Add(new XAttribute(XName.Get("X"), this.MovementSpeed.X));
            movementSpeedElement.Add(new XAttribute(XName.Get("Y"), this.MovementSpeed.Y));
            actorElement.Add(movementSpeedElement);

            // ----------------------------
            // Define the Dictionary element
            XElement spritesElement = new XElement(XName.Get("Sprites"));

            // Add GameSprites using the Serialize functions in GameSprite
            foreach (GameSprite sprite in this.sprites.Values)
            {
                XElement spriteElement = sprite.XmlSerialize();
                spritesElement.Add(spriteElement);
            }

            actorElement.Add(spritesElement);

            // ----------------------
            // Create and add the Body element
            XElement bodyElement = new XElement(XName.Get("Body"));

            // Manually extract all relevant information from the Body and put it into an element                    
            bodyElement.Add(new XAttribute(XName.Get("Density"), this.Body.FixtureList[0].Shape.Density));
            bodyElement.Add(new XAttribute(XName.Get("Width"), this.bodyInfo.Width));
            bodyElement.Add(new XAttribute(XName.Get("Height"), this.bodyInfo.Height));

            // Add several other attributes to the body
            bodyElement.Add(new XAttribute(XName.Get("BodyType"), Enum.GetName(typeof(BodyType), this.Body.BodyType)));
            bodyElement.Add(new XAttribute(XName.Get("FixedRotation"), this.Body.FixedRotation));
            bodyElement.Add(new XAttribute(XName.Get("CollidesWith"), Enum.GetName(typeof(Category), this.bodyInfo.CollidesWith)));

            // add the body's position
            XElement bodyPositionElement = new XElement(XName.Get("Position"));
            bodyPositionElement.Add(new XAttribute(XName.Get("X"), this.Body.Position.X));
            bodyPositionElement.Add(new XAttribute(XName.Get("Y"), this.Body.Position.Y));
            bodyElement.Add(bodyPositionElement);

            actorElement.Add(bodyElement);

            return actorElement;
        }

        public void XmlDeserialize(XElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            // Find all Vector elements
            XElement movementSpeedElement = element.Element("MovementSpeed");

            this.MovementSpeed = new Vector2(
                                    float.Parse(movementSpeedElement.Attribute("X").Value, CultureInfo.CurrentCulture),
                                    float.Parse(movementSpeedElement.Attribute("Y").Value, CultureInfo.CurrentCulture));

            // ----------------------------
            // Find the Dictionary element
            IEnumerable<XElement> gameSpriteElements = element.Descendants("GameSprite");

            // Create GameSprites out of the Deserialze functions in GameSprite
            foreach (XElement gameSpriteElement in gameSpriteElements)
            {
                GameSprite sprite = new GameSprite(gameSpriteElement);
                this.Sprites.Add(gameSpriteElement.Name.LocalName, sprite);
            }

            // ----------------------
            // Find the Body element
            XElement bodyElement = element.Element("Body");

            // Manually extract all relevant information from the Body element
            // assign the body properties to a new body
            this.Body = BodyFactory.CreateRectangle(
                                    MainGame.World,
                                    float.Parse(bodyElement.Attribute("Width").Value, CultureInfo.CurrentCulture),
                                    float.Parse(bodyElement.Attribute("Height").Value, CultureInfo.CurrentCulture),
                                    float.Parse(bodyElement.Attribute("Density").Value, CultureInfo.CurrentCulture)); // width, height, density

            this.Body.BodyType = (BodyType)Enum.Parse(typeof(BodyType), bodyElement.Attribute("BodyType").Value);
            this.Body.Position = new Vector2(
                                    float.Parse(bodyElement.Element("Position").Attribute("X").Value, CultureInfo.CurrentCulture),
                                    float.Parse(bodyElement.Element("Position").Attribute("Y").Value, CultureInfo.CurrentCulture));

            this.Body.CollidesWith = (Category)Enum.Parse(typeof(Category), bodyElement.Attribute("CollidesWith").Value);
            this.Body.FixedRotation = bool.Parse(bodyElement.Attribute("FixedRotation").Value);

            // ----------------------------------
            // Assign the new values to the Actor
            this.Health = int.Parse(element.Attribute("Health").Value, CultureInfo.CurrentCulture);
            this.Rotation = float.Parse(element.Attribute("Rotation").Value, CultureInfo.CurrentCulture);
            this.IsEnabled = bool.Parse(element.Attribute("IsEnabled").Value);

            this.VisibleState = (Visibility)Enum.Parse(typeof(Visibility), element.Attribute("VisibleState").Value);
        }

        public float XPosition()
        {
            return this.Position.X;
            throw new NotImplementedException();
        }

        public float YPosition()
        {
            return this.Position.Y;
            throw new NotImplementedException();
        }
    }
}
