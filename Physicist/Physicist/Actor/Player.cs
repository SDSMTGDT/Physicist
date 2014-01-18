namespace Physicist.Actors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using FarseerPhysics;
    using FarseerPhysics.Dynamics;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    public class Player : Actor
    {
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

        private bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {            
            return true;
        }
    }
}
