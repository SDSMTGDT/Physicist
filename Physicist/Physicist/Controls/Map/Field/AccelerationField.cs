namespace Physicist.Controls.Fields
{
    using System;
    using System.Xml.Linq;
    using FarseerPhysics.Dynamics;
    using Microsoft.Xna.Framework;
    using Physicist.Extensions;

    public class AccelerationField : Field
    {
        public AccelerationField()
        {
        }

        public AccelerationField(Vector2 vector, Body fieldBody)
            : base(vector, fieldBody)
        {
        }

        public override void ApplyField(float dt, Body controllerBody, Body worldBody)
        {
            if (controllerBody != null && worldBody != null)
            {
                Vector2 f = Vector2.Zero;
                Vector2 d = this.Singularity ? (controllerBody.Position - worldBody.Position) : this.FieldVector.UnitVector();

                // Force applied at given magnitude in direction d, removing the world gravity
                f = ((d * this.FieldVector.Length()) - this.World.Gravity * worldBody.GravityScale) * worldBody.Mass;

                worldBody.ApplyForce(ref f);
            }
        }

        public override XElement XmlSerialize()
        {
            return new XElement(
                "AccelerationField",
                base.XmlSerialize());
        }

        public override void XmlDeserialize(XElement element)
        {
            if (element != null)
            {
                base.XmlDeserialize(element.Element("Field"));
            }
        }
    }
}
