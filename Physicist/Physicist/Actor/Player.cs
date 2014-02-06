namespace Physicist.Actors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;
    using FarseerPhysics;
    using FarseerPhysics.Dynamics;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Physicist.Controls;
    using Physicist.Enums;
    using Physicist.Extensions;

    public class Player : Actor, IXmlSerializable
    {
        public Player(XElement element)
            : this()
        {
            this.XmlDeserialize(element);
        }

        public Player() : 
            base()
        {
            this.MovementSpeed = new Vector2(1f, 1f);
        }

        public new Body Body
        {
            get
            {
                return base.Body;
            }

            set
            {
                base.Body = value;
                base.Body.OnCollision += this.Body_OnCollision;
            }
        }

        public void Update(GameTime time, KeyboardState ks)
        {
            bool keypress = false;
            string spriteStateString = string.Empty;
            Vector2 dp = Vector2.Zero;

            if (ks.IsKeyDown(Keys.Up))
            {
                dp.Y -= this.MovementSpeed.Y;
                spriteStateString = "Up";
                keypress = true;
            }
            else if (ks.IsKeyDown(Keys.Down))
            {
                dp.Y += this.MovementSpeed.Y;
                spriteStateString = "Down";
                keypress = true;
            }
            else if (ks.IsKeyDown(Keys.Left))
            {
                dp.X -= this.MovementSpeed.X;
                spriteStateString = "Left";
                keypress = true;
            }
            else if (ks.IsKeyDown(Keys.Right))
            {
                dp.X += this.MovementSpeed.X;
                spriteStateString = "Right";
                keypress = true;
            }

            if (!keypress)
            {
                spriteStateString = "Idle";
            }
            
            this.Body.LinearVelocity += dp;

            foreach (var sprite in this.Sprites.Values)
            {
                sprite.CurrentAnimationString = spriteStateString;
            }
            
            base.Update(time);
        }
        
        public XElement XmlSerialize()
        {            
            throw new NotImplementedException();
        }

        public void XmlDeserialize(XElement classData)
        {
            if (classData != null)
            {
                Texture2D texture = ContentController.Instance.GetContent<Texture2D>(classData.Attribute("textureref").Value);

                GameSprite testSprite = new GameSprite(texture, new Size(19, 40));
                testSprite.AddAnimation(StandardAnimation.Idle, new SpriteAnimation(0, 1, 1));
                testSprite.AddAnimation(StandardAnimation.Down, new SpriteAnimation(0, 8, 1));
                testSprite.AddAnimation(StandardAnimation.Up, new SpriteAnimation(0, 8, 1) { FlipVertical = true });
                testSprite.AddAnimation(StandardAnimation.Right, new SpriteAnimation(1, 8, 1));
                testSprite.AddAnimation(StandardAnimation.Left, new SpriteAnimation(1, 8, 1) { FlipHorizontal = true });
                testSprite.CurrentAnimationString = StandardAnimation.Idle.ToString();

                Player test = new Player();
                test.AddSprite("test", testSprite);

                test.Body = FarseerPhysics.Factories.BodyFactory.CreateRectangle(MainGame.World, 19f, 40f, 1f);
                test.Body.BodyType = BodyType.Dynamic;
                test.Body.CollidesWith = Category.All;
                test.Body.FixedRotation = true;
                test.Body.Friction = 2f;
                test.Position = Vector2.Zero;

                MainGame.RegisterActor(test);
            }
        }

        private bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            return true;
        }
    }
}
