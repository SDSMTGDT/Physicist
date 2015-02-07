namespace Physicist.Actors.NPCs
{
    using System;
    using System.Linq;
    using System.Xml.Linq;
    using FarseerPhysics.Collision;
    using FarseerPhysics.Dynamics;
    using FarseerPhysics.Dynamics.Contacts;
    using FarseerPhysics.Factories;
    using Microsoft.Xna.Framework;
    using Physicist.Controls;
    using Physicist.Events;
    using Physicist.Extensions;
    
    public class Slug : Enemy
    {
        private float distanceTraveled = 0;
        private bool moveRight = false;
        private string startDirection = "Right";
        private Vector2 prevPosition;

        private ProximityTrigger leftButton;
        private ProximityTrigger rightButton;
        private ProximityTrigger leftDetectionButton;
        private ProximityTrigger rightDetectionButton;
        private ProximityTrigger bottomRightDetectionButton;
        private ProximityTrigger bottomLeftDetectionButton;
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

        //bool isRotating = false;
        //object contact;
        //FarseerPhysics.Dynamics.Joints.PrismaticJoint weld;
        protected override void NormalUpdate(GameTime gameTime)
        {
            if (gameTime != null)
            {
                var gStep = ((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);
                // Always step along the line of our rotation
                var dp = gStep * Vector2.Transform(new Vector2(this.MovementSpeed.X * (this.MoveRight ? 1 : -1), 0), Matrix.CreateRotationZ(this.Rotation));

                // Get effective velocity step due to gravity
                var grav_ang = (float)this.World.Gravity.Angle() - MathHelper.PiOver2;
                var gravity_effect = this.Body.IgnoreGravity ? Vector2.Zero : Vector2.Transform(new Vector2(0, (Vector2.Transform(this.Body.LinearVelocity, Matrix.CreateRotationZ(grav_ang)).Y)), Matrix.CreateRotationZ(-grav_ang));
  
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

                //var step = 0.05f;
                //if (this.StickToWalls)
                //{
                //    this.Body.IgnoreGravity = (this.bottomLeftDetectionButton.IsActive || this.bottomRightDetectionButton.IsActive || this.bottomButton.IsActive);
                //    var horizontalDetection = this.MoveRight ? this.rightDetectionButton : this.leftDetectionButton;
                //    var primaryVerticalDetection = this.MoveRight ? this.bottomRightDetectionButton : this.bottomLeftDetectionButton;
                //    var secondaryVerticalDetection = this.MoveRight ? this.bottomLeftDetectionButton : this.bottomRightDetectionButton;

                //    if (weld != null)
                //    {
                //        this.World.RemoveJoint(weld);
                //    }

                //    if (!isRotating && horizontalDetection.IsActive && horizontalDetection.ActivationData.HasValue)
                //    {
                //        var hcontact = horizontalDetection.ActivationData.Value.Data as Contact;
                //        if ((hcontact != null) && (hcontact.FixtureA != null) && (hcontact.FixtureA.Body != null) && (hcontact.FixtureA.Body.UserData is IMapObject || hcontact.FixtureA.Body.UserData is Map))
                //        {

                //            isRotating = true;
                //            contact = hcontact.FixtureA.Body.UserData;
                //        }
                //    }
                //    else if(isRotating)
                //    {
                //        if(secondaryVerticalDetection.IsActive)
                //        {
                //            var vcontact = secondaryVerticalDetection.ActivationData.Value.Data as Contact;
                //            if((vcontact != null) && (vcontact.FixtureA != null) && (vcontact.FixtureA.Body != null) && vcontact.FixtureA.Body.UserData.Equals(contact))
                //            {
                //                isRotating = false;

                //                var downVect = Vector2.Transform(new Vector2(0, -5), Matrix.CreateRotationZ(-this.Rotation));
                //                weld = FarseerPhysics.Factories.JointFactory.CreatePrismaticJoint(
                //                    this.World, 
                //                    this.Body, 
                //                    vcontact.FixtureA.Body,
                //                    (this.Body.WorldCenter - vcontact.FixtureA.Body.WorldCenter + downVect).ToSimUnits(), 
                //                    Vector2.UnitY);

                //                this.Body.RestoreCollisionWith(vcontact.FixtureA.Body);
                //            }
                //        }
                //        if(isRotating)
                //        {
                //            this.Rotation += this.MoveRight ? -step : step;
                //        }
                //    }
                //}

                //if (!isRotating)
                //{
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
                //}

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

                    //Fixture bottomRightDetectionFixture = FixtureFactory.AttachRectangle(aabb.Width * 0.3f, 1, 0, new Vector2(aabb.Width * 0.35f, (aabb.Height / 2f) + 2), buttonBody);
                    //this.bottomRightDetectionButton = new ProximityTrigger(this.Body, bottomRightDetectionFixture, this.World);
                    //this.bottomRightDetectionButton.Initialize(null);
                    //buttonBody.DestroyFixture(bottomRightDetectionFixture);

                    //Fixture bottomLeftDetectionFixture = FixtureFactory.AttachRectangle(aabb.Width * 0.3f, 1, 0, new Vector2(-aabb.Width * 0.35f, (aabb.Height / 2f) + 2), buttonBody);
                    //this.bottomLeftDetectionButton = new ProximityTrigger(this.Body, bottomLeftDetectionFixture, this.World);
                    //this.bottomLeftDetectionButton.Initialize(null);
                    //buttonBody.DestroyFixture(bottomLeftDetectionFixture);

                    //Fixture leftDetectionFixture = FixtureFactory.AttachRectangle(1, aabb.Height * 0.7f, 0, new Vector2(-aabb.Width * 0.60f, 0), buttonBody);
                    //this.leftDetectionButton = new ProximityTrigger(this.Body, leftDetectionFixture, this.World);
                    //this.leftDetectionButton.Initialize(null);
                    //buttonBody.DestroyFixture(leftDetectionFixture);

                    //Fixture rightDetectionFixture = FixtureFactory.AttachRectangle(1, aabb.Height * 0.7f, 0, new Vector2(aabb.Width * 0.60f, 0), buttonBody);
                    //this.rightDetectionButton = new ProximityTrigger(this.Body, rightDetectionFixture, this.World);
                    //this.rightDetectionButton.Initialize(null);
                    //buttonBody.DestroyFixture(rightDetectionFixture);
                }
            }
        }
    }
}
