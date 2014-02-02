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
        public XElement XmlSerialize()
        {
            return null;
        }

        public void XmlDeserialize(XElement element)
        {
            // Find all Vector elements
            XElement positionElement = (XElement)(from e1 in element.Descendants() where string.Compare(e1.Name.ToString(), "Position") == 0 select e1).First();
            XElement velocityElement = (XElement)(from e1 in element.Descendants() where string.Compare(e1.Name.ToString(), "Velocity") == 0 select e1).First();
            XElement accelerationElement = (XElement)(from e1 in element.Descendants() where string.Compare(e1.Name.ToString(), "AccelerationElement") == 0 select e1).First();
            XElement movementSpeedElement = (XElement)(from e1 in element.Descendants() where string.Compare(e1.Name.ToString(), "MovementSpeed") == 0 select e1).First();

            Vector2 newPosition = new Vector2(0, 0);
            float.TryParse(((XAttribute)positionElement.Attribute("X")).Value, out newPosition.X);
            float.TryParse(((XAttribute)positionElement.Attribute("Y")).Value, out newPosition.Y);

            Vector2 newVelocity = new Vector2(0, 0);
            float.TryParse(((XAttribute)velocityElement.Attribute("X")).Value, out newVelocity.X);
            float.TryParse(((XAttribute)velocityElement.Attribute("Y")).Value, out newVelocity.Y);

            Vector2 newAcceleration = new Vector2(0, 0);
            float.TryParse(((XAttribute)accelerationElement.Attribute("X")).Value, out newAcceleration.X);
            float.TryParse(((XAttribute)accelerationElement.Attribute("Y")).Value, out newAcceleration.Y);

            Vector2 newMovementSpeed = new Vector2(0, 0);
            float.TryParse(((XAttribute)movementSpeedElement.Attribute("X")).Value, out newMovementSpeed.X);
            float.TryParse(((XAttribute)movementSpeedElement.Attribute("Y")).Value, out newMovementSpeed.Y);

            // ----------------------------
            // Find the Dictionary element
            XElement spritesElement = (XElement)(from e1 in element.Descendants() where string.Compare(e1.Name.ToString(), "Sprites") == 0 select e1).First();

            // Find all elements in the Dictionary element
            IEnumerable<XElement> gameSpriteElements = from e1 in element.Descendants() where string.Compare(e1.Name.ToString(), "GameSprite") == 0 select e1;

            // Extract all attributes and assign them as the primitive properties of the actor
            int newHealth = 1;
            float newRotation = 0;
            bool newIsEnabled = true;
            Visibility newVisibleState = Visibility.Visible;

            float.TryParse(((XAttribute)element.Attribute("Rotation")).Value, out newRotation);
            int.TryParse(((XAttribute)element.Attribute("Health")).Value, out newHealth);
            bool.TryParse(((XAttribute)element.Attribute("IsEnabled")).Value, out newIsEnabled);
            Enum.TryParse<Visibility>(((XAttribute)element.Attribute("VisibleState")).Value, out newVisibleState);

            // Create GameSprites out of the Deserialze functions in GameSprite
            foreach (XElement gameSpriteElement in gameSpriteElements)
            {
                GameSprite newSprite = new GameSprite();
                newSprite.XmlDeserialize(gameSpriteElement);
                this.AddSprite(gameSpriteElement.Name.ToString(), newSprite);
            }

            // ----------------------
            // Find the Body element
            XElement bodyElement = (XElement)(from e1 in element.Descendants() where string.Compare(e1.Name.ToString(), "Body") == 0 select e1).First();

            // Manually extract all relevant information from the Body element
            // This includes the Vector 2 Element, several properties and a few construction parameters - Width, Height, and Density
            XElement bodyPositionElement = (XElement)(from e1 in bodyElement.Descendants() where string.Compare(e1.Name.ToString(), "Position") == 0 select e1).First();
            BodyType newBodyType = BodyType.Dynamic;
            Category newCollidesWith = Category.All;
            bool newFixedRotation = true;
            Vector2 newBodyPosition = new Vector2(0, 0);
            float newWidth, newHeight, newDensity = 1f;

            float.TryParse(((XAttribute)bodyPositionElement.Attribute("X")).Value, out newBodyPosition.X);
            float.TryParse(((XAttribute)bodyPositionElement.Attribute("Y")).Value, out newBodyPosition.Y);
            Enum.TryParse<BodyType>(((XAttribute)bodyElement.Attribute("BodyType")).Value, out newBodyType);
            Enum.TryParse<Category>(((XAttribute)bodyElement.Attribute("BodyType")).Value, out newCollidesWith);
            bool.TryParse(((XAttribute)bodyElement.Attribute("FixedRotation")).Value, out newFixedRotation);
            float.TryParse(((XAttribute)bodyElement.Attribute("Width")).Value, out newWidth);
            float.TryParse(((XAttribute)bodyElement.Attribute("Height")).Value, out newHeight);
            float.TryParse(((XAttribute)bodyElement.Attribute("Density")).Value, out newDensity);

            // assign the body properties to a new body
            Body newBody = BodyFactory.CreateRectangle(MainGame.World, newWidth, newHeight, newDensity); // width, height, density

            newBody.BodyType = newBodyType;
            newBody.Position = newBodyPosition;
            newBody.CollidesWith = newCollidesWith;
            newBody.FixedRotation = newFixedRotation;

            // -----------------------------------------
            // Assign the new values to the Actor, starting with the body so the velocity, position etc. properties can override the default body values
            this.Body = newBody;

            this.Health = newHealth;
            this.Rotation = newHealth;
            this.IsEnabled = newIsEnabled;
            this.VisibleState = newVisibleState;
            this.Position = newPosition;
            this.Velocity = newVelocity;
            this.Acceleration = newAcceleration;
            this.MovementSpeed = newMovementSpeed;
        }
    }
}
