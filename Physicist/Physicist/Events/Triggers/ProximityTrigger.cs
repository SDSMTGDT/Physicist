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
        private Body sensorBody = null;
        private Fixture collisionFixture = null;
        private Fixture separationFixture = null;

        public ProximityTrigger(XElement element)
        {
            this.XmlDeserialize(element);
        }

        public ProximityTrigger(float sensorRadius, Vector2 position)
        {
            this.sensorBody = BodyFactory.CreateCircle(MainGame.World, sensorRadius, 1f, position);
            this.CreateSensors(this.sensorBody.FixtureList[0]);
        }

        public ProximityTrigger(Body sensorBody, Fixture sensorTemplate)
        {
            this.sensorBody = sensorBody;
            this.CreateSensors(sensorTemplate);
            this.IsEnabled = true;
            this.IsContinuous = false;
        }

        public bool IsContinuous 
        {
            get
            {
                return !(this.collisionFixture.IgnoreCCDWith == Category.All);
            }

            set
            {
                if (value)
                {
                    this.collisionFixture.IgnoreCCDWith = Category.All;
                }
                else
                {
                    this.collisionFixture.IgnoreCCDWith = Category.None;
                }
            }
        }

        public override XElement XmlSerialize()
        {
            throw new NotImplementedException();
        }

        public new void XmlDeserialize(XElement element)
        {
            Fixture sensorTemplate = null;

            if (element != null)
            {
                base.XmlDeserialize(element.Element("Trigger"));

                var attachedEle = element.Attribute("attachedTarget");
                if (attachedEle != null)
                {
                    Actor target = MainGame.Map.NamedObjects[attachedEle.Value] as Actor;
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
                                            MainGame.World,
                                            float.Parse(element.Attribute("radius").Value, CultureInfo.CurrentCulture),
                                            1f,
                                            new Vector2(position.X, MainGame.Map.Height - position.Y));

                    tempBody.IsSensor = true;
                    
                    if (this.sensorBody == null)
                    {
                        this.sensorBody = tempBody;
                        this.sensorBody.BodyType = BodyType.Static;
                    }

                    sensorTemplate = tempBody.FixtureList[0];
                }
                else
                {
                    var bodyData = XmlBodyFactory.DeserializeBody(bodyInfoEle.Elements().ElementAt(0));
                    MainGame.World.RemoveBody(bodyData.Item1);
                    sensorTemplate = bodyData.Item1.FixtureList[0];                   
                }

                this.CreateSensors(sensorTemplate);
                this.IsContinuous = false;

                var continuousAtt = element.Attribute("IsContinuous");
                if (continuousAtt != null)
                {
                    this.IsContinuous = bool.Parse(continuousAtt.Value);
                }
            }
        }

        protected virtual bool OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            this.IsActive = true;
            return this.IsEnabled;
        }

        protected virtual void OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            this.IsActive = false;
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
            this.collisionFixture.IsSensor = true;

            this.separationFixture.OnSeparation += this.OnSeparation;
            this.separationFixture.IsSensor = true;
            this.separationFixture.IgnoreCCDWith = Category.All;

            foreach (var fixture in this.sensorBody.FixtureList)
            {
                this.collisionFixture.IgnoreCollisionWith(fixture);
                this.separationFixture.IgnoreCollisionWith(fixture);
            }
        }
    }
}
