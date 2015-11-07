namespace Physicist.MainGame.Controls.Fields
{
    using System.Collections.Generic;
    using System.Xml.Linq;
    using FarseerPhysics.Controllers;
    using FarseerPhysics.Dynamics;
    using Microsoft.Xna.Framework;
    using Physicist.MainGame.Controls;
    using Physicist.MainGame.Extensions;
    using Physicist.Types.Util;

    public abstract class Field : Controller, IField
    {
        private Body fieldBody = null;

        protected Field()
            : base(ControllerType.AbstractForceController)
        {
        }

        protected Field(Vector2 fieldVector, Body fieldBody)
            : base(ControllerType.AbstractForceController)
        {
            this.FieldVector = fieldVector;
            this.Body = fieldBody;
        }

        public Vector2 FieldVector { get; set; }

        public Body Body 
        { 
            get
            {
                return this.fieldBody;
            }

            set
            {
                this.fieldBody = value;
                if (this.fieldBody != null)
                {
                    this.fieldBody.CollisionCategories = PhysicistCategory.Field;
                }
            }
        }
        
        public bool Singularity { get; set; }

        public override void Update(float dt)
        {
            if (this.Body != null)
            {
                // Loop through all bodies
                foreach (Body worldBody in World.BodyList)
                {
                    if (!this.IsActiveOn(worldBody))
                    {
                        continue;
                    }

                    // Get all fixtures that intesect point
                    List<Fixture> fixtures = World.TestPointAll(worldBody.Position);

                    foreach (var fixture in fixtures)
                    {
                        Body controllerBody = fixture.Body;

                        if (worldBody == controllerBody || (worldBody.IsStatic && controllerBody.IsStatic) || !controllerBody.Enabled)
                        {
                            continue;
                        }

                        if (this.Body != controllerBody)
                        {
                            continue;
                        }

                        this.ApplyField(dt, controllerBody, worldBody);
                    }
                }
            }
        }

        public abstract void ApplyField(float dt, Body controllerBody, Body worldBody);

        public virtual XElement XmlSerialize()
        {
            return new XElement(
                    "Field", 
                    this.FieldVector.XmlSerialize("FieldVector"),
                    new XAttribute("singularity", this.Singularity));
        }

        public virtual void XmlDeserialize(XElement element)
        {
            if (element != null)
            {
                this.FieldVector = XmlDeserializeHelper.XmlDeserialize<Vector2>(element.Element("FieldVector"));
                this.Singularity = element.GetAttribute("singularity", false);
            }
        }
    }
}