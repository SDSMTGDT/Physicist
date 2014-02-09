namespace Physicist.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using FarseerPhysics.Collision.Shapes;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Map
    {
        private List<Backdrop> backdrops = new List<Backdrop>();
        private List<BackgroundMusic> backgroundMusic = new List<BackgroundMusic>();
        private List<MapObject> mapObjects = new List<MapObject>();

        public Map()
        {
        }

        public IEnumerable<IXmlSerializable> Backdrops
        {
            get
            {
                return this.backdrops;
            }
        }

        public IEnumerable<IXmlSerializable> BackgroundMusic
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

        public void AddMapObject(MapObject mapObject)
        {
            this.mapObjects.Add(mapObject);
        }

        public void AddBackgroundMusic(BackgroundMusic music)
        {
            this.backgroundMusic.Add(music);
        }

        public void AddBackdrop(Backdrop backdrop)
        {
            this.backdrops.Add(backdrop);
        }

        public void Draw(SpriteBatch sb)
        {
            this.mapObjects.ForEach(mapObject => mapObject.Draw(sb));
            this.backdrops.ForEach(backdrop => backdrop.Draw(sb));
        }

        public void Update(GameTime gameTime)
        {
            this.backgroundMusic.ForEach(music => music.Update(gameTime));
        }
    }
}
