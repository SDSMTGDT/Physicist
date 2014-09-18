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

    public class Player : Actor
    {
        public Player() :
            base()
        {
        }

        public Player(string name) :
            base(name)
        {
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

        public override void Update(GameTime gameTime)
        {
            bool keypress = false;
            string spriteStateString = string.Empty;
            Vector2 dp = Vector2.Zero;

            if (this.Screen.IsKeyDown(Keys.Up, false))
            {
                dp.Y -= this.MovementSpeed.Y;
                spriteStateString = "Up";
                keypress = true;
            }
            else if (this.Screen.IsKeyDown(Keys.Down))
            {
                dp.Y += this.MovementSpeed.Y;
                spriteStateString = "Down";
                keypress = true;
            }
            else if (this.Screen.IsKeyDown(Keys.Left))
            {
                dp.X -= this.MovementSpeed.X;
                spriteStateString = "Left";
                keypress = true;
            }
            else if (this.Screen.IsKeyDown(Keys.Right))
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

            base.Update(gameTime);
        }

        public override XElement XmlSerialize()
        {
            return new XElement("Player", new XAttribute("class", typeof(Player).ToString()), base.XmlSerialize());
        }

        public override void XmlDeserialize(XElement element)
        {            
            if (element != null)
            {
                base.XmlDeserialize(element.Element("Actor"));

                this.Body.BodyType = BodyType.Dynamic;
                this.Body.FixedRotation = true;
            }
        }

        private bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            return true;
        }
    }
}
