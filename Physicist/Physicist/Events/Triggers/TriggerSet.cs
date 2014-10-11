namespace Physicist.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;
    using FarseerPhysics.Dynamics;
    using Microsoft.Xna.Framework;
    using Physicist.Actors;
    using Physicist.Controls;
    using Physicist.Enums;
    using Physicist.Extensions;

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
            throw new NotImplementedException();
        }

        public override void XmlDeserialize(XElement element)
        {
            if (element != null)
            {
                base.XmlDeserialize(element);

                var operationAtt = element.Attribute("operation");
                if (operationAtt != null)
                {
                    this.Operation = (TriggerSetOperation)Enum.Parse(typeof(TriggerSetOperation), operationAtt.Value);
                }

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
