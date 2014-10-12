namespace Physicist.Actors
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;
    using Microsoft.Xna.Framework;
    using Physicist.Controls;
    using Physicist.Enums;
    using Physicist.Events;

    public class PathNode : PhysicistGameScreenItem
    {
        private bool isActive;

        private Dictionary<TriggerMode, Dictionary<string, IModifier>> modifiers = new Dictionary<TriggerMode, Dictionary<string, IModifier>>();

        public PathNode()
        {
            this.IsInitialized = false;
            this.modifiers.Add(TriggerMode.OnActivated, new Dictionary<string, IModifier>());
            this.modifiers.Add(TriggerMode.WhileActivated, new Dictionary<string, IModifier>());
            this.modifiers.Add(TriggerMode.OnDeactivated, new Dictionary<string, IModifier>());
        }

        public PathNode(Actor target)
        {
            this.TargetActor = target;
        }

        public event EventHandler<EventArgs> Deactivated;

        public bool IsInitialized { get; private set; }

        public bool IsActive
        {
            get
            {
                return this.isActive;
            }

            set
            {
                if (this.isActive != value)
                {
                    this.isActive = value;
                    if (this.isActive)
                    {
                        foreach (var mode in this.modifiers.Keys)
                        {
                            foreach (var modifier in this.modifiers[mode].Values)
                            {
                                modifier.IsActive = !modifier.IsActive;
                            }
                        }
                    }

                    if (!this.isActive && this.Deactivated != null)
                    {
                        this.Deactivated(this, null);
                    }
                }
            }
        }

        public Actor TargetActor { get; set; }

        public virtual void Update(GameTime gameTime)
        {
            if (this.isActive)
            {
                foreach (var modifier in this.modifiers[TriggerMode.WhileActivated].Values)
                {
                    if (modifier != null)
                    {
                        modifier.Update(gameTime);
                    }
                }
            }
        }

        public void Initialize(IEnumerable<IModifier> availableModifiers)
        {
            if (availableModifiers != null)
            {
                foreach (var modifier in availableModifiers)
                {
                    foreach (var mode in this.modifiers.Keys)
                    {
                        if (this.modifiers[mode].ContainsKey(modifier.Name))
                        {
                            this.modifiers[mode][modifier.Name] = modifier;
                            modifier.IsActive = mode == TriggerMode.OnDeactivated;
                        }
                    }
                }

                this.IsInitialized = true;
            }
        }

        public void AddModifier(TriggerMode mode, IModifier modifier)
        {
            if (modifier != null)
            {
                this.modifiers[mode].Add(modifier.Name, modifier);
            }
        }

        public bool RemoveModifier(IModifier modifier)
        {
            bool removed = false;
            if (modifier != null)
            {
                foreach (var mode in this.modifiers.Keys)
                {
                    if (this.modifiers[mode].ContainsValue(modifier))
                    {
                        this.modifiers[mode].Remove(modifier.Name);
                        removed = true;
                    }
                }
            }

            return removed;
        }

        public override XElement XmlSerialize()
        {
            XElement element = new XElement("PathNode");
            foreach (var mode in this.modifiers.Keys)
            {
                XElement modeElement = new XElement(mode.ToString());
                foreach (var modifierName in this.modifiers[mode].Keys)
                {
                    modeElement.Add(new XElement("Modifier", new XAttribute("name", modifierName)));
                }

                element.Add(modeElement);
            }

            return element;
        }

        public override void XmlDeserialize(XElement element)
        {
            if (element != null)
            {
                foreach (var mode in this.modifiers.Keys)
                {
                    var modeEle = element.Element(mode.ToString());
                    if (modeEle != null)
                    {
                        foreach (var modifierEle in modeEle.Elements("Modifier"))
                        {
                            this.modifiers[mode].Add(modifierEle.Attribute("name").Value, null);
                        }
                    }
                }
            }
        }
    }
}