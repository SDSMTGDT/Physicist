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

        private float LastRotationTime;

        public Player() :
            base()
        {
            this.RotatesWithWorld = false;
        }

        public Player(string name) :
            base(name)
        {
            this.RotatesWithWorld = false;
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

            this.MovementSpeed = new Vector2(1, 1);

            if (this.Screen.IsKeyDown(Keys.Up, false))
            {
                dp -= Vector2.Transform(new Vector2(0, this.MovementSpeed.Y), Matrix.CreateRotationZ(-1 * this.Screen.ScreenRotation));
                spriteStateString = "Up";
                keypress = true;
            }
            else if (this.Screen.IsKeyDown(Keys.Down))
            {
                dp += Vector2.Transform(new Vector2(0, this.MovementSpeed.Y), Matrix.CreateRotationZ(-1 * this.Screen.ScreenRotation));
                spriteStateString = "Down";
                keypress = true;
            }
            else if (this.Screen.IsKeyDown(Keys.Left))
            {
                dp -= Vector2.Transform(new Vector2(this.MovementSpeed.X, 0), Matrix.CreateRotationZ(-1 * this.Screen.ScreenRotation));
                spriteStateString = "Left";
                keypress = true;
            }
            else if (this.Screen.IsKeyDown(Keys.Right))
            {
                dp += Vector2.Transform(new Vector2(this.MovementSpeed.X, 0), Matrix.CreateRotationZ(-1 * this.Screen.ScreenRotation));
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

            this.Body.Rotation = this.Screen.ScreenRotation;

            this.Screen.RotateWorld(.001f);
            
               

            this.Screen.IsKeyDown(Keys.A, false);
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
