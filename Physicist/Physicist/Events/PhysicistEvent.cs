namespace Physicist.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Microsoft.Xna.Framework;
    using Physicist.Controls;

    public class PhysicistEvent : PhysicistGameScreenItem, IPhysicistEvent
    {
        private List<IModifier> modifiers = new List<IModifier>();
        private List<ITrigger> triggers = new List<ITrigger>();
        private List<ITrigger> triggerSets = new List<ITrigger>();

        public PhysicistEvent()
        {
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

        public IEnumerable<ITrigger> TriggerSets
        {
            get
            {
                return this.triggerSets;
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

        public void AddTriggerSet(ITrigger triggerSet) 
        {
            if (triggerSet != null) 
            {
                this.triggerSets.Add(triggerSet);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (this.IsEnabled)
            {
                this.triggers.ForEach(trigger => trigger.Update(gameTime));
                this.triggerSets.ForEach(triggerset => triggerset.Update(gameTime));
            }
        }

        public override XElement XmlSerialize()
        {
            throw new NotImplementedException();
        }

        public override void XmlDeserialize(XElement element)
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
                        modifier.Screen = this.Screen;
                        modifier.XmlDeserialize(modifierEle);
                        this.modifiers.Add(modifier);
                    }
                }

                foreach (var triggerEle in element.Element("Triggers").Elements())
                {
                    Trigger trigger = (Trigger)MapLoader.CreateInstance(triggerEle, null);
                    if (trigger != null)
                    {
                        trigger.Screen = this.Screen;
                        trigger.XmlDeserialize(triggerEle);
                        trigger.Initialize(this.modifiers);
                        this.triggers.Add(trigger);
                    }
                }

                var triggerSetsEle = element.Element("TriggerSets");
                if (triggerSetsEle != null)
                {
                    foreach (var triggerSetEle in triggerSetsEle.Elements()) 
                    {
                        TriggerSet triggerSet = (TriggerSet)MapLoader.CreateInstance(triggerSetEle, null);
                        if (triggerSet != null)
                        {
                            triggerSet.Screen = this.Screen;
                            triggerSet.XmlDeserialize(triggerSetEle);
                            triggerSet.Initialize(this.triggers, this.modifiers);
                            this.triggerSets.Add(triggerSet);
                        }
                    }
                }
            }
        }
    }
}
