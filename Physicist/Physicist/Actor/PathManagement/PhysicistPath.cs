namespace Physicist.Actors
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Microsoft.Xna.Framework;
    using Physicist.Controls;
    using Physicist.Events;
    using Physicist.Extensions;

    public class PhysicistPath : PhysicistGameScreenItem
    {
        private List<IModifier> modifiers = new List<IModifier>();

        private Queue<PathNode> nodeQueue = new Queue<PathNode>();

        private Actor target = null;

        public PhysicistPath()
        {
        }

        public PhysicistPath(string name)
        {
            this.Name = name;
        }

        public event EventHandler PathCompleted;

        public Actor Target
        {
            get
            {
                return this.target;
            }

            set
            {
                this.target = value;
                foreach (var node in this.nodeQueue)
                {
                    node.TargetActor = this.target;
                }
            }
        }

        public bool IsEnabled { get; set; }

        public bool LoopPath { get; set; }

        public string Name { get; set; }

        public string TargetPathUponCompletion { get; set; }

        public IEnumerable<IModifier> Modifiers
        {
            get
            {
                return this.modifiers;
            }
        }

        public void AddPathNode(PathNode node)
        {
            if (node != null)
            {
                if (this.nodeQueue.Count == 0)
                {
                    node.Deactivated += this.NodeDeactivated;
                    node.IsActive = true;
                }

                this.nodeQueue.Enqueue(node);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (this.IsEnabled && this.nodeQueue.Count > 0)
            {
                this.nodeQueue.ElementAt(0).Update(gameTime);
            }
        }

        public void ClearPathNodes()
        {
            this.nodeQueue.Clear();
        }

        public void NodeDeactivated(object sender, EventArgs e)
        {
            var pathNode = sender as PathNode;
            if (pathNode != null && this.nodeQueue.Contains(pathNode))
            {
                this.NextNode();
            }
        }

        public void NextNode()
        {
            var oldNode = this.nodeQueue.Dequeue();
            if (oldNode != null)
            {
                oldNode.Deactivated -= this.NodeDeactivated;
                if (this.LoopPath)
                {
                    this.nodeQueue.Enqueue(oldNode);
                }
            }

            if (this.nodeQueue.Count > 0)
            {
                var nextNode = this.nodeQueue.ElementAt(0);
                nextNode.Deactivated += this.NodeDeactivated;
                nextNode.IsActive = true;
            }
            else
            {
                this.IsEnabled = false;
                this.PathHasCompleted();
            }
        }

        public override XElement XmlSerialize()
        {
            XElement element = new XElement(
                "PhysicistPath",
                new XAttribute("name", this.Name),
                new XAttribute("class", "PhysicistPath"),
                new XAttribute("isEnabled", this.IsEnabled),
                new XAttribute("loopPath", this.LoopPath),
                new XAttribute("targetPathUponCompletion", this.TargetPathUponCompletion),
                new XElement(
                    "Modifiers",
                    this.modifiers.Select(modifier => modifier.XmlSerialize()).ToArray()),
                this.nodeQueue.Select(node => node.XmlSerialize()).ToArray());

            return element;
        }

        public override void XmlDeserialize(XElement element)
        {
            if (element != null)
            {
                this.Name = element.Attribute("name").Value;

                this.IsEnabled = element.GetAttribute("isEnabled", true);

                this.LoopPath = element.GetAttribute("loopPath", true);

                this.TargetPathUponCompletion = element.GetAttribute("targetPathUponCompletion", string.Empty);

                var modifierEleList = element.Element("Modifiers");
                if (modifierEleList != null)
                {
                    foreach (var modifierEle in modifierEleList.Elements())
                    {
                        IModifier modifier = (IModifier)MapLoader.CreateInstance(modifierEle, "class");
                        modifier.XmlDeserialize(modifierEle);
                        this.modifiers.Add(modifier);
                    }
                }

                foreach (var pathNodeEle in element.Elements().Where(elem => elem.Name != "Modifiers"))
                {
                    PathNode node = (PathNode)MapLoader.CreateInstance(pathNodeEle, null);
                    node.Screen = this.Screen;
                    node.XmlDeserialize(pathNodeEle);
                    if (node != null)
                    {
                        node.TargetActor = this.target;
                        node.Initialize(this.modifiers);
                        this.AddPathNode(node);
                    }
                }
            }
        }

        private void PathHasCompleted()
        {
            if (this.PathCompleted != null)
            {
                this.PathCompleted(this, null);
            }
        }
    }
}