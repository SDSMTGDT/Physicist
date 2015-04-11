namespace Physicist.Events
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Linq;
    using FarseerPhysics.Dynamics;
    using FarseerPhysics.Dynamics.Contacts;
    using FarseerPhysics.Factories;
    using Microsoft.Xna.Framework;
    using Physicist.Actors;
    using Physicist.Controls;
    using Physicist.Enums;
    using Physicist.Extensions;
    
    public class ProximityTrigger : Trigger, IXmlSerializable
    {
        private BodyInfo bodyInfo = null;
        private Body sensorBody = null;
        private Fixture collisionFixture = null;
        private Fixture separationFixture = null;

        public ProximityTrigger()
        {
        }

        public ProximityTrigger(float sensorRadius, Vector2 position, World world)
        {
            if (this.World != world)
            {
                this.World = world;
            }

            this.SensorBody = BodyFactory.CreateCircle(this.World, sensorRadius, 1f, position);
            this.IsEnabled = true;
            this.IsContinuous = true;
        }

        public ProximityTrigger(float sensorWidth, float sensorHeight, Vector2 position, World world)
        {
            if (this.World != world)
            {
                this.World = world;
            }

            this.SensorBody = BodyFactory.CreateRectangle(this.World, sensorWidth, sensorHeight, 1f, position);
            this.CreateSensors(this.SensorBody.FixtureList[0]);
            this.IsEnabled = true;
            this.IsContinuous = true;
            this.IsSensor = true;
        }

        public ProximityTrigger(Body sensorBody, Fixture sensorTemplate, World world)
        {
            if (this.World != world)
            {
                this.World = world;
            }

            this.sensorBody = sensorBody;
            this.CreateSensors(sensorTemplate);
            this.IsEnabled = true;
            this.IsContinuous = true;
            this.IsSensor = true;
        }

        public Body SensorBody
        {
            get
            {
                return this.sensorBody;
            }

            set
            {
                this.sensorBody = value;
                if (value != null && value.FixtureList[0] != null)
                {
                    this.CreateSensors(value.FixtureList[0]);
                }
            }
        }

        public bool IsContinuous 
        {
            get
            {
                return !(this.collisionFixture.IgnoreCCDWith == PhysicistCategory.All);
            }

            set
            {
                if (!value)
                {
                    this.collisionFixture.IgnoreCCDWith = PhysicistCategory.All;
                }
                else
                {
                    this.collisionFixture.IgnoreCCDWith = PhysicistCategory.None;
                }
            }
        }

        public bool IsSensor
        {
            get
            {
                return this.collisionFixture.IsSensor;
            }

            set
            {
                this.collisionFixture.IsSensor = value;
                this.separationFixture.IsSensor = value;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void XmlDeserialize(XElement element)
        {
            Fixture sensorTemplate = null;

            if (element != null)
            {
                base.XmlDeserialize(element.Element("Trigger"));

                var attachedEle = element.Attribute("attachedTarget");
                if (attachedEle != null)
                {
                    Actor target = this.Map.NamedObjects[attachedEle.Value] as Actor;
                    if (target != null)
                    {
                        this.sensorBody = target.Body;
                    }
                }

                var bodyInfoEle = element.Element("FixtureTemplate");
                if (bodyInfoEle == null)
                {
                    Vector2 position = ExtensionMethods.XmlDeserializeVector2(element.Element("Position"));
                    var tempBody = BodyFactory.CreateCircle(
                                            this.World,
                                            float.Parse(element.Attribute("radius").Value, CultureInfo.CurrentCulture),
                                            1f,
                                            new Vector2(position.X, this.Map.Height - position.Y).ToSimUnits());

                    // tempBody.IsSensor = true;
                    tempBody.FixtureList.ForEach(fx => fx.UserData = "Sensor");
                    
                    if (this.sensorBody == null)
                    {
                        this.sensorBody = tempBody;
                        this.sensorBody.BodyType = BodyType.Static;
                    }

                    sensorTemplate = tempBody.FixtureList[0];
                }
                else
                {
                    var bodyData = XmlBodyFactory.DeserializeBody(this.World, this.Map.Height, bodyInfoEle.Elements().ElementAt(0));
                    this.World.RemoveBody(bodyData.Item1);
                    this.bodyInfo = bodyData.Item2;
                    sensorTemplate = bodyData.Item1.FixtureList[0];                   
                }

                this.CreateSensors(sensorTemplate);
                this.IsContinuous = element.GetAttribute("isContinuous", true);
                this.IsSensor = element.GetAttribute("isSensor", false);
            }
        }

        public override XElement XmlSerialize()
        {
            XElement element = new XElement(
                "ProximityTrigger",
                new XAttribute("isContinuous", this.IsContinuous),
                new XAttribute("attachedTarget", ((Actor)this.sensorBody.UserData).Name),
                base.XmlSerialize());

            if (this.bodyInfo != null)
            {
                element.Add(new XElement("FixtureTemplate", this.bodyInfo.XmlSerialize()));
            }
            else
            {
                element.Add(
                    new XAttribute("radius", ((FarseerPhysics.Collision.Shapes.CircleShape)this.sensorBody.FixtureList[0].Shape).Radius),
                    new Vector2(this.sensorBody.Position.X, this.Map.Height - this.sensorBody.Position.Y).XmlSerialize("Position"));
            }

            return element;
        }

        public void RestoreCollisionWith(Fixture fixture)
        {
            this.collisionFixture.RestoreCollisionWith(fixture);
            this.separationFixture.RestoreCollisionWith(fixture);
        }

        protected virtual bool OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            var success = this.IsEnabled;
            if (fixtureB != null && !fixtureB.IsSensor)
            {
                this.ActivationData = new ActivationData(contact, ActivationType.Collision.ToString());
                this.ActivateWithStyle();
                success = false;
            }

            return success;
        }

        protected virtual void OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            if (fixtureB != null && !fixtureB.IsSensor)
            {
                this.ActivationData = null;
                this.DeactivateWithStyle();
            }
        }

        // Slight Hack, if both OnSeparation and OnCollision event are subscribed to
        // the OnSeparation call is either ignored or lost in the works of the 
        // Farseer Physics engine. A simple solution is to use 2 fixtures, both sensors, that
        // ignore eachother in collision and subscribe to OnSeparation/OnCollision individually
        private void CreateSensors(Fixture sensorTemplate)
        {
            this.collisionFixture = sensorTemplate;
            if (!this.sensorBody.FixtureList.Contains(sensorTemplate))
            {
                this.collisionFixture = sensorTemplate.CloneOnto(this.sensorBody);
            }

            this.separationFixture = this.collisionFixture.CloneOnto(this.sensorBody);

            this.collisionFixture.OnCollision += this.OnCollision;

            // this.collisionFixture.IsSensor = true;
            this.collisionFixture.CollidesWith = PhysicistCategory.AllIgnoreFields;

            this.separationFixture.OnSeparation += this.OnSeparation;

            // this.separationFixture.IsSensor = true;
            this.separationFixture.IgnoreCCDWith = PhysicistCategory.All;
            this.separationFixture.CollidesWith = PhysicistCategory.AllIgnoreFields;

            foreach (var fixture in this.sensorBody.FixtureList)
            {
                this.collisionFixture.IgnoreCollisionWith(fixture);
                this.separationFixture.IgnoreCollisionWith(fixture);
            }
        }
    }
}
