namespace Physicist.MainGame.Controls
{
    using System;
    using System.Collections.Generic;
    using FarseerPhysics.Common;
    using FarseerPhysics.Dynamics;
    using FarseerPhysics.Factories;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Physicist.MainGame.Actors;
    using Physicist.MainGame.Extensions;
    using Physicist.Types.Interfaces;
    using Physicist.Types.Xna;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "Follows Monogame pattern")]
    public class MapLayer : IName
    {
        private Texture2D fill;
        private List<IDraw> drawObjects = new List<IDraw>();
        private List<Player> players = new List<Player>();
        private Vertices layerBounds;
        private List<IBody> bodies = new List<IBody>();
        private Body layerBody;
        private uint collisionLayer = 0;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Loop Body is tracked and disposed by world")]
        public MapLayer(World world, string name, int width, int height, int depth, Vector2 offset, int mapHeight, uint collisionLayer)
        {
            this.Depth = depth;
            this.Width = width;
            this.Height = height;
            this.Name = name;
            this.Offset = offset;

            this.Fade = 1f;

            this.layerBounds = new Vertices() 
            { 
                new Vector2(this.Offset.X, mapHeight - this.Offset.Y),
                new Vector2(this.Offset.X + this.Width, mapHeight - this.Offset.Y), 
                new Vector2(this.Offset.X + this.Width, mapHeight - (this.Offset.Y + this.Height)),
                new Vector2(this.Offset.X, mapHeight - (this.Offset.Y + this.Height))
            };

            this.layerBody = BodyFactory.CreateLoopShape(world, this.layerBounds.ToSimUnits());
            this.layerBody.CollisionCategories = PhysicistCategory.All;
            this.layerBody.CollidesWith = PhysicistCategory.All;
            this.layerBody.CollisionLayer = 0;
            this.layerBody.UserData = this;

            this.CollisionLayer = collisionLayer;

            this.fill = new Texture2D(MainGame.GraphicsDev, 1, 1);
            this.fill.SetData(new Color[1] { Color.Black });
        }

        public string Name { get; private set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public int Depth { get; private set; }

        public Vector2 Offset { get; private set; }

        public IEnumerable<IBody> Bodies
        {
            get
            {
                return this.bodies;
            }
        }

        public float Fade
        {
            get;
            set;
        }

        public uint CollisionLayer
        {
            get
            {
                return this.collisionLayer;
            }

            set
            {
                this.collisionLayer = value;
                if (this.layerBody != null)
                {
                    this.layerBody.CollisionLayer = value;
                }

                this.bodies.ForEach(body => body.Body.CollisionLayer = value);
                this.players.ForEach(player => player.Body.CollisionLayer = value);
            }
        }

        public void Draw(FCCSpritebatch sb)
        {
            if (sb != null)
            {
                var prefade = sb.Fade;
                sb.Fade = this.Fade;
                sb.Draw(this.fill, new Rectangle((int)this.layerBounds[0].X, (int)this.layerBounds[0].Y, this.Width, 1), Color.White);
                sb.Draw(this.fill, new Rectangle((int)this.layerBounds[3].X, (int)this.layerBounds[3].Y, 1, this.Height), Color.White);
                sb.Draw(this.fill, new Rectangle((int)this.layerBounds[3].X, (int)this.layerBounds[3].Y, this.Width, 1), Color.White);
                sb.Draw(this.fill, new Rectangle((int)this.layerBounds[2].X, (int)this.layerBounds[2].Y, 1, this.Height), Color.White);

                this.drawObjects.ForEach(drawObj => drawObj.Draw(sb));
                this.players.ForEach(player => player.Draw(sb));
                sb.Fade = prefade;
            }
        }

        public void AddLayerObject(object instance)
        {
            var drawObject = instance as IDraw;
            var player = instance as Player;
            var body = instance as IBody;

            if (player != null)
            {
                this.players.Add(player);
                player.Body.CollisionLayer = this.CollisionLayer;
            }
            else
            {
                if (body != null)
                {
                    body.Body.CollisionLayer = this.collisionLayer;
                    this.bodies.Add(body);
                }

                if (drawObject != null)
                {
                    this.drawObjects.Add(drawObject);
                }
            }
        }

        public void RemovePlayerObject(Player player)
        {
            if (player != null)
            {
                this.players.Remove(player);
            }
        }

        public void RemoveDrawObject(IName namedObject)
        {
            this.drawObjects.RemoveAll(match =>
            {
                bool found = false;
                var named = match as IName;
                if (named != null && string.Compare(namedObject.Name, named.Name, StringComparison.CurrentCulture) == 0)
                {
                    found = true;
                }

                return found;
            });
        }

        public void UnloadMedia()
        {
            if (this.fill != null)
            {
                this.fill.Dispose();
            }
        }
    }
}
