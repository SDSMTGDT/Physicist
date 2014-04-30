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
        private int nextRotationTime;
        private int markedMilliseconds;

        public Player() :
            base()
        {
            this.RotatesWithWorld = false;
            this.RotationTiming = 10;
            this.nextRotationTime = 0;
            this.RotationSpeed = .02f;
        }

        public Player(string name) :
            base(name)
        {
            this.RotatesWithWorld = false;
            this.RotationTiming = 10;
            this.nextRotationTime = 0;
            this.RotationSpeed = .02f;
        }

        public float RotationSpeed { get; set; }

        public int RotationTiming { get; set; }

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
            if (gameTime != null)
            {
                this.markedMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
            }

            bool keypress = false;
            string spriteStateString = string.Empty;
            Vector2 dp = Vector2.Zero;

            this.MovementSpeed = new Vector2(1, 1);

            if (this.Screen.IsKeyDown(KeyboardController.JumpKey, true))
            {
                this.Body.LinearVelocity += Vector2.Transform(new Vector2(0, -1000), Matrix.CreateRotationZ(-1 * this.Screen.ScreenRotation));
            }

            if (this.Screen.IsKeyDown(KeyboardController.UpKey))
            {
                dp -= Vector2.Transform(new Vector2(0, this.MovementSpeed.Y), Matrix.CreateRotationZ(-1 * this.Screen.ScreenRotation));
                spriteStateString = "Up";
                keypress = true;
            }
            else if (this.Screen.IsKeyDown(KeyboardController.DownKey))
            {
                dp += Vector2.Transform(new Vector2(0, this.MovementSpeed.Y), Matrix.CreateRotationZ(-1 * this.Screen.ScreenRotation));
                spriteStateString = "Down";
                keypress = true;
            }
            else if (this.Screen.IsKeyDown(KeyboardController.LeftKey))
            {
                dp -= Vector2.Transform(new Vector2(this.MovementSpeed.X, 0), Matrix.CreateRotationZ(-1 * this.Screen.ScreenRotation));
                spriteStateString = "Left";
                keypress = true;
            }
            else if (this.Screen.IsKeyDown(KeyboardController.RightKey))
            {
                dp += Vector2.Transform(new Vector2(this.MovementSpeed.X, 0), Matrix.CreateRotationZ(-1 * this.Screen.ScreenRotation));
                spriteStateString = "Right";
                keypress = true;
            }

            if (!keypress)
            {
                spriteStateString = "Idle";
            }

            if (this.Screen.IsKeyDown(KeyboardController.RotateRightKey))
            {
                if (this.markedMilliseconds > this.nextRotationTime)
                {
                    this.Screen.RotateWorld(-this.RotationSpeed);
                    this.nextRotationTime = this.markedMilliseconds + this.RotationTiming;
                }
            }

            if (this.Screen.IsKeyDown(KeyboardController.RotateLeftKey))
            {
                if (this.markedMilliseconds > this.nextRotationTime)
                {
                    this.Screen.RotateWorld(this.RotationSpeed);
                    this.nextRotationTime = this.markedMilliseconds + this.RotationTiming;
                }
            }

            this.Body.LinearVelocity += dp;

            foreach (var sprite in this.Sprites.Values)
            {
                sprite.CurrentAnimationString = spriteStateString;
            }

            this.Body.Rotation = this.Screen.ScreenRotation;

            this.Screen.IsKeyDown(KeyboardController.UpKey, false);
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
