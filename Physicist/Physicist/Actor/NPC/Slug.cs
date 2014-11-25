namespace Physicist.Actors.NPCs
{
    using System;
    using System.Linq;
    using System.Xml.Linq;
    using FarseerPhysics.Dynamics;
    using FarseerPhysics.Dynamics.Contacts;
    using Microsoft.Xna.Framework;
    using Physicist.Extensions;

    public class Slug : Enemy
    {
        private float distanceTraveled = 0;
        private bool moveRight = false;
        private string startDirection = "Right";

        public Slug()
        {
        }

        public Slug(string name) :
            base(name)
        {
        }

        public bool FollowDistance { get; set; }

        public int TravelDistance { get; set; }

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
                base.XmlDeserialize(element.Element("Enemy"));

                this.MoveRight = string.Compare(this.startDirection, "Right", StringComparison.CurrentCulture) == 0;
            }
        }

        protected override bool OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (!this.FollowDistance)
            {
                this.MoveRight = !this.MoveRight;
            }

            return base.OnCollision(fixtureA, fixtureB, contact);
        }

        protected override void NormalUpdate(GameTime gameTime)
        {
            var dp = Vector2.Transform(new Vector2(this.MovementSpeed.X * (this.MoveRight ? 1 : -1), 0), Matrix.CreateRotationZ(-1 * this.Screen.ScreenRotation));

            if (Math.Abs((this.Body.LinearVelocity + dp).X) > this.MaxSpeed)
            {
                this.Body.LinearVelocity = new Vector2(this.Body.LinearVelocity.UnitVector().X * this.MaxSpeed, this.Body.LinearVelocity.Y + dp.Y);
            }
            else
            {
                this.Body.LinearVelocity += dp;
            }


            if (this.FollowDistance)
            {
                this.distanceTraveled += this.MovementSpeed.X;
                if (this.distanceTraveled > this.TravelDistance)
                {
                    this.distanceTraveled = 0;
                    this.MoveRight = !this.MoveRight;
                }
            }
        }
    }
}
