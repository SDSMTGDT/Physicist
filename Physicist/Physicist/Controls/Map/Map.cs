namespace Physicist.MainGame.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using FarseerPhysics.Dynamics;
    using Microsoft.Xna.Framework;
    using Physicist.Controls.Screens;
    using Physicist.MainGame.Actors;
    using Physicist.Types.Interfaces;
    using Physicist.Types.Controllers;
    using Physicist.Types.Util;
    using Physicist.Types.Xna;

    public class Map
    {
        private List<IUpdate> updateObjects = new List<IUpdate>();       
        private NamedKeyedCollection<IName> namedObjects = new NamedKeyedCollection<IName>();

        private NamedKeyedCollection<MapLayer> mapLayers = new NamedKeyedCollection<MapLayer>();

        private List<IBackgroundObject> backgroundObjects = new List<IBackgroundObject>();
        private List<IMapObject> mapObjects = new List<IMapObject>();
        private List<IActor> actors = new List<IActor>();
        private List<IMediaInfo> mediaReferences = new List<IMediaInfo>();

        private List<Player> players = new List<Player>();

        private Dictionary<string, Tuple<ILayerTransition, string>> transitionObjects = new Dictionary<string, Tuple<ILayerTransition, string>>();

        private List<object> unknownObjects = new List<object>();

        private string activeLayer = string.Empty;
        private string previouslayer = string.Empty;

        private bool transition = false;
        private float fadeStep = 0.05f;

        public Map(World world, int width, int height)
        {
            this.Width = width;
            this.Height = height;

            this.World = world;
        }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public World World { get; private set; }

        public string ActiveLayer 
        {
            get
            {
                return this.activeLayer;
            }

            set
            {
                if (this.mapLayers.Contains(value))
                {
                    this.previouslayer = this.activeLayer;
                    this.activeLayer = value;
                }
            }
        }

        public NamedKeyedCollection<IName> NamedObjects
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

        public NamedKeyedCollection<MapLayer> MapLayers
        {
            get
            {
                return this.mapLayers;
            }
        }

        public void AddMapLayer(MapLayer layer)
        {
            if (layer != null)
            {
                this.mapLayers.Add(layer);
                if (this.mapLayers.Count == 1)
                {
                    this.ActiveLayer = layer.Name;
                }
            }
        }

        public void AddObjectToMap(object instance, string layer)
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

                var bodyObj = instance as IBody;
                if (bodyObj != null)
                {
                    var actor = instance as IActor;
                    var player = instance as Player;
                    if (actor != null)
                    {
                        if (player != null)
                        {
                            this.players.Add(player);
                            this.ActiveLayer = this.mapLayers[layer].Name;
                            player.Body.CollisionLayer = this.mapLayers[layer].CollisionLayer;
                        }

                        this.actors.Add(actor);
                        known = true;
                    }

                    this.mapLayers[layer].AddLayerObject(actor);
                }

                var background = instance as IBackgroundObject;
                if (background != null)
                {
                    this.backgroundObjects.Add(background);
                    this.mapLayers[layer].AddLayerObject(instance as IDraw);
                    known = true;
                }

                var mapobject = instance as IMapObject;
                if (mapobject != null)
                {
                    this.mapObjects.Add(mapobject);
                    this.mapLayers[layer].AddLayerObject(mapobject);
                    known = true;
                }

                var nameObj = instance as IName;
                if (nameObj != null && !string.IsNullOrEmpty(nameObj.Name))
                {
                    this.namedObjects.Add(nameObj);
                    known = true;
                }

                var transitionObj = instance as ILayerTransition;
                if (transitionObj != null)
                {
                    this.transitionObjects.Add(transitionObj.Name, Tuple.Create(transitionObj, layer));
                    transitionObj.LayerTransition += this.MakeTransition;
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
                this.namedObjects.Add(namedObject);
            }
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

        public void Draw(FCCSpritebatch sb)
        {
            if (sb != null)
            {
                foreach (var layer in this.mapLayers)
                {
                    if (string.Compare(layer.Name, this.ActiveLayer, StringComparison.CurrentCulture) == 0)
                    {
                        layer.Draw(sb);
                    }

                    if (this.transition && string.Compare(layer.Name, this.previouslayer, StringComparison.CurrentCulture) == 0)
                    {
                        layer.Draw(sb);
                    }
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            if (this.transition)
            {
                this.mapLayers[this.previouslayer].Fade -= this.fadeStep;
                this.mapLayers[this.activeLayer].Fade += this.fadeStep;

                if (this.mapLayers[this.previouslayer].Fade <= 0 || this.mapLayers[this.activeLayer].Fade >= 1f)
                {
                    this.mapLayers[this.previouslayer].Fade = 0;
                    this.mapLayers[this.activeLayer].Fade = 1f;
                    this.transition = false;
                }
            }

            this.updateObjects.ForEach(item => item.Update(gameTime));
        }

        public void UnloadMedia()
        {
            foreach (var reference in this.mediaReferences)
            {
                ContentController.Instance.UnloadContent(reference.Name, reference.Format);
            }

            foreach (var layer in this.mapLayers)
            {
                layer.UnloadMedia();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Screen manager tracks object")]
        private void MakeTransition(object sender, LayerTransitionEventArgs e)
        {
            if (sender != null && e != null)
            {
                if (string.Compare(e.TargetDoor, "LEVEL_END", StringComparison.CurrentCulture) == 0)
                {
                    ScreenManager.AddScreen(new LevelEndScreen());
                }
                else
                {
                    string target = e.TargetDoor;
                    int sublevelIndex = e.TargetDoor.IndexOf("{", StringComparison.CurrentCulture);
                    if (sublevelIndex > 0)
                    {
                        string sublevelValue = e.TargetDoor.Substring(sublevelIndex + 1, e.TargetDoor.IndexOf("}", sublevelIndex, StringComparison.CurrentCulture) - sublevelIndex - 1).Trim();
                        target = e.TargetDoor.Substring(0, sublevelIndex);
                        var elevator = this.transitionObjects[target].Item1 as Actors.Environment.Elevator;
                        if (elevator != null)
                        {
                            elevator.SetToFloor(uint.Parse(sublevelValue, CultureInfo.CurrentCulture));
                        }
                    }

                    this.mapLayers[this.ActiveLayer].RemovePlayerObject(this.Players.ElementAt(0));
                    this.ActiveLayer = this.transitionObjects[target].Item2;

                    this.transition = true;
                    this.mapLayers[this.previouslayer].Fade = 1f;
                    this.mapLayers[this.activeLayer].Fade = 0f;

                    this.transitionObjects[target].Item1.SetPlayer(this.Players.ElementAt(0));
                    this.Players.ElementAt(0).Body.LinearVelocity += new Vector2(0f, -0.0001f);
                    this.mapLayers[this.ActiveLayer].AddLayerObject(this.Players.ElementAt(0));
                }
            }
        }
    }
}
