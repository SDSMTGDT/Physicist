namespace Physicist.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Microsoft.Xna.Framework;
    using Physicist.Controls;

    public class PhysicistEvent : IXmlSerializable, IPhysicistEvent
    {
        private List<IModifier> modifiers = new List<IModifier>();
        private List<ITrigger> triggers = new List<ITrigger>();

        public PhysicistEvent(XElement element)
        {
            this.XmlDeserialize(element);
        }

        public bool IsEnabled
        {
            get;
            set;
        }

        public string Name
        {
            get;
            private set;
        }

        public IEnumerable<IModifier> Modifiers
        {
            get
            {
                return this.modifiers;
            }
        }

        public IEnumerable<ITrigger> Triggers
        {
            get
            {
                return this.triggers;
            }
        }

        public void AddModifier(IModifier modifier)
        {
            if (modifier != null)
            {
                this.modifiers.Add(modifier);
            }
        }

        public void AddTrigger(ITrigger trigger)
        {
            if (trigger != null)
            {
                this.triggers.Add(trigger);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (this.IsEnabled)
            {
                this.triggers.ForEach(trigger => trigger.Update(gameTime));
                this.modifiers.ForEach(modifier => modifier.Update(gameTime));
            }
        }

        public XElement XmlSerialize()
        {
            throw new NotImplementedException();
        }

        public void XmlDeserialize(XElement element)
        {            
            if (element != null)
            {
                this.IsEnabled = true;
                this.Name = element.Attribute("name").Value;

                XAttribute enabledAtt = element.Attribute("isEnabled");
                if (enabledAtt != null)
                {
                    this.IsEnabled = bool.Parse(enabledAtt.Value);
                }

                foreach (var modifierEle in element.Element("Modifiers").Elements())
                {
                    IModifier modifier = (IModifier)MapLoader.CreateInstance(modifierEle, null);
                    if (modifier != null)
                    {
                        this.modifiers.Add(modifier);
                    }
                }

                foreach (var triggerEle in element.Element("Triggers").Elements())
                {
                    Trigger trigger = (Trigger)MapLoader.CreateInstance(triggerEle, null);
                    if (trigger != null)
                    {
                        this.triggers.Add(trigger);
                        trigger.Initialize(this.modifiers);
                    }
                }
            }
        }
    }
}
