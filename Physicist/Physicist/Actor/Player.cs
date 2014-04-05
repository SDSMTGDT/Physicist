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

        public CameraController PlayerCamera { get; set; }

        public Player(XElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            this.XmlDeserialize(element);
        }

        public Player() :
            base()
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

        public void Update(GameTime time, KeyboardState ks)
        {
            bool keypress = false;
            string spriteStateString = string.Empty;
            Vector2 dp = Vector2.Zero;

            if (ks.IsKeyDown(Keys.Up))
            {
                dp -= new Vector2(0, this.MovementSpeed.Y);
                spriteStateString = "Up";
                keypress = true;
            }
            else if (ks.IsKeyDown(Keys.Down))
            {
                dp += new Vector2(0, this.MovementSpeed.Y);
                spriteStateString = "Down";
                keypress = true;
            }
            else if (ks.IsKeyDown(Keys.Left))
            {
                dp -= new Vector2(this.MovementSpeed.X, 0);
                spriteStateString = "Left";
                keypress = true;
            }
            else if (ks.IsKeyDown(Keys.Right))
            {
                //dp += this.MovementSpeed.X;
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

        public new XElement XmlSerialize()
        {
            return new XElement("Player", new XAttribute("class", typeof(Player).ToString()), base.XmlSerialize());
        }

        public new void XmlDeserialize(XElement element)
        {            
            if (element != null)
            {
                base.XmlDeserialize(element.Element("Actor"));
            }
        }

        private bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            return true;
        }

        public void SetWorldRotation(float theta)
        {
            Vector2 OldGravity = MainGame.World.Gravity;
            MainGame.World.Gravity = Vector2.Transform(OldGravity, Matrix.CreateRotationZ(theta));
            PlayerCamera.Rotation = theta;
        }

        public void RotateWorld(float theta)
        {
            SetWorldRotation(PlayerCamera.Rotation + theta);
        }

        public void ResetCameraGravity()
        {
            MainGame.World.Gravity = new Vector2(0, 9.81f);
            PlayerCamera.Reset();
        }

    }
}
