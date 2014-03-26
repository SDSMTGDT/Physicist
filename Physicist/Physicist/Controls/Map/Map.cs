namespace Physicist.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using FarseerPhysics.Collision.Shapes;
    using FarseerPhysics.Common;
    using FarseerPhysics.Factories;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Physicist.Actors;
    using Physicist.Events;
    using Physicist.Extensions;

    public class Map
    {
        private List<IUpdate> updateObjects = new List<IUpdate>();
        private List<IDraw> drawObjects = new List<IDraw>();
        private Dictionary<string, IName> namedObjects = new Dictionary<string, IName>();

        private List<object> unknownObjects = new List<object>();
        private List<object> loadedObjects = new List<object>();

        // Figure out how to remove
        private List<MapObject> mapObjects = new List<MapObject>();
        private List<Backdrop> backdrops = new List<Backdrop>();
        private List<BackgroundMusic> backgroundMusic = new List<BackgroundMusic>();
        private List<Player> players = new List<Player>();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Loop Body is tracked and disposed by world")]
        public Map(int width, int height)
        {
            this.Width = width;
            this.Height = height;

            Vertices mapBounds = new Vertices() 
                            { 
                                Vector2.Zero, 
                                new Vector2(0, this.Height), 
                                new Vector2(this.Width, this.Height), 
                                new Vector2(this.Width, 0) 
                            };

            BodyFactory.CreateLoopShape(MainGame.World, mapBounds.ToSimUnits()).Friction = 1f;
        }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public IDictionary<string, IName> NamedObjects
        {
            get
            {
                return this.namedObjects;
            }
        }

        public IEnumerable<Player> Players
        {
            get
            {
                return this.players;
            }
        }

        public IEnumerable<MapObject> MapObjects
        {
            get
            {
                return this.mapObjects;
            }
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

        public void AddObjectToMap(object instance)
        {
            bool known = false;
            if (instance != null)
            {
                var updateObj = instance as IUpdate;
                if (updateObj != null)
                {
                    var player = updateObj as Player;
                    if (player != null)
                    {
                        this.players.Add(player);
                    }
                    else
                    {
                        this.updateObjects.Add(updateObj);
                    }

                    known = true;
                }

                var drawObj = instance as IDraw;
                if (drawObj != null)
                {
                    this.drawObjects.Add(drawObj);
                    known = true;
                }

                var nameObj = instance as IName;
                if (nameObj != null)
                {
                    this.namedObjects.Add(nameObj.Name, nameObj);
                    known = true;
                }

                if (!known)
                {
                    this.unknownObjects.Add(instance);
                }

                this.loadedObjects.Add(instance);
            }
        }

        public void AddNamedObject(IName namedObject)
        {
            if (namedObject != null)
            {
                this.namedObjects.Add(namedObject.Name, namedObject);
            }
        }

        public void AddDrawObject(IDraw drawObject)
        {
            this.drawObjects.Add(drawObject);
        }
        
        public void AddUpdateObject(IUpdate updateObject)
        {
            var player = updateObject as Player;
            if (player != null)
            {
                this.players.Add(player);
            }
            else
            {
                this.updateObjects.Add(updateObject);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            this.drawObjects.ForEach(item => item.Draw(sb));
        }

        public void Update(GameTime gameTime)
        {
            this.updateObjects.ForEach(item => item.Update(gameTime));
        }
    }
}
