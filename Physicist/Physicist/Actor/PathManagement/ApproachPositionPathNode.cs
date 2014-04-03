namespace Physicist.Actors
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Linq;
    using Microsoft.Xna.Framework;
    using Physicist.Extensions;

    public class ApproachPositionPathNode : PathNode
    {
        public ApproachPositionPathNode()
        {
        }

        public ApproachPositionPathNode(XElement element)
        {
            this.XmlDeserialize(element);
        }

        public ApproachPositionPathNode(Actor target, Vector2 position, float speed)
            : base(target)
        {
            this.TargetPosition = position;
            this.Speed = speed;
            this.DeactivateAfterPathing = false;
        }

        public Vector2 TargetPosition
        {
            get;
            private set;
        }

        public float Speed { get; set; }

        public bool DeactivateAfterPathing { get; set; }

        public bool HideAtEndOfPath { get; set; }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (this.IsActive && this.Target.IsEnabled)
            {
                if (!(this.TargetPosition == null))
                {
                    Vector2 delta = this.TargetPosition - this.Target.Position;

                    if (delta.Length() > 2f)
                    {
                        delta.Normalize();
                        delta *= this.Speed;
                        this.Target.Body.LinearVelocity = delta;
                    }
                    else
                    {
                        if (this.DeactivateAfterPathing)
                        {
                            this.Target.IsEnabled = false;
                        }

                        if (this.HideAtEndOfPath)
                        {
                            this.Target.VisibleState = Visibility.Hidden;
                        }

                        this.IsActive = false;
                    }
                }
            }
        }

        public override void XmlDeserialize(XElement element)
        {
            if (element != null)
            {
                base.XmlDeserialize(element.Element("PathNode"));

                var speedAtt = element.Attribute("speed");
                if (speedAtt != null)
                {
                    this.Speed = int.Parse(speedAtt.Value, CultureInfo.CurrentCulture);
                }

                this.TargetPosition = ExtensionMethods.XmlDeserializeVector2(element.Element("Position"));
            }
        }
    }
}
