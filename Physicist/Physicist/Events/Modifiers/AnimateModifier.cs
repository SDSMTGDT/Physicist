namespace Physicist.Events
{
    using System.Collections.Generic;
    using System.Xml.Linq;
    using Microsoft.Xna.Framework;
    using Physicist.Actors;
    using Physicist.Controls;
    using Physicist.Extensions;

    public class AnimateModifier : Modifier<Actor>
    {
        private string targetAnimationName;
        private string targetSprite;
        private Dictionary<Actor, string> previousAnimations = new Dictionary<Actor, string>();

        public AnimateModifier()
        {
        }

        public AnimateModifier(Actor target, string targetSprite, string targetAnimation, bool isOneShot, bool hasMemory)
        {
            this.AddTarget(target);
            this.targetAnimationName = targetAnimation;
            this.targetSprite = targetSprite;
            this.IsOneShot = isOneShot;
            this.HasMemory = hasMemory;
        }

        public bool HasMemory { get; set; }

        public bool IsOneShot { get; private set; }

        public override XElement XmlSerialize()
        {
            return new XElement(
                "AnimateModifier",
                new XAttribute("targetSprite", this.targetSprite),
                new XAttribute("targetAnimation", this.targetAnimationName),
                new XAttribute("isOneShot", this.IsOneShot),
                new XAttribute("hasMemory", this.HasMemory),
                base.XmlSerialize());
        }

        public override void XmlDeserialize(XElement element)
        {
            if (element != null)
            {
                base.XmlDeserialize(element.Element("Modifier"));

                this.targetSprite = element.GetAttribute<string>("targetSprite", string.Empty);
                this.targetAnimationName = element.GetAttribute<string>("targetAnimation", string.Empty);

                this.IsOneShot = element.GetAttribute<bool>("isOneShot", false);

                this.HasMemory = element.GetAttribute<bool>("hasMemory", true);
            }
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            foreach (var target in this.Targets)
            {
                if (!this.previousAnimations.ContainsKey(target))
                {
                    this.previousAnimations.Add(target, target.Sprites[this.targetSprite].CurrentAnimationString);
                }
            }

            foreach (var target in this.Targets)
            {
                if (target.Sprites.ContainsKey(this.targetSprite))
                {
                    target.Sprites[this.targetSprite].CurrentAnimationString = this.targetAnimationName;
                }
            }

            if (this.IsOneShot)
            {
                this.IsEnabled = false;
            }
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();

            if (this.HasMemory)
            {
                foreach (var target in this.Targets)
                {
                    if (this.previousAnimations.ContainsKey(target))
                    {
                        target.Sprites[this.targetSprite].CurrentAnimationString = this.previousAnimations[target];
                    }
                }
            }

            if (!this.HasMemory)
            {
                this.previousAnimations.Clear();
            }
        }
    
        protected override void SetTargets(IEnumerable<object> targetObjects)
        {
            if (targetObjects != null)
            {
                foreach (Actor actor in targetObjects)
                {
                    if (actor != null)
                    {
                        this.AddTarget(actor);
                    }
                }
            }
        }
    }
}
