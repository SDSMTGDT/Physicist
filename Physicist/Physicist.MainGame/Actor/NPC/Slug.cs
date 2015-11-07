namespace Physicist.MainGame.Actors.NPCs
{
    using System;
    using System.Xml.Linq;
    using FarseerPhysics.Collision;
    using FarseerPhysics.Dynamics;
    using FarseerPhysics.Factories;
    using Microsoft.Xna.Framework;
    using Physicist.MainGame.Extensions;
    using Physicist.Events;
    using Physicist.Types.Util;
    
    public class Slug : Enemy
    {
        private float distanceTraveled = 0;
        private bool moveRight = false;
        private string startDirection = "Right";
        private Vector2 prevPosition;

        private ProximityTrigger leftButton;
        private ProximityTrigger rightButton;
        private ProximityTrigger topButton;
        private ProximityTrigger bottomButton;

        public Slug()
        {
        }

        public Slug(string name) :
            base(name)
        {
            this.prevPosition = this.Body.Position;
            this.CreateButtons();
        }

        public bool FollowDistance { get; set; }

        public int TravelDistance { get; set; }

        public bool StickToWalls { get; set; }

        private bool MoveRight 
        {
            get
            {
                return this.moveRight;
            }

            set
            {
                this.moveRight = value;
                this.Sprites.Values.ForEach(gs => gs.CurrentAnimationString = this.moveRight ? "Right" : "Left");
            }
        }

        public override XElement XmlSerialize()
        {
            return new XElement(
                                "Slug", 
                                new XAttribute("startDirection", this.startDirection),
                                new XAttribute("followDistance", this.FollowDistance),
                                new XAttribute("travelDistance", this.TravelDistance),
                                base.XmlSerialize());
        }

        public override void XmlDeserialize(XElement element)
        {
            if (element != null)
            {
                this.startDirection = element.GetAttribute("startDirection", "Right");
                this.FollowDistance = element.GetAttribute("followDistance", false);
                this.TravelDistance = element.GetAttribute("travelDistance", 0);
                this.StickToWalls = element.GetAttribute("stickToWalls", false);

                base.XmlDeserialize(element.Element("Enemy"));

                this.MoveRight = string.Compare(this.startDirection, "Right", StringComparison.CurrentCulture) == 0;
                this.prevPosition = this.Body.Position;
                this.CreateButtons();
            }
        }

        protected override void NormalUpdate(GameTime gameTime)
        {
            if (gameTime != null)
            {
                var gravityStep = (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f;

                // Always step along the line of our rotation
                var dp = gravityStep * Vector2.Transform(new Vector2(this.MovementSpeed.X * (this.MoveRight ? 1 : -1), 0), Matrix.CreateRotationZ(this.Rotation));

                // Get effective velocity step due to gravity
                var grav_ang = (float)this.World.Gravity.Angle() - MathHelper.PiOver2;
                var gravity_effect = this.Body.IgnoreGravity ? Vector2.Zero : Vector2.Transform(new Vector2(0, Vector2.Transform(this.Body.LinearVelocity, Matrix.CreateRotationZ(grav_ang)).Y), Matrix.CreateRotationZ(-grav_ang));
 
                // Remove gravity from linear speed vector
                var lv_rem_grav = this.Body.LinearVelocity - gravity_effect;

                if ((lv_rem_grav + dp).Length() >= this.MaxSpeed)
                {
                    this.Body.LinearVelocity = gravity_effect + ((lv_rem_grav + dp).UnitVector() * this.MaxSpeed);
                }
                else
                {
                    this.Body.LinearVelocity += dp;
                }

                if (this.FollowDistance)
                {
                    this.distanceTraveled += (this.Body.Position - this.prevPosition).Length();
                    if (this.distanceTraveled > this.TravelDistance)
                    {
                        this.distanceTraveled = 0;
                        this.MoveRight = !this.MoveRight;
                    }
                }

                if (this.leftButton.IsActive)
                {
                    this.MoveRight = true;
                    this.distanceTraveled = 0;
                }
                else if (this.rightButton.IsActive)
                {
                    this.MoveRight = false;
                    this.distanceTraveled = 0;
                }

                this.prevPosition = this.Body.Position;
            }
        }

        private void CreateButtons()
        {
            AABB aabb;
            this.Body.FixtureList[0].GetAABB(out aabb, 0);

            if (this.Position != null)
            {
                using (var buttonBody = new Body(this.World))
                {
                    buttonBody.CollidesWith = PhysicistCategory.AllIgnoreFields;
                    buttonBody.CollisionCategories = PhysicistCategory.Enemy1;

                    Fixture rightButtonFixture = FixtureFactory.AttachRectangle(1, aabb.Height * 0.8f, 0, new Vector2(aabb.Width, 0) / 2f, buttonBody);
                    this.rightButton = new ProximityTrigger(this.Body, rightButtonFixture, this.World);
                    this.rightButton.Initialize(null);
                    this.rightButton.IsSensor = false;
                    this.rightButton.IsContinuous = false;
                    buttonBody.DestroyFixture(rightButtonFixture);

                    Fixture leftButtonFixture = FixtureFactory.AttachRectangle(1, aabb.Height * 0.8f, 0, new Vector2(-aabb.Width, 0) / 2f, buttonBody);
                    this.leftButton = new ProximityTrigger(this.Body, leftButtonFixture, this.World);
                    this.leftButton.Initialize(null);
                    this.leftButton.IsSensor = false;
                    this.leftButton.IsContinuous = false;
                    buttonBody.DestroyFixture(leftButtonFixture);

                    Fixture topButtonFixture = FixtureFactory.AttachRectangle(aabb.Width * 0.8f, 1, 0, new Vector2(0, -aabb.Height) / 2f, buttonBody);
                    this.topButton = new ProximityTrigger(this.Body, topButtonFixture, this.World);
                    this.topButton.Initialize(null);
                    this.topButton.IsSensor = false;
                    this.topButton.IsContinuous = false;
                    buttonBody.DestroyFixture(topButtonFixture);

                    Fixture bottomButtonFixture = FixtureFactory.AttachRectangle(aabb.Width * 0.8f, 1, 0, new Vector2(0, aabb.Height) / 2f, buttonBody);
                    this.bottomButton = new ProximityTrigger(this.Body, bottomButtonFixture, this.World);
                    this.bottomButton.Initialize(null);
                    this.bottomButton.IsSensor = false;
                    this.bottomButton.IsContinuous = false;
                    buttonBody.DestroyFixture(bottomButtonFixture);
                }
            }
        }
    }
}
