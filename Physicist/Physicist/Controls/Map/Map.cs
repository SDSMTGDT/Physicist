namespace Physicist.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using FarseerPhysics.Collision.Shapes;
    using FarseerPhysics.Common;
    using FarseerPhysics.Dynamics;
    using FarseerPhysics.Factories;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Physicist.Extensions;
    using Physicist.Extensions.Primitives;

    public class Map
    {
        private List<Backdrop> backdrops = new List<Backdrop>();
        private List<BackgroundMusic> backgroundMusic = new List<BackgroundMusic>();
        private List<MapObject> mapObjects = new List<MapObject>();
        private List<IMediaInfo> mediaReferences = new List<IMediaInfo>();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Loop Body is tracked and disposed by world")]
        public Map(World world, int width, int height)
        {
            this.Width = width;
            this.Height = height;
            
            BodyFactory.CreateLoopShape(
                            world, 
                            new Vertices() { Vector2.Zero, new Vector2(0, this.Height), new Vector2(this.Width, this.Height), new Vector2(this.Width, 0) }.ToSimUnits()).Friction = 10f;
        }

        public int Width { get; private set; }

        public int Height { get; private set; }

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

        public IEnumerable<IMediaInfo> MediaReferences
        {
            get
            {
                return this.mediaReferences;
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

        public void AddMediaReference(IMediaInfo reference)
        {
            if (reference != null)
            {
                this.mediaReferences.Add(reference);
            }
        }

        public void Draw(ISpritebatch sb)
        {
            this.mapObjects.ForEach(mapObject => mapObject.Draw(sb));
            this.backdrops.ForEach(backdrop => backdrop.Draw(sb));
        }

        public void Update(GameTime gameTime)
        {
            this.backgroundMusic.ForEach(music => music.Update(gameTime));
        }

        public void UnloadMedia()
        {
            foreach (var reference in this.mediaReferences)
            {
                ContentController.Instance.UnloadContent(reference.Name, reference.Format);
            }
        }
    }
}
