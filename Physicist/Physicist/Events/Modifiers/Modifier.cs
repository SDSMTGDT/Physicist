namespace Physicist.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Microsoft.Xna.Framework;
    using Physicist.Actors;
    using Physicist.Controls;
    using Physicist.Extensions;

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

        public bool IsSingleUse { get; private set; }

        public bool IsEnabled { get; set; }

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

        public IEnumerable<T> Targets
        {
            get
            {
                return this.targets;
            }
        }

        public override XElement XmlSerialize()
        {
            return new XElement(
                    "Modifier",
                    new XAttribute("isEnabled", this.IsEnabled),
                    new XAttribute("isActive", this.IsActive),
                    new XAttribute("name", this.Name),
                    new XAttribute("isSingleUse", this.IsSingleUse),
                    this.targets.Select(target => new XElement("Target", ((IName)target).Name)));
        }

        public override void XmlDeserialize(XElement element)
        {
            if (element != null)
            {
                this.Name = element.GetAttribute<string>("name", string.Empty);
                this.IsSingleUse = element.GetAttribute<bool>("isSingleUse", false);
                this.IsEnabled = element.GetAttribute<bool>("isEnabled", true);
                this.isActive = element.GetAttribute<bool>("isActive", true);

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
