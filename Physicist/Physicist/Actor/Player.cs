namespace Physicist.Actors
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;
    using FarseerPhysics;
    using FarseerPhysics.Collision;
    using FarseerPhysics.Dynamics;
    using FarseerPhysics.Factories;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Physicist.Controls;
    using Physicist.Enums;
    using Physicist.Events;
    using Physicist.Extensions;

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
        private string spriteStateString = "Idle";

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
            this.MovementSpeed = new Vector2(5, 5);
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
            this.MovementSpeed = new Vector2(5, 5);
        }

        public float RotationSpeed { get; set; }

        public int RotationTiming { get; set; }

        public int JumpTiming { get; set; }

        public int JumpSpeed { get; set; }

        public float MaxSpeed { get; set; }

        public Vector2 MovementVelocity { get; set; }

        public override void Update(GameTime gameTime)
        {
            if (gameTime != null)
            {
                this.markedMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
            }

            var state = KeyboardController.GetState();
            this.spriteStateString = "Idle";
            this.Body.LinearVelocity += this.GetMovementSpeed(state);
            this.rotating = this.GetRotation(state);
            this.GetJump(state);

            // update the animations
            foreach (var sprite in this.Sprites.Values)
            {
                sprite.CurrentAnimationString = this.spriteStateString;
            }

            // rotate the body and move the sprite so it is drawn in the correct position
            this.Body.Rotation = (float)(2 * Math.PI) - this.Screen.ScreenRotation;

            this.FixOffsetForRotation();

            if (this.rotating)
            {
                this.Body.GravityScale = 0;
            }

            base.Update(gameTime);
        }

        public override XElement XmlSerialize()
        {
            return new XElement(
                "Player",
                new XAttribute("class", "Player"),
                base.XmlSerialize());
        }

        public override void XmlDeserialize(XElement element)
        {
            if (element != null)
            {
                base.XmlDeserialize(element.Element("Actor"));
                this.MovementSpeed = new Vector2(5, 5);
                this.Body.BodyType = BodyType.Dynamic;
                this.Body.FixedRotation = true;
                this.CreateButtons();
            }
        }

        private void FixOffsetForRotation()
        {
            // A temporary rotational fix.  Need to change origin of rotation in actuality:
            var rotationSpriteOffset = new Vector2();
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

                // weird x offset of 2 at pi / 2
                if (this.Screen.ScreenRotation < Math.PI)
                {
                    rotationSpriteOffset.X += (float)(Math.Sin(this.Screen.ScreenRotation) * -2);
                }

                sprite.Offset = rotationSpriteOffset;
            }
        }

        private void GetJump(KeyboardDebouncer state)
        {
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

            if (state.IsKeyDown(KeyboardController.JumpKey, true) && this.footButton.IsActive)
            {
                if (this.jumpEndTime == 0)
                {
                    this.jumpEndTime = 1000;
                    this.Body.GravityScale = 0;
                    this.Body.LinearVelocity += Vector2.Transform(new Vector2(0, -1 * this.JumpSpeed), Matrix.CreateRotationZ(-1 * this.Screen.ScreenRotation));
                }
            }

            if (!state.IsKeyDown(KeyboardController.JumpKey, false) || this.headButton.IsActive)
            {
                if (this.jumpEndTime != 0 && this.jumpEndTime - 200 > this.passedJumpMilliseconds)
                {
                    this.passedJumpMilliseconds = this.jumpEndTime - 200;
                }
            }
        }

        private bool GetRotation(KeyboardDebouncer state)
        {
            if (state.IsKeyDown(KeyboardController.RotateLeftKey))
            {
                if (this.markedMilliseconds > this.nextRotationTime)
                {
                    this.Body.Awake = true;
                    this.Screen.RotateWorld(-this.RotationSpeed);
                    this.nextRotationTime = this.markedMilliseconds + this.RotationTiming;
                }

                return true;
            }

            if (state.IsKeyDown(KeyboardController.RotateRightKey))
            {
                if (this.markedMilliseconds > this.nextRotationTime)
                {
                    this.Body.Awake = true;
                    this.Screen.RotateWorld(this.RotationSpeed);
                    this.nextRotationTime = this.markedMilliseconds + this.RotationTiming;
                }

                return true;
            }

            return false;
        }

        private Vector2 GetMovementSpeed(KeyboardDebouncer state)
        {
            var dp = Vector2.Zero;

            if (state.IsKeyDown(KeyboardController.JumpKey, true))
            {
                if (dp.Length() < this.MaxSpeed)
                {
                    var speedMod = Vector2.Transform(new Vector2(this.MovementSpeed.X, 0), Matrix.CreateRotationZ(-1 * this.Screen.ScreenRotation));
                    if (!this.footButton.IsActive)
                    {
                        speedMod /= 5;
                    }

                    dp -= speedMod;
                }
            }

            if (state.IsKeyDown(KeyboardController.UpKey))
            {
                dp -= Vector2.Transform(new Vector2(0, this.MovementSpeed.Y), Matrix.CreateRotationZ(-1 * this.Screen.ScreenRotation));
                this.spriteStateString = "Up";
            }
            else if (state.IsKeyDown(KeyboardController.DownKey))
            {
                dp += Vector2.Transform(new Vector2(0, this.MovementSpeed.Y), Matrix.CreateRotationZ(-1 * this.Screen.ScreenRotation));
                this.spriteStateString = "Down";
            }
            else if (state.IsKeyDown(KeyboardController.LeftKey))
            {
                dp -= Vector2.Transform(new Vector2(this.MovementSpeed.X, 0), Matrix.CreateRotationZ(-1 * this.Screen.ScreenRotation));
                this.spriteStateString = "Left";
            }
            else if (state.IsKeyDown(KeyboardController.RightKey))
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

                this.spriteStateString = "Right";
            }
            else if (this.footButton.IsActive)
            {
                // Dampen the horizontal velocity to simulate the player stopping itself from sliding around
                Vector2 newVelocity = this.Body.LinearVelocity;
             
                newVelocity = Vector2.Transform(newVelocity, Matrix.CreateRotationZ(this.Screen.ScreenRotation));
                newVelocity.X /= this.dampening;
                newVelocity = Vector2.Transform(newVelocity, Matrix.CreateRotationZ(-1 * this.Screen.ScreenRotation));
             
                this.Body.LinearVelocity = newVelocity;
            }

            return dp;
        }

        private void CreateButtons()
        {
            this.Body.CollisionCategories = PhysicistCategory.Player;
            AABB aabb;
            this.Body.FixtureList[0].GetAABB(out aabb, 0);

            if (this.Position != null)
            {
                using (var headBody = new Body(this.World))
                using (var footBody = new Body(this.World))
                {
                    Fixture headButtonFixture = FixtureFactory.AttachRectangle(aabb.Width * 9f / 10f, 1, 0, new Vector2(0, -aabb.Height).ToSimUnits(), headBody);
                    Fixture footButtonFixture = FixtureFactory.AttachRectangle(aabb.Width * 9f / 10f, 1, 0, new Vector2(0, aabb.Height).ToSimUnits(), footBody);

                    this.headButton = new ProximityTrigger(this.Body, headButtonFixture, null);
                    this.headButton.Initialize(null);

                    this.footButton = new ProximityTrigger(this.Body, footButtonFixture, null);
                    this.footButton.Initialize(null);
                }
            }
        }
    }
}
