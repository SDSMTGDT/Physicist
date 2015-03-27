namespace Physicist.Actors
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Linq;
    using Microsoft.Xna.Framework;
    using Physicist.Enums;
    using Physicist.Extensions;

    public class ApproachPositionPathNode : PathNode
    {
        public ApproachPositionPathNode()
        {
            this.Precision = 2f;
        }

        public ApproachPositionPathNode(Actor target, Vector2 position, float speed)
            : base(target)
        {
            this.TargetLocation = position;
            this.Speed = speed;
        }

        public Vector2 TargetLocation
        {
            get;
            private set;
        }

        public float Speed { get; set; }

        public bool DisableAfterPathing { get; set; }

        public bool HideAtEndOfPath { get; set; }

        public float Precision { get; set; }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (this.IsActive && this.TargetActor.IsEnabled)
            {
                if (this.TargetLocation != null)
                {
                    Vector2 delta = this.TargetLocation - this.TargetActor.Position;

                    if ((int)delta.Length() > this.Precision)
                    {
                        delta.Normalize();
                        delta *= this.Speed;
                        this.TargetActor.Body.LinearVelocity = delta;
                    }
                    else
                    {
                        this.TargetActor.Position = this.TargetLocation;

                        if (this.DisableAfterPathing)
                        {
                            this.TargetActor.IsEnabled = false;
                        }

                        if (this.HideAtEndOfPath)
                        {
                            this.TargetActor.VisibleState = Visibility.Hidden;
                        }

                        this.IsActive = false;
                        this.TargetActor.Body.LinearVelocity = Vector2.Zero;
                    }
                }
            }
        }

        public override XElement XmlSerialize()
        {
            return new XElement(
                "ApproachPositionPathNode",
                new XAttribute("speed", this.Speed),
                new XAttribute("precision", this.Precision),
                new XAttribute("disableAfterPathing", this.DisableAfterPathing),
                new XAttribute("hideAfterPathing", this.HideAtEndOfPath),
                ExtensionMethods.XmlSerialize(new Vector2(this.TargetLocation.X, this.Map.Height - this.TargetLocation.Y), "Position"),
                base.XmlSerialize());
        }

        public override void XmlDeserialize(XElement element)
        {
            if (element != null)
            {
                base.XmlDeserialize(element.Element("PathNode"));

                this.Speed = element.GetAttribute("speed", 0);

                this.DisableAfterPathing = element.GetAttribute("disableAfterPathing", false);

                this.HideAtEndOfPath = element.GetAttribute("hideAfterPathing", false);

                this.Precision = element.GetAttribute("precision", 2f);

                var designPosition = ExtensionMethods.XmlDeserializeVector2(element.Element("Position"));
                this.TargetLocation = new Vector2(designPosition.X, this.Map.Height - designPosition.Y);
            }
        }
    }
}