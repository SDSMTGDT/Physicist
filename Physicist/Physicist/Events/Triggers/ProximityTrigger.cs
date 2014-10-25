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
            this.CreateSensors(this.SensorBody.FixtureList[0]);
            this.IsEnabled = true;
            this.IsContinuous = false;
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
            this.IsContinuous = false;
        }

        public ProximityTrigger(Body sensorBody, Fixture sensorTemplate, World world)
        {
            if (this.World != world)
            {
                this.World = world;
            }

            this.SensorBody = sensorBody;
            this.CreateSensors(sensorTemplate);
            this.IsEnabled = true;
            this.IsContinuous = false;
        }

        public Body SensorBody { get; set; }

        public bool IsContinuous 
        {
            get
            {
                return !(this.collisionFixture.IgnoreCCDWith == Category.All);
            }

            set
            {
                if (!value)
                {
                    this.collisionFixture.IgnoreCCDWith = Category.All;
                }
                else
                {
                    this.collisionFixture.IgnoreCCDWith = Category.None;
                }
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
                        this.SensorBody = target.Body;
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

                    tempBody.IsSensor = true;
                    
                    if (this.SensorBody == null)
                    {
                        this.SensorBody = tempBody;
                        this.SensorBody.BodyType = BodyType.Static;
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
                this.IsContinuous = element.GetAttribute("isContinuous", false);
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

        protected virtual bool OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            this.ActivateWithStyle();
            return this.IsEnabled;
        }

        protected virtual void OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            this.DeactivateWithStyle();
        }

        // Slight Hack, if both OnSeparation and OnCollision event are subscribed to
        // the OnSeparation call is either ignored or lost in the works of the 
        // Farseer Physics engine. A simple solution is to use 2 fixtures, both sensors, that
        // ignore eachother in collision and subscribe to OnSeparation/OnCollision individually
        private void CreateSensors(Fixture sensorTemplate)
        {
            this.collisionFixture = sensorTemplate;
            if (!this.SensorBody.FixtureList.Contains(sensorTemplate))
            {
                this.collisionFixture = sensorTemplate.CloneOnto(this.SensorBody);
            }

            this.separationFixture = this.collisionFixture.CloneOnto(this.SensorBody);

            this.collisionFixture.OnCollision += this.OnCollision;
            this.collisionFixture.IsSensor = true;

            this.separationFixture.OnSeparation += this.OnSeparation;
            this.separationFixture.IsSensor = true;
            this.separationFixture.IgnoreCCDWith = Category.All;

            foreach (var fixture in this.SensorBody.FixtureList)
            {
                this.collisionFixture.IgnoreCollisionWith(fixture);
                this.separationFixture.IgnoreCollisionWith(fixture);
            }
        }
    }
}
