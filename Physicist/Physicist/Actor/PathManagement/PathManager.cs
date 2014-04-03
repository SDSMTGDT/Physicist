namespace Physicist.Actors
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Physicist.Controls;

    public class PathManager : IXmlSerializable
    {
        private Dictionary<string, PhysicistPath> paths = new Dictionary<string, PhysicistPath>();
        private string currentPath;
        private Actor target;

        public PathManager(Actor target)
        {
            this.target = target;
            this.IsEnabled = true;
        }

        public bool IsEnabled { get; set; }

        public string CurrentPath 
        {
            get
            {
                return this.currentPath;
            }

            set
            {
                if (this.currentPath != value && this.paths.ContainsKey(value))
                {
                    if (this.currentPath != null)
                    {
                        this.paths[this.currentPath].PathCompleted -= this.OnPathCompleted;
                    }

                    this.currentPath = value;

                    this.paths[this.currentPath].PathCompleted += this.OnPathCompleted;
                }
            }
        }

        public void AddPath(PhysicistPath path)
        {
            this.paths.Add(path.Name, path);

            if (this.paths.Count == 1)
            {
                this.CurrentPath = path.Name;
            }
        }

        public void RemovePath(string pathName)
        {
            if (this.paths.ContainsKey(pathName))
            {
                this.OnPathCompleted(this.paths[pathName], null);
                this.paths.Remove(pathName);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (this.IsEnabled)
            {
                this.paths[this.CurrentPath].Update(gameTime);
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
                foreach (var pathEle in element.Elements())
                {
                    PhysicistPath path = (PhysicistPath)MapLoader.CreateInstance(pathEle, "class");
                    if (path != null)
                    {                            
                        path.Target = this.target;
                        this.AddPath(path);
                    }
                }
            }
        }

        private void OnPathCompleted(object sender, EventArgs e)
        {
            var oldPath = sender as PhysicistPath;
            if (oldPath != null)
            {
                if (oldPath.TargetPathUponCompletion != null)
                {
                    this.CurrentPath = oldPath.TargetPathUponCompletion;
                }
                else
                {
                    oldPath.PathCompleted -= this.OnPathCompleted;
                    this.IsEnabled = false;
                }
            }
        }
    }
}
