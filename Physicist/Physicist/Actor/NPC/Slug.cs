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
        private ProximityTrigger bottomRightButton;
        private ProximityTrigger bottomLeftButton;
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


                this.StickToWalls = true;
                

                base.XmlDeserialize(element.Element("Enemy"));

                this.MoveRight = string.Compare(this.startDirection, "Right", StringComparison.CurrentCulture) == 0;
                this.prevPosition = this.Body.Position;
                this.CreateButtons();
            }
        }
        bool turn = false;

        protected override void NormalUpdate(GameTime gameTime)
        {
            var dp = Vector2.Transform(new Vector2(this.MovementSpeed.X * (this.MoveRight ? 1 : -1), 0), Matrix.CreateRotationZ(this.Rotation));

            if (Math.Abs(Vector2.Transform(this.Body.LinearVelocity + dp, Matrix.CreateRotationZ(-this.Rotation)).X) > this.MaxSpeed)
            {
                this.Body.LinearVelocity = dp.UnitVector() * this.MaxSpeed;
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

            //if (this.StickToWalls)
            //{
            //    this.Body.IgnoreGravity = this.bottomLeftButton.IsActive || this.bottomRightButton.IsActive || this.bottomButton.IsActive;

            //    var button = this.MoveRight ? this.rightButton : this.leftButton;
            //    var bottomButton = this.MoveRight ? this.bottomRightButton : this.bottomLeftButton;
            //    if (!this.turn && button.IsActive && button.ActivationData.HasValue)
            //    {
            //        var contact = button.ActivationData.Value.Data as Contact;
            //        if (contact != null && contact.FixtureA != null && (contact.FixtureA.Body.UserData is MapObject || contact.FixtureA.Body.UserData is Map))
            //        {
            //            AABB aabb;
            //            this.Body.FixtureList[0].GetAABB(out aabb, 0);
            //            switch (contact.FixtureA.Shape.ShapeType)
            //            {
            //                case FarseerPhysics.Collision.Shapes.ShapeType.Polygon:
            //                    var pshape = (FarseerPhysics.Collision.Shapes.PolygonShape)contact.FixtureA.Shape;
            //                    var verts = pshape.Vertices;
            //                    var path = new PhysicistPath("HugPolygon") { LoopPath = true, World = this.World, Screen = this.Screen, IsEnabled = true, Map = this.Map };
            //                     var index = pshape.Normals.FindIndex(n => n == contact.Manifold.LocalNormal);
            //                    for (int i = 0; i < pshape.Normals.Count; i++)
            //                    {
            //                        path.AddPathNode(new ApproachPositionPathNode(this, contact.FixtureA.Body.Position + pshape.Vertices[(index * i) % pshape.Normals.Count] + new Vector2(aabb.Width, aabb.Height) / 2f, 10));
            //                    }
                                
            //                    this.PathManager.AddPath(path);

            //                    this.turn = true;

            //                    break;

            //                case FarseerPhysics.Collision.Shapes.ShapeType.Chain:
            //                    var cshape = (FarseerPhysics.Collision.Shapes.ChainShape)contact.FixtureA.Shape;
            //                    break;
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //}

            if (this.StickToWalls)
            {
                this.Body.IgnoreGravity = this.bottomLeftButton.IsActive || this.bottomRightButton.IsActive || this.bottomButton.IsActive;
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

                    Fixture bottomRightFixture = FixtureFactory.AttachRectangle(aabb.Width * 0.2f, 3, 0, new Vector2(aabb.Width * 0.40f, aabb.Height / 2f + 2), buttonBody);
                    this.bottomRightButton = new ProximityTrigger(this.Body, bottomRightFixture, this.World);
                    this.bottomRightButton.Initialize(null);
                    buttonBody.DestroyFixture(bottomRightFixture);

                    Fixture bottomLeftFixture = FixtureFactory.AttachRectangle(aabb.Width * 0.2f, 3, 0, new Vector2(-aabb.Width * 0.40f, aabb.Height / 2f + 2), buttonBody);
                    this.bottomLeftButton = new ProximityTrigger(this.Body, bottomLeftFixture, this.World);
                    this.bottomLeftButton.Initialize(null);
                    buttonBody.DestroyFixture(bottomLeftFixture);
                }
            }
        }
    }
}
