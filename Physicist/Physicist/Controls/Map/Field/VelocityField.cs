namespace Physicist.MainGame.Controls.Fields
{
    using System;
    using System.Xml.Linq;
    using FarseerPhysics.Dynamics;
    using Microsoft.Xna.Framework;
    using Physicist.Types.Util;

    public class VelocityField : Field
    {
        public VelocityField()
        {
        }

        public VelocityField(Vector2 vector, Body fieldBody)
            : base(vector, fieldBody)
        {
        }

        public override void ApplyField(float dt, Body controllerBody, Body worldBody)
        {
            if (controllerBody != null && worldBody != null)
            {
                Vector2 d = this.Singularity ? (controllerBody.Position - worldBody.Position) : this.FieldVector.UnitVector();

                worldBody.LinearVelocity += d * this.FieldVector.Length();
            }
        }

        public override XElement XmlSerialize()
        {
            return new XElement(
                "VelocityField",
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
