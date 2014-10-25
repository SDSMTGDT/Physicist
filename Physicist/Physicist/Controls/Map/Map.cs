namespace Physicist.Controls
{
    using System.Collections.Generic;
    using FarseerPhysics.Collision.Shapes;
    using FarseerPhysics.Common;
    using FarseerPhysics.Dynamics;
    using FarseerPhysics.Factories;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Physicist.Actors;
    using Physicist.Events;
    using Physicist.Extensions;
    using Physicist.Extensions.Primitives;

    public class Map
    {
        private List<IUpdate> updateObjects = new List<IUpdate>();
        private List<List<IDraw>> drawObjects = new List<List<IDraw>>();
        private Dictionary<string, IName> namedObjects = new Dictionary<string, IName>();

        private List<IBackgroundObject> backgroundObjects = new List<IBackgroundObject>();
        private List<IMapObject> mapObjects = new List<IMapObject>();
        private List<IActor> actors = new List<IActor>();
        private List<IMediaInfo> mediaReferences = new List<IMediaInfo>();

        private List<Player> players = new List<Player>();

        private List<object> unknownObjects = new List<object>();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Loop Body is tracked and disposed by world")]
        public Map(World world, int width, int height)
        {
            this.Width = width;
            this.Height = height;

            for (int i = 0; i < 4; i++)
            {
                this.drawObjects.Add(new List<IDraw>());
            }

            Vertices mapBounds = new Vertices() 
                            { 
                                Vector2.Zero, 
                                new Vector2(0, this.Height), 
                                new Vector2(this.Width, this.Height), 
                                new Vector2(this.Width, 0) 
                            };

            BodyFactory.CreateLoopShape(world, mapBounds.ToSimUnits()).Friction = 1f;
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

        public IEnumerable<IMapObject> MapObjects
        {
            get
            {
                return this.mapObjects;
            }
        }

        public IEnumerable<Player> Players
        {
            get
            {
                return this.players;
            }
        }

        public IEnumerable<IBackgroundObject> BackgroundObjects
        {
            get
            {
                return this.backgroundObjects;
            }
        }

        public IEnumerable<IActor> Actors
        {
            get
            {
                return this.actors;
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
                    this.updateObjects.Add(updateObj);
                    known = true;
                }

                var actor = instance as IActor;
                if (actor != null)
                {
                    var player = instance as Player;
                    if (player != null)
                    {
                        this.players.Add(player);
                    }

                    this.actors.Add(actor);
                    this.drawObjects[0].Add(actor);
                    known = true;
                }

                var background = instance as IBackgroundObject;
                if (background != null)
                {
                    this.backgroundObjects.Add(background);

                    var drawObj = instance as IDraw;
                    if (drawObj != null)
                    {
                        this.drawObjects[1].Add(drawObj);
                    }

                    known = true;
                }

                var mapobject = instance as IMapObject;
                if (mapobject != null)
                {
                    this.mapObjects.Add(mapobject);
                    this.drawObjects[2].Add(mapobject);
                    known = true;
                }

                var nameObj = instance as IName;
                if (nameObj != null && !string.IsNullOrEmpty(nameObj.Name))
                {
                    this.namedObjects.Add(nameObj.Name, nameObj);
                    known = true;
                }

                if (!known)
                {
                    this.unknownObjects.Add(instance);
                }
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
            int depth = 3;
            if (drawObject is IBackgroundObject)
            {
                depth = 0;
            }
            else if (drawObject is IMapObject)
            {
                depth = 1;
            }
            else if (drawObject is IActor)
            {
                depth = 2;
            }

            this.drawObjects[depth].Add(drawObject);
        }
        
        public void AddUpdateObject(IUpdate updateObject)
        {
            this.updateObjects.Add(updateObject);
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
            this.drawObjects.ForEach(list => list.ForEach(item => item.Draw(sb)));
        }

        public void Update(GameTime gameTime)
        {
            this.updateObjects.ForEach(item => item.Update(gameTime));
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
