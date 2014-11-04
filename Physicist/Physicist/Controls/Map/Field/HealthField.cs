namespace Physicist.Controls.Fields
{
    using System;
    using System.Xml.Linq;
    using FarseerPhysics.Dynamics;
    using Microsoft.Xna.Framework;
    using Physicist.Controls;
    using Physicist.Extensions;

    public class HealthField : Field
    {
        private float totalelapsedtime = 0f;

        public HealthField()
        {
        }

        public HealthField(float magnitude, float hitsPerSecond, Body fieldBody)
            : base(new Vector2(magnitude, 0), fieldBody)
        {
            this.HitsPerSecond = hitsPerSecond;
        }

        // Vector2 notes:
        // magnitude of Vector2 is the % of HP affected
        // x-direction of Vector2 : positive - heals, negative - damages
        public float HitsPerSecond { get; set; }

        public override void ApplyField(float dt, Body controllerBody, Body worldBody)
        {
            if (worldBody != null)
            {
                this.totalelapsedtime += dt;
                var damage = worldBody.UserData as IActor;
                if (damage != null && this.totalelapsedtime >= this.HitsPerSecond)
                {
                    damage.Health += (int)(this.FieldVector.Length() * this.FieldVector.UnitVector().X);
                    this.totalelapsedtime = 0;
                }
            }
        }

        public override XElement XmlSerialize()
        {
            return new XElement(
                "HealthField",
                new XAttribute("hitsPerSecond", this.HitsPerSecond),
                base.XmlSerialize());
        }

        public override void XmlDeserialize(XElement element)
        {
            if (element != null)
            {
                base.XmlDeserialize(element.Element("Field"));
                this.HitsPerSecond = element.GetAttribute("hitsPerSecond", 0);
            }
        }
    }
}
