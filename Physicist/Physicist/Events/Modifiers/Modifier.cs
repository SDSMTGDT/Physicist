namespace Physicist.Events
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Linq;
    using Microsoft.Xna.Framework;
    using Physicist.Actors;
    using Physicist.Controls;

    public abstract class Modifier<T> : PhysicistGameScreenItem, IModifier, IXmlSerializable
    {
        private bool isActive;
        private bool isActiveHasChanged = false;
        private List<T> targets = new List<T>();

        protected Modifier()
            : this(false)
        {
        }

        protected Modifier(bool isSingleUse)
        {
            this.isActive = false;
            this.IsEnabled = true;
            this.IsSingleUse = isSingleUse;
        }

        public string Name { get; set; }

        public bool IsSingleUse
        {
            get;
            private set;
        }

        public bool IsActive 
        {
            get
            {
                return this.isActive;
            }

            set
            {
                if (this.isActive != value && !(this.IsSingleUse && this.isActiveHasChanged))
                {
                    this.isActive = value;
                    this.isActiveHasChanged = true;
                    if (this.isActive)
                    {
                        this.OnActivated();
                    }
                    else
                    {
                        this.OnDeactivated();
                    }
                }
            }
        }

        public bool IsEnabled { get; set; }

        public IEnumerable<T> Targets
        {
            get
            {
                return this.targets;
            }
        }

        public override XElement XmlSerialize()
        {
            XElement modifierElement = new XElement(
                                            "Modifier",
                                            new XAttribute("isEnabled", this.IsEnabled),
                                            new XAttribute("isActive", this.IsActive));

            this.targets.ForEach(target => modifierElement.Add(new XElement("Target", ((IName)target).Name)));

            return modifierElement;
        }

        public override void XmlDeserialize(XElement element)
        {
            if (element != null)
            {
                var nameAtt = element.Attribute("name");
                if (nameAtt != null)
                {
                    this.Name = nameAtt.Value;
                }

                var singleUseAtt = element.Attribute("isSingleUse");
                if (singleUseAtt != null)
                {
                    this.IsSingleUse = bool.Parse(singleUseAtt.Value);
                }

                var enabledAtt = element.Attribute("isEnabled");
                if (enabledAtt != null)
                {
                    this.IsEnabled = bool.Parse(enabledAtt.Value);
                }

                var activeAtt = element.Attribute("isActive");
                if (activeAtt != null)
                {
                    this.IsActive = bool.Parse(activeAtt.Value);
                }

                List<IName> targetObjects = new List<IName>();
                foreach (var targetEle in element.Elements("Target"))
                {
                    targetObjects.Add(this.Map.NamedObjects[targetEle.Attribute("name").Value]);
                }

                this.SetTargets(targetObjects);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        protected virtual void OnActivated()
        {
        }

        protected virtual void OnDeactivated()
        {
        }

        protected void AddTarget(T target)
        {
            this.targets.Add(target);
        }

        protected void RemoveTarget(T target)
        {
            this.targets.Remove(target);
        }

        protected abstract void SetTargets(IEnumerable<object> targetObjects);
    }
}
