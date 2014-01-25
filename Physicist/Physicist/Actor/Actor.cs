namespace Physicist.Actors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using FarseerPhysics.Dynamics;
    using FarseerPhysics.Factories;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using System.Xml.Linq;

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
        public void Serialize (System.IO.Stream iostream)
        {
 	        return;
        }

        public Object Deserialize(XElement element)
        {
            // Find the Body element
            XElement bodyElement = (XElement)(from e1 in element.Descendants() where string.Compare(e1.Name.ToString(), "Body") == 0 select e1).First();

            // Find all Vector elements
            XElement positionElement = (XElement)(from e1 in element.Descendants() where string.Compare(e1.Name.ToString(), "Position") == 0 select e1).First();
            XElement velocityElement = (XElement)(from e1 in element.Descendants() where string.Compare(e1.Name.ToString(), "Velocity") == 0 select e1).First();
            XElement accelerationElement = (XElement)(from e1 in element.Descendants() where string.Compare(e1.Name.ToString(), "AccelerationElement") == 0 select e1).First();
            XElement movementSpeedElement = (XElement)(from e1 in element.Descendants() where string.Compare(e1.Name.ToString(), "MovementSpeed") == 0 select e1).First();

            // Find the Visibility element
            XElement VisibleStateElement = (XElement)(from e1 in element.Descendants() where string.Compare(e1.Name.ToString(), "VisibleState") == 0 select e1).First();

            // Find the Dictionary element
            XElement spritesElement = (XElement)(from e1 in element.Descendants() where string.Compare(e1.Name.ToString(), "Sprites") == 0 select e1).First();
            // Find all elements in the Dictionary element
            IEnumerable<XElement>gameSpriteElements = from e1 in element.Descendants() where string.Compare(e1.Name.ToString(), "GameSprite") == 0 select e1;
            
            //
            //Create the new Actor
            Actor newActor = new Actor();

            //Extract all attributes and assign them as the primitive properties of the actor
            XAttribute rotationAttribute = (XAttribute)(from a1 in element.Attributes() where string.Compare(a1.Name.ToString(), "Rotation") == 0 select a1).First();
            XAttribute healthAttribute = (XAttribute)(from a1 in element.Attributes() where string.Compare(a1.Name.ToString(), "Health") == 0 select a1).First();
            XAttribute isEnabledAttribute = (XAttribute)(from a1 in element.Attributes() where string.Compare(a1.Name.ToString(), "IsEnabled") == 0 select a1).First();
            XAttribute isDeadAttribute = (XAttribute)(from a1 in element.Attributes() where string.Compare(a1.Name.ToString(), "IsDead") == 0 select a1).First();

            int newHealth = 1;
            float newRotation = 0;
            bool newIsEnabled = true;
            bool newIsDead = false;

            float.TryParse(rotationAttribute.Value, out newRotation);
            int.TryParse(healthAttribute.Value, out newHealth);
            bool.TryParse(isEnabledAttribute.Value, out newIsEnabled);
            bool.TryParse(isDeadAttribute.Value, out newIsDead);

            //
            //Create GameSprites out of the Deserialze functions in GameSprite
            foreach (XElement gameSpriteElement in gameSpriteElements)
            {
                GameSprite newSprite = new GameSprite();
                newSprite = (GameSprite)newSprite.Deserialize(gameSpriteElement);
                newActor.AddSprite(gameSpriteElement.Name.ToString(), newSprite);
            }

            //Manually extract all relevant information from the Body element
            //Body newBody = 
            //newActor.Body = newBody;
            
            return newActor;
        }       
    }
}
