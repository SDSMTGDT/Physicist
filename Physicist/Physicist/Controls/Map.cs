namespace Physicist.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using FarseerPhysics.Collision.Shapes;
    using Microsoft.Xna.Framework.Graphics;

    public class Map
    {
        private List<Backdrop> backdrops = new List<Backdrop>();
        private List<BackgroundMusic> backgroundMusic = new List<BackgroundMusic>();
        private List<IXmlSerializable> mapObjects = new List<IXmlSerializable>();

        public Map()
        {
        }

        public IEnumerable<Backdrop> Backdrops
        {
            get
            {
                return this.backdrops;
            }
        }

        public IEnumerable<BackgroundMusic> BackgroundMusic
        {
            get
            {
                return this.backgroundMusic;
            }
        }

        public IEnumerable<IXmlSerializable> MapObjects
        {
            get
            {
                return this.mapObjects;
            }
        }
    }
}
