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
using Physicist.Events;
    using FarseerPhysics.Collision;
    using FarseerPhysics.Factories;

    public class Player : Actor
    {
        private bool rotating = false;
        private int nextRotationTime;
        private int jumpEndTime;
        private float dampening = 1.1f;
        private int markedMilliseconds;
        private int passedJumpMilliseconds;
        private ProximityTrigger headButton = null;
        private ProximityTrigger footButton = null;

        public Player() :
            base()
        {
            this.RotatesWithWorld = false;
            this.RotationTiming = 10;
            this.JumpTiming = 25;
            this.JumpSpeed = 150;
            this.nextRotationTime = 0;
            this.RotationSpeed = .02f;
            this.MaxSpeed = 100f;
        }

        public Player(string name) :
            base(name)
        {
            this.RotatesWithWorld = false;
            this.RotationTiming = 10;
            this.JumpTiming = 50;
            this.JumpSpeed = 150;
            this.nextRotationTime = 0;
            this.RotationSpeed = .02f;
            this.MaxSpeed = 100f;
        }

        public float RotationSpeed { get; set; }

        public int RotationTiming { get; set; }

        public int JumpTiming { get; set; }

        public int JumpSpeed { get; set; }

        public float MaxSpeed { get; set; }

        public Vector2 MovementVelocity { get; set; }

        public new Body Body
        {
            get
            {
                return base.Body;
            }

            set
            {
                this.CreateButtons();
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

            this.MovementSpeed = new Vector2(5, 5);

            if (this.Screen.IsKeyDown(KeyboardController.LeftKey))
            {
                if (dp.Length() < this.MaxSpeed)
                {
                    var speedMod = Vector2.Transform(new Vector2(this.MovementSpeed.X, 0), Matrix.CreateRotationZ(-1 * this.Screen.ScreenRotation));
                    if(!this.footButton.IsActive)
                    {
                        speedMod /= 5;
                    }
                    dp -= speedMod;
                }

                spriteStateString = "Left";
                keypress = true;
            }
            else if (this.Screen.IsKeyDown(KeyboardController.RightKey))
            {
                if (dp.Length() < this.MaxSpeed)
                {
                    var speedMod = Vector2.Transform(new Vector2(this.MovementSpeed.X, 0), Matrix.CreateRotationZ(-1 * this.Screen.ScreenRotation));
                    if (!this.footButton.IsActive)
                    {
                        speedMod /= 5;
                    }
                    dp += speedMod;
                }

                spriteStateString = "Right";
                keypress = true;
            }
            else if(this.footButton.IsActive)
            {
                // Dampen the horizontal velocity to simulate the player stopping itself from sliding around
                Vector2 newVelocity = this.Body.LinearVelocity;
             
                newVelocity = Vector2.Transform(newVelocity, Matrix.CreateRotationZ(this.Screen.ScreenRotation));
                newVelocity.X /= this.dampening;
                newVelocity = Vector2.Transform(newVelocity, Matrix.CreateRotationZ(-1 * this.Screen.ScreenRotation));
             
                this.Body.LinearVelocity = newVelocity;
            }

            if (!keypress)
            {
                spriteStateString = "Idle";
            }

            if (this.Screen.IsKeyDown(KeyboardController.RotateLeftKey))
            {
                this.rotating = true;

                if (this.markedMilliseconds > this.nextRotationTime)
                {
                    this.Body.Awake = true;
                    this.Screen.RotateWorld(-this.RotationSpeed);
                    this.nextRotationTime = this.markedMilliseconds + this.RotationTiming;
                }
            }
            else if (this.Screen.IsKeyDown(KeyboardController.RotateRightKey))
            {
                this.rotating = true;

                if (this.markedMilliseconds > this.nextRotationTime)
                {
                    this.Body.Awake = true;
                    this.Screen.RotateWorld(this.RotationSpeed);
                    this.nextRotationTime = this.markedMilliseconds + this.RotationTiming;
                }
            }
            else
            {
                this.rotating = false;
            }

            this.Body.LinearVelocity += dp;

            // Jumping in progress
            if (this.jumpEndTime > 0)
            {
                this.passedJumpMilliseconds += this.JumpTiming;
                if (this.passedJumpMilliseconds >= this.jumpEndTime)
                {
                    this.Body.GravityScale = 4;
                    this.passedJumpMilliseconds = 0;
                    this.jumpEndTime = 0;

                    // remove the jump velocity
                    Vector2 newVelocity = this.Body.LinearVelocity;

                    newVelocity = Vector2.Transform(newVelocity, Matrix.CreateRotationZ(this.Screen.ScreenRotation));
                    newVelocity.Y /= 2;
                    newVelocity = Vector2.Transform(newVelocity, Matrix.CreateRotationZ(-1 * this.Screen.ScreenRotation));

                    this.Body.LinearVelocity = newVelocity;
                }
            }
            else
            {
                this.Body.GravityScale = 4;
            }

            if (this.Screen.IsKeyDown(KeyboardController.JumpKey, true) && this.footButton.IsActive)
            {
                if (this.jumpEndTime == 0)
                {
                    this.jumpEndTime = 1000;
                    this.Body.GravityScale = 0;
                    this.Body.LinearVelocity += Vector2.Transform(new Vector2(0, -1 * this.JumpSpeed), Matrix.CreateRotationZ(-1 * this.Screen.ScreenRotation));
                }
            }

            if(! this.Screen.IsKeyDown(KeyboardController.JumpKey, false) || this.headButton.IsActive)
            {
                if (this.jumpEndTime != 0 && this.jumpEndTime - 200 > this.passedJumpMilliseconds)
                      this.passedJumpMilliseconds = this.jumpEndTime - 200;
            }

            foreach (var sprite in this.Sprites.Values)
            {
                sprite.CurrentAnimationString = spriteStateString;
            }

            // rotate the body and move the sprite so it is drawn in the correct position
            this.Body.Rotation = (float)(2 * Math.PI) - this.Screen.ScreenRotation;

            // A temporary rotational fix.  Need to change origin of rotation in actuality:
            Vector2 rotationSpriteOffset = new Vector2();
            foreach (var sprite in this.Sprites.Values)
            {
                // the offset calculation
                rotationSpriteOffset.X = (float)(Math.Sin(Math.PI - (this.Screen.ScreenRotation / 2)) * sprite.CurrentSprite.Width) - (float)(Math.Sin(this.Screen.ScreenRotation) * sprite.CurrentSprite.Width);
                rotationSpriteOffset.Y = (float)(Math.Sin(Math.PI - (this.Screen.ScreenRotation / 2)) * sprite.CurrentSprite.Height);
                
                // bizzare x offset of 3
                rotationSpriteOffset.X += (float)(Math.Abs(Math.Sin(this.Screen.ScreenRotation)) * (-3));

                // weird -width y offset at 3 pi / 2
                if (this.Screen.ScreenRotation > Math.PI)
                {
                    rotationSpriteOffset.Y += (float)(Math.Sin(this.Screen.ScreenRotation) * sprite.CurrentSprite.Width);
                }

                sprite.Offset = rotationSpriteOffset;

                // weird x offset of 2 at pi / 2
                if (this.Screen.ScreenRotation < Math.PI)
                {
                    rotationSpriteOffset.X += (float)(Math.Sin(this.Screen.ScreenRotation) * -2);
                }

                sprite.Offset = rotationSpriteOffset;
            }

            if (this.rotating)
            {
                this.Body.GravityScale = 0;
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
                this.CreateButtons();
            }
        }

        private bool Body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            return true;
        }

        private void CreateButtons()
        {
            this.Body.CollisionCategories = PhysicistCategory.Player;
            AABB aabb;
            this.Body.FixtureList[0].GetAABB(out aabb, 0);

            if (this.Position != null)
            {
                Fixture headButtonFixture = FixtureFactory.AttachRectangle(aabb.Width * 9f / 10f, 1, 0, new Vector2(0, -aabb.Height).ToSimUnits(), new Body(this.World));
                Fixture footButtonFixture = FixtureFactory.AttachRectangle(aabb.Width * 9f / 10f, 1, 0, new Vector2(0, aabb.Height).ToSimUnits(), new Body(this.World));

                this.headButton = new ProximityTrigger(this.Body, headButtonFixture);
                headButton.Initialize(null);

                this.footButton = new ProximityTrigger(this.Body, footButtonFixture);
                footButton.Initialize(null);
            }
        }
    }
}
