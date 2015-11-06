namespace Physicist.Events
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Microsoft.Xna.Framework;
    using Physicist.MainGame.Controls;
    using Physicist.Types.Enums;
    using Physicist.Types.Interfaces;
    using Physicist.Types.Util;

    public class TriggerSet : Trigger
    {
        private Dictionary<string, ITrigger> triggers = new Dictionary<string, ITrigger>();

        public TriggerSet()
            : base()
        {
        }

        public TriggerSetOperation Operation { get; private set; }

        public new bool IsInitialized { get; protected set; }

        public override void Update(GameTime gameTime)
        {
            if (this.IsEnabled)
            {
                if (this.Operation == TriggerSetOperation.AND)
                {
                    this.IsActive = this.triggers.Values.ToList().Find(t => { return !t.IsActive; }) == null;
                }
                else if (this.Operation == TriggerSetOperation.OR)
                {
                    this.IsActive = this.triggers.Values.ToList().Find(t => { return t.IsActive; }) != null;
                }

                base.Update(gameTime);
            }
        }

        public void Initialize(IEnumerable<ITrigger> availableTriggers, IEnumerable<IModifier> availableModifiers)
        {
            if (availableModifiers != null && availableTriggers != null)
            {
                foreach (var trigger in availableTriggers) 
                {
                    if (this.triggers.ContainsKey(trigger.Name))
                    {
                        this.triggers[trigger.Name] = trigger;
                    }
                }

                base.Initialize(availableModifiers);

                this.IsInitialized = true;
            }
        }

        public override XElement XmlSerialize()
        {
            return new XElement(
                "TriggerSet",
                new XAttribute("operation", this.Operation),
                new XElement(
                    "Triggers",
                    this.triggers.Select(trigger => new XElement("Trigger", new XAttribute("name", trigger.Key)))));
        }

        public override void XmlDeserialize(XElement element)
        {
            if (element != null)
            {
                base.XmlDeserialize(element);

                this.Operation = element.GetAttribute("operation", TriggerSetOperation.AND);

                var triggersEle = element.Element("Triggers");
                if (triggersEle != null)
                {
                    foreach (var triggerEle in triggersEle.Elements("Trigger"))
                    {
                        this.triggers.Add(triggerEle.Attribute("name").Value, null);
                    }
                }
            }
        }
    }
}
