namespace Physicist.Actors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;
    using FarseerPhysics.Dynamics;
    using FarseerPhysics.Factories;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class Actor : IXmlSerializable
    {
        private Dictionary<string, GameSprite> sprites = new Dictionary<string, GameSprite>();
        private Body body;

        public Actor()
        {            
            this.VisibleState = Visibility.Visible;
            this.IsEnabled = true;
            this.Health = 1;
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
        public XElement Serialize()
        {
            return null;
        }

        public void Deserialize(XElement element)
        {
            // Find all Vector elements
            XElement movementSpeedElement = element.Element("MovementSpeed");
            this.MovementSpeed = new Vector2(
                                    float.Parse(movementSpeedElement.Attribute("X").Value),
                                    float.Parse(movementSpeedElement.Attribute("Y").Value));

            // ----------------------------
            // Find the Dictionary element
            XElement spritesElement = element.Element("Sprites");

            // Find all elements in the Dictionary element
            IEnumerable<XElement> gameSpriteElements = element.Descendants("GameSprite");

            // Create GameSprites out of the Deserialze functions in GameSprite
            foreach (XElement gameSpriteElement in gameSpriteElements)
            {
                GameSprite sprite = new GameSprite();
                sprite.Deserialize(gameSpriteElement);
                this.AddSprite(gameSpriteElement.Name.ToString(), sprite);
            }

            // ----------------------
            // Find the Body element
            XElement bodyElement = element.Element("Body");

            // Manually extract all relevant information from the Body element                      
            // assign the body properties to a new body
            this.Body = BodyFactory.CreateRectangle(
                                    MainGame.World,
                                    float.Parse(bodyElement.Attribute("Width").Value),
                                    float.Parse(bodyElement.Attribute("Height").Value),
                                    float.Parse(bodyElement.Attribute("Density").Value)); // width, height, density

            this.Body.BodyType = (BodyType)Enum.Parse(typeof(BodyType), bodyElement.Attribute("BodyType").Value);
            this.Body.Position = new Vector2(
                                    float.Parse(bodyElement.Element("Position").Attribute("X").Value),
                                    float.Parse(bodyElement.Element("Position").Attribute("Y").Value));

            this.Body.CollidesWith = (Category)Enum.Parse(typeof(Category), bodyElement.Attribute("CollidesWith").Value);
            this.Body.FixedRotation = bool.Parse(bodyElement.Attribute("FixedRotation").Value);

            // ----------------------------------
            // Assign the new values to the Actor
            this.Health = int.Parse(element.Attribute("Health").Value);
            this.Rotation = float.Parse(element.Attribute("Rotation").Value);
            this.IsEnabled = bool.Parse(element.Attribute("IsEnabled").Value);

            this.VisibleState = (Visibility)Enum.Parse(typeof(Visibility), element.Attribute("VisibleState").Value);
        }
    }
}
