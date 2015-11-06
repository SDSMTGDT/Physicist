namespace Physicist.Events
{
    using System.Collections.Generic;
    using System.Xml.Linq;
    using FarseerPhysics.Dynamics;
    using Microsoft.Xna.Framework;
    using Physicist.MainGame.Actors;
    using Physicist.Types.Util;

    public class VelocityModifier : Modifier<Body>
    {
        private Vector2 stepChange = new Vector2();

        public VelocityModifier()
        {
        }

        public VelocityModifier(Body target, Vector2 change)
        {
            this.AddTarget(target);
            this.stepChange = change;
        }

        public override void Update(GameTime gameTime)
        {
            if (gameTime != null && this.IsEnabled && this.IsActive)
            {
                foreach (var body in this.Targets)
                {
                    if (body != null)
                    {
                        body.LinearVelocity += this.stepChange;
                    }
                }
            }
        }

        public override void XmlDeserialize(XElement element)
        {
            if (element != null)
            {                
                base.XmlDeserialize(element.Element("Modifier"));

                this.stepChange = XmlDeserializeHelper.XmlDeserialize<Vector2>(element.Element("StepChange"));
            }
        }

        public override XElement XmlSerialize()
        {
            return new XElement(
                "VelocityModifier",
                this.stepChange.XmlSerialize("StepChange"),
                base.XmlSerialize());
        }

        protected override void SetTargets(IEnumerable<object> targetObjects)
        {
            if (targetObjects != null)
            {
                foreach (Actor actor in targetObjects)
                {
                    if (actor != null)
                    {
                        this.AddTarget(actor.Body);
                    }
                }
            }
        }
    }
}
