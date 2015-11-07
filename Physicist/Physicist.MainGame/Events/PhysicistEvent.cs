namespace Physicist.Events
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Microsoft.Xna.Framework;
    using Physicist.MainGame.Controls;
    using Physicist.Types.Interfaces;
    using Physicist.Types.Util;

    public class PhysicistEvent : PhysicistGameScreenItem, IUpdate, IName
    {
        private List<IModifier> modifiers = new List<IModifier>();
        private List<ITrigger> triggers = new List<ITrigger>();
        private List<ITrigger> triggerSets = new List<ITrigger>();

        public PhysicistEvent()
        {
        }

        public bool IsEnabled { get; set; }

        public string Name { get; private set; }

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
            return new XElement(
                "Event",
                new XAttribute("class", "PhysicistEvent"),
                new XAttribute("name", this.Name),
                new XElement("Triggers", this.triggers.Select(trigger => trigger.XmlSerialize()).ToArray()),
                new XElement("Modifiers", this.modifiers.Select(modifier => modifier.XmlSerialize()).ToArray()),
                new XElement("TriggerSets", this.triggerSets.Select(triggerset => triggerset.XmlSerialize()).ToArray()));
        }

        public override void XmlDeserialize(XElement element)
        {            
            if (element != null)
            {
                this.IsEnabled = true;
                this.Name = element.GetAttribute("name", string.Empty);
                this.IsEnabled = element.GetAttribute("isEnabled", true);

                var modifiersEle = element.Element("Modifiers");
                if (modifiersEle != null)
                {
                    foreach (var modifierEle in modifiersEle.Elements())
                    {
                        IModifier modifier = (IModifier)MapLoader.CreateInstance(modifierEle, null);
                        if (modifier != null)
                        {
                            modifier.Screen = this.Screen;
                            modifier.XmlDeserialize(modifierEle);
                            this.modifiers.Add(modifier);
                        }
                    }
                }

                var triggersEle = element.Element("Triggers");
                if (triggersEle != null)
                {
                    foreach (var triggerEle in triggersEle.Elements())
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
