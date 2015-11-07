namespace Physicist.MainGame.Actors
{
    using System;
    using System.Xml.Linq;
    using FarseerPhysics.Collision;
    using FarseerPhysics.Dynamics;
    using FarseerPhysics.Factories;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Physicist.Events;
    using Physicist.Types.Controllers;
    using Physicist.Types.Util;
    using Physicist.MainGame.Extensions;

    public class Player : Actor
    {
        private bool rotating = false;
        private int nextRotationTime;
        private int jumpEndTime;
        private float midairDampening = 5f;
        private float groundDampening = 1.1f;
        private int markedRotateMilliseconds;
        private int markedJumpMilliseconds;
        private ProximityTrigger headButton = null;
        private ProximityTrigger footButton = null;
        private string spriteState = "Idle";
        private string rotateSound;

        public Player()
        {
            this.RotatesWithWorld = false;
            this.RotationTiming = 10;
            this.JumpTiming = 500;
            this.JumpSpeed = 150;
            this.nextRotationTime = 0;
            this.RotationSpeed = .02f;
            this.MaxSpeed = 100f;
            this.MovementSpeed = new Vector2(5, 5);
            this.Listener = new AudioListener();
        }

        public Player(string name) :
            base(name)
        {
            this.RotatesWithWorld = false;
            this.RotationTiming = 10;
            this.JumpTiming = 500;
            this.JumpSpeed = 150;
            this.nextRotationTime = 0;
            this.RotationSpeed = .02f;
            this.MaxSpeed = 100f;
            this.MovementSpeed = new Vector2(5, 5);
            this.Listener = new AudioListener();
        }

        public AudioListener Listener { get; set; }

        public float RotationSpeed { get; set; }

        public int RotationTiming { get; set; }

        public int JumpTiming { get; set; }

        public int JumpSpeed { get; set; }

        public float MaxSpeed { get; set; }

        public Vector2 MovementVelocity { get; set; }

        private string SpriteState
        {
            set
            {
                if (string.Compare(this.spriteState, value, StringComparison.CurrentCulture) != 0)
                {
                    this.spriteState = value;

                    // update the animations
                    foreach (var sprite in this.Sprites.Values)
                    {
                        sprite.CurrentAnimationString = this.spriteState;
                    }
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (gameTime != null)
            {
                this.markedRotateMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
                this.markedJumpMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
            }

            // rotate the body and move the sprite so it is drawn in the up direction
            this.Rotation = -this.Screen.ScreenRotation;

            var state = KeyboardController.GetState();

            var dp = this.GetMovementSpeed(state);
            this.Body.LinearVelocity += dp;

            this.rotating = this.GetRotation(state);
            this.GetJump(state);

            // deal with the rotating sound pausing/playing
            if (this.rotating)
            {
                this.Body.GravityScale = 0;
                if (!this.PlayingSounds.ContainsKey(this.rotateSound))
                {
                    this.PlaySound(this.rotateSound, false, true, 1.0f, 0.0f, 0.0f);
                }
                else
                {
                    this.ResumeSound(this.rotateSound);
                }
            }
            else if (this.PlayingSounds.ContainsKey(this.rotateSound))
            {
                if (this.SoundIsPlaying(this.rotateSound))
                {
                    this.PauseSound(this.rotateSound);
                }
            }

            this.Listener.Position = new Vector3(this.Position.X, this.Position.Y, 0);

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
                this.rotateSound = element.GetAttribute("rotateSoundRef", string.Empty);
                base.XmlDeserialize(element.Element("Actor"));
                this.Body.BodyType = BodyType.Dynamic;
                this.Body.FixedRotation = true;
                this.CreateButtons();
            }
        }

        private void GetJump(KeyboardDebouncer state)
        {
            if (this.jumpEndTime > 0)
            {
                if (this.markedJumpMilliseconds >= this.jumpEndTime)
                {
                    this.Body.GravityScale = 4;
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

            if (state.IsKeyDown(KeyboardController.JumpKey) && this.footButton.IsActive)
            {
                if (this.jumpEndTime == 0)
                {
                    this.jumpEndTime = this.JumpTiming;
                    this.markedJumpMilliseconds = 0;
                    this.Body.GravityScale = 0;
                    this.Body.LinearVelocity += Vector2.Transform(new Vector2(0, -1 * this.JumpSpeed), Matrix.CreateRotationZ(-1 * this.Screen.ScreenRotation));
                }
            }

            if (!state.IsKeyDown(KeyboardController.JumpKey) || this.headButton.IsActive)
            {
                if (this.jumpEndTime != 0 && this.jumpEndTime - 200 > this.markedJumpMilliseconds)
                {
                    this.jumpEndTime = this.markedJumpMilliseconds + 200;
                }
            }
        }

        private bool GetRotation(KeyboardDebouncer state)
        {
            bool rotated = false;

            if (state.IsKeyDown(KeyboardController.RotateLeftKey))
            {
                if (this.markedRotateMilliseconds > this.nextRotationTime)
                {
                    this.Body.Awake = true;
                    this.Screen.RotateWorld(-this.RotationSpeed);
                    this.nextRotationTime = this.RotationTiming;
                    this.markedRotateMilliseconds = 0;
                }

                rotated = true;
            }
            else if (state.IsKeyDown(KeyboardController.RotateRightKey))
            {
                if (this.markedRotateMilliseconds > this.nextRotationTime)
                {
                    this.Body.Awake = true;
                    this.Screen.RotateWorld(this.RotationSpeed);
                    this.nextRotationTime = this.RotationTiming;
                    this.markedRotateMilliseconds = 0;
                }

                rotated = true;
            }

            return rotated;
        }

        private Vector2 GetMovementSpeed(KeyboardDebouncer state)
        {
            var dp = Vector2.Zero;
            if (state.IsKeyDown(KeyboardController.LeftKey))
            {
                var speedMod = Vector2.Transform(new Vector2(-1 * this.MovementSpeed.X, 0), Matrix.CreateRotationZ(-1 * this.Screen.ScreenRotation));
                if (!this.footButton.IsActive)
                {
                    speedMod /= this.midairDampening;
                }

                dp += speedMod;
                this.SpriteState = "Left";
            }
            else if (state.IsKeyDown(KeyboardController.RightKey))
            {
                var speedMod = Vector2.Transform(new Vector2(this.MovementSpeed.X, 0), Matrix.CreateRotationZ(-1 * this.Screen.ScreenRotation));
                if (!this.footButton.IsActive)
                {
                    speedMod /= this.midairDampening;
                }

                dp += speedMod;

                this.SpriteState = "Right";
            }
            else if (this.footButton.IsActive)
            {
                // Dampen the horizontal velocity to simulate the player stopping itself from sliding around
                Vector2 newVelocity = this.Body.LinearVelocity;

                newVelocity = Vector2.Transform(newVelocity, Matrix.CreateRotationZ(this.Screen.ScreenRotation));
                newVelocity.X /= this.groundDampening;
                newVelocity = Vector2.Transform(newVelocity, Matrix.CreateRotationZ(-1 * this.Screen.ScreenRotation));

                this.Body.LinearVelocity = newVelocity;
                this.SpriteState = "Idle";
            }
            else
            {
                this.SpriteState = "Idle";
            }

            return dp;
        }

        private void CreateButtons()
        {
            this.Body.CollisionCategories = PhysicistCategory.Player1;
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

                this.Body.BodyType = BodyType.Dynamic;
                this.Body.FixedRotation = true;

                this.Body.CollidesWith = PhysicistCategory.Physical;
                this.Body.CollisionCategories = PhysicistCategory.Physical;
            }
        }
    }
}
