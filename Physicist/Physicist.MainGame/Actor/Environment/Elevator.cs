namespace Physicist.MainGame.Actors.Environment
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Linq;
    using FarseerPhysics.Dynamics;
    using FarseerPhysics.Factories;
    using Microsoft.Xna.Framework;
    using Physicist.Events;
    using Physicist.MainGame.Actors;
    using Physicist.MainGame.Controls;
    using Physicist.MainGame.Extensions;
    using Physicist.Types.Controllers;
    using Physicist.Types.Interfaces;
    using Physicist.Types.Util;

    public class Elevator : Actor, ILayerTransition
    {
        private ProximityTrigger elevatorTrigger;
        private Dictionary<int, string> targetDoors = new Dictionary<int, string>();

        private int width = 0;
        private int height = 0;

        public Elevator()
            : base()
        {
            this.FloorCount = 0;
            this.CurrentFloor = 0;
            this.Transitioning = false;
        }

        public event EventHandler<LayerTransitionEventArgs> LayerTransition;

        public string TargetDoor
        {
            get
            {
                return this.targetDoors[this.CurrentFloor];
            }
        }

        public int FloorCount
        {
            get;
            set;
        }

        public int CurrentFloor
        {
            get;
            private set;
        }

        public bool Transitioning
        {
            get;
            private set;
        }

        public void SetPlayer(IPosition player)
        {
            if (player != null)
            {
                player.Position = this.Position + new Vector2(this.width + 2, this.height + 45);
            }
        }

        public void SetToFloor(uint floor)
        {
            if (floor < this.FloorCount)
            {
                bool playerInside = this.elevatorTrigger.IsActive;

                this.PathManager.StopPathing();
                foreach (var p in this.PathManager.Paths)
                {
                    if (int.Parse(p.Name, CultureInfo.CurrentCulture) == floor)
                    {
                        var node = p.Nodes.ElementAt(0) as ApproachPositionPathNode;
                        if (node != null)
                        {
                            this.Position = node.TargetLocation;
                        }
                    }
                }

                if (playerInside)
                {
                    Map.Players.ElementAt(0).Position = this.Position + new Vector2(this.width / 2, this.height / 2).ToSimUnits();
                }

                this.CurrentFloor = (int)floor;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (gameTime != null)
            {
                KeyboardDebouncer ks = KeyboardController.GetState();
                if (!this.Transitioning && this.elevatorTrigger.IsActive)
                {
                    PhysicistPath path = null;
                    int floordif = int.MaxValue;
                    if (ks.IsKeyDown(KeyboardController.UpKey, true))
                    {
                        if (this.CurrentFloor != 0)
                        {
                            foreach (var p in this.PathManager.Paths)
                            {
                                int floorVal = int.Parse(p.Name, CultureInfo.CurrentCulture);
                                if ((floorVal < this.CurrentFloor) && (this.CurrentFloor - floorVal < floordif))
                                {
                                    floordif = this.CurrentFloor - floorVal;
                                    path = p;
                                }
                            }
                        }
                    }
                    else if (ks.IsKeyDown(KeyboardController.DownKey, true))
                    {
                        if (this.CurrentFloor != this.FloorCount - 1)
                        {
                            foreach (var p in this.PathManager.Paths)
                            {
                                int floorVal = int.Parse(p.Name, CultureInfo.CurrentCulture);
                                if ((floorVal > this.CurrentFloor) && (floorVal - this.CurrentFloor < floordif))
                                {
                                    floordif = floorVal - this.CurrentFloor;
                                    path = p;
                                }
                            }
                        }
                    }
                    else if (ks.IsKeyDown(KeyboardController.InteractionKey, true))
                    {
                        if (this.LayerTransition != null)
                        {
                            this.LayerTransition(this, new LayerTransitionEventArgs(this.Position, this.TargetDoor));
                        }
                    }

                    if (path != null)
                    {
                        this.Transitioning = true;
                        path.Reset();
                        path.PathCompleted += this.FloorReached;
                        path.IsEnabled = true;
                        this.PathManager.CurrentPath = path.Name;
                    }
                }

                base.Update(gameTime);
            }
        }

        public override void XmlDeserialize(XElement element)
        {
            if (element != null)
            {
                this.FloorCount = element.GetAttribute("floorCount", 0);
                this.CurrentFloor = element.GetAttribute("startingFloor", 0);

                element.Elements("Floor").ForEach(floor => this.targetDoors.Add(floor.GetAttribute("level", -1), floor.GetAttribute("targetDoor", "LEVEL_END")));

                base.XmlDeserialize(element.Element("Actor"));

                this.CreateTrigger();
                this.Body.IgnoreGravity = true;
            }
        }

        public void CreateTrigger()
        {
            var shape = this.Body.FixtureList[0].Shape as FarseerPhysics.Collision.Shapes.ChainShape;
            if (shape != null)
            {
                Vector2 svert = shape.Vertices[0];
                for (int i = 1; i < shape.Vertices.Count; i++)
                {
                    if (Math.Abs(shape.Vertices[i].X - svert.X) > this.width)
                    {
                        this.width = (int)Math.Abs(shape.Vertices[i].X - svert.X);
                    }

                    if (Math.Abs(shape.Vertices[i].Y - svert.Y) > this.height)
                    {
                        this.height = (int)Math.Abs(shape.Vertices[i].Y - svert.Y);
                    }
                }
            }

            this.Body.CollidesWith = PhysicistCategory.Physical;
            this.Body.CollisionCategories = PhysicistCategory.Physical;

            using (var boxBody = new Body(this.World))
            {
                Fixture boxFixture = FixtureFactory.AttachRectangle(this.width, this.height, 0, new Vector2((this.width / 2) + 2, (this.height / 2) + 5), boxBody);
                this.elevatorTrigger = new ProximityTrigger(this.Body, boxFixture, this.World);
                this.elevatorTrigger.Initialize(null);
                this.elevatorTrigger.IsSensor = true;
                this.elevatorTrigger.IsContinuous = true;
                boxBody.DestroyFixture(boxFixture);
            }
        }

        private void FloorReached(object sender, EventArgs e)
        {
            var path = sender as PhysicistPath;
            if (path != null)
            {
                path.PathCompleted -= this.FloorReached;
                path.IsEnabled = false;
                this.CurrentFloor = int.Parse(path.Name, CultureInfo.CurrentCulture);
            }

            this.Transitioning = false;
        }
    }
}
