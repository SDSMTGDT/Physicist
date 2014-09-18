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
    using Physicist.Extensions;

    public class Actor : PhysicistGameScreenItem, IActor
    {
        private Dictionary<string, GameSprite> sprites = new Dictionary<string, GameSprite>();
        private Body body;
        private BodyInfo bodyInfo;
        private int health;

        public Actor()
        {
        }

        public Actor(string name)
        {
            this.PathManager = new PathManager(this);
            this.VisibleState = Visibility.Visible;
            this.IsEnabled = true;
            this.CanBeDamaged = true;
            this.Health = 1;
            this.Name = name;
        }

        public string Name
        {
            get;
            private set;
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
                if (value != null)
                {
                    this.body.UserData = this;
                    this.body.OnCollision += this.OnCollision;
                }
            }
        }

        public bool CanBeDamaged { get; set; }

        public int AttackDamage
        {
            get;
            set;
        }

        // 2space variables
        public Vector2 Position
        {
            get
            {
                return this.body.Position.ToDisplayUnits();
            }

            set
            {
                this.body.Position = value.ToSimUnits();
            }
        }

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

        public PathManager PathManager
        {
            get;
            set;
        }

        public Vector2 MovementSpeed { get; set; }

        // gameplay state variables
        public int Health 
        {
            get
            {
                return this.health;
            }

            set
            {
                this.health = value;
                if (this.health <= 0)
                {
                    this.VisibleState = Visibility.Hidden;
                    this.IsEnabled = false;
                    this.body.CollidesWith = Category.None;
                }
            }
        }

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

        public virtual void Draw(ISpritebatch sb)
        {
            if (this.IsEnabled)
            {
                if (sb != null && this.VisibleState == Visibility.Visible)
                {
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
                            this.Position + sprite.Offset - this.bodyInfo.ShapeOffset,
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

        public virtual void AddSprite(GameSprite sprite)
        {
            if (sprite == null)
            {
                throw new ArgumentNullException("sprite");
            }

            this.Sprites.Add(sprite.SpriteName, sprite);
        }

        public virtual void Update(GameTime gameTime)
        {
            // update every sprite in the sprite collection
            if (this.IsEnabled)
            {
                foreach (var sprite in this.Sprites.Values)
                {
                    sprite.Update(gameTime);
                }

                this.PathManager.Update(gameTime);
            }
        }

        // Implementing Interface   
        public override XElement XmlSerialize()
        {
            // define the Actor element
            XElement actorElement = new XElement("Actor");
            actorElement.Add(new XAttribute("class", typeof(Actor).ToString()));
            actorElement.Add(new XAttribute("name", this.Name));
            actorElement.Add(new XAttribute("health", this.Health));
            actorElement.Add(new XAttribute("rotation", this.Rotation));
            actorElement.Add(new XAttribute("isEnabled", this.IsEnabled));
            actorElement.Add(new XAttribute("visibleState", Enum.GetName(typeof(Visibility), this.VisibleState)));

            actorElement.Add(ExtensionMethods.XmlSerialize(this.MovementSpeed, "MovementSpeed"));

            // ----------------------------
            // Define the Dictionary element
            XElement spritesElement = new XElement("Sprites");

            // Add GameSprites using the Serialize functions in GameSprite
            foreach (GameSprite sprite in this.sprites.Values)
            {
                spritesElement.Add(sprite.XmlSerialize());
            }

            actorElement.Add(spritesElement);

            // ----------------------
            // Create and add the Body element
            actorElement.Add(new XElement("BodyInfo", this.bodyInfo.XmlSerialize()));

            return actorElement;
        }

        public override void XmlDeserialize(XElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            XAttribute nameAtt = element.Attribute("name");
            if (nameAtt != null)
            {
                this.Name = nameAtt.Value;
            }

            this.MovementSpeed = ExtensionMethods.XmlDeserializeVector2(element.Element("MovementSpeed"));

            // Create GameSprites out of the Deserialze functions in GameSprite
            foreach (XElement gameSpriteEle in element.Element("Sprites").Elements("GameSprite"))
            {
                GameSprite gameSprite = new GameSprite();
                gameSprite.XmlDeserialize(gameSpriteEle);
                this.sprites.Add(gameSprite.SpriteName, gameSprite);
            }

            // ----------------------
            // Find the Body element
            XElement bodyElement = element.Element("BodyInfo");
            if (bodyElement != null)
            {
                var bodyData = XmlBodyFactory.DeserializeBody(this.World, this.Map.Height, bodyElement.Elements().ElementAt(0));
                this.Body = bodyData.Item1;
                this.bodyInfo = bodyData.Item2;
            }

            this.PathManager = new PathManager(this);
            this.PathManager.Screen = this.Screen;
            XElement pathManagerEle = element.Element("PathManager");
            if (pathManagerEle != null)
            {
                this.PathManager.XmlDeserialize(pathManagerEle);
            }
            
            // ----------------------------------
            // Assign the new values to the Actor
            this.Health = int.Parse(element.Attribute("health").Value, CultureInfo.CurrentCulture);
            this.Rotation = float.Parse(element.Attribute("rotation").Value, CultureInfo.CurrentCulture);
            this.IsEnabled = bool.Parse(element.Attribute("isEnabled").Value);

            this.VisibleState = (Visibility)Enum.Parse(typeof(Visibility), element.Attribute("visibleState").Value);
        }

        protected virtual bool OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (this.IsEnabled && this.CanBeDamaged && fixtureB != null)
            {
                var collisionBody = fixtureB.Body;
                if (collisionBody != null)
                {
                    IDamage damagingBody = collisionBody.UserData as IDamage;
                    if (damagingBody != null)
                    {
                        this.Health -= damagingBody.AttackDamage;
                    }
                }
            }

            return this.IsEnabled;
        }
    }
}
