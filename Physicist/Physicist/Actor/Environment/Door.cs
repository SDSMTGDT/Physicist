namespace Physicist.Actors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using FarseerPhysics.Collision;
    using FarseerPhysics.Common;
    using FarseerPhysics.Dynamics;
    using FarseerPhysics.Factories;
    using Microsoft.Xna.Framework;
    using Physicist.Controls;
    using Physicist.Events; 
    using Physicist.Extensions;

    public class Door : Actor, ILayerTransition
    {
        private ProximityTrigger doorTrigger;
        private ProximityTrigger layerTrigger;
        private bool isDoorOpen = false;

        public Door()
            : base()
        {
            this.Transitioning = false;
        }

        public event EventHandler<LayerTransitionEventArgs> LayerTransition;

        public event EventHandler DoorOpened;

        public event EventHandler DoorClosed;

        public bool Transitioning
        {
            get;
            private set;
        }

        public bool Receiving
        {
            get;
            private set;
        }

        public string TargetDoor
        {
            get;
            private set;
        }

        public void SetPlayer(Player player)
        {
            if (player != null)
            {
                player.Position = this.Position + new Vector2(0, 37f);
                this.Sprites["Door"].Depth = 0.5f;
                this.Sprites["Door"].CurrentAnimationString = "Open";
                this.Transitioning = true;
                this.Receiving = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (gameTime != null)
            {
                KeyboardDebouncer ks = KeyboardController.GetState();
                if (!this.Transitioning && this.doorTrigger.IsActive)
                {
                    if (ks.IsKeyDown(KeyboardController.InteractionKey, true) && string.Compare(this.Sprites["Door"].CurrentAnimationString, "Open", StringComparison.CurrentCulture) != 0)
                    {
                        this.Sprites["Door"].CurrentAnimationString = "Open";
                        this.Transitioning = true;
                    }
                    else if (this.isDoorOpen && this.layerTrigger.IsActive)
                    {
                        this.Sprites["Door"].CurrentAnimationString = "Close";
                        this.Sprites["Door"].Depth = 1f;
                        this.Transitioning = true;
                    }
                }
                else if (string.Compare(this.Sprites["Door"].CurrentAnimationString, "Open", StringComparison.CurrentCulture) == 0 && !this.doorTrigger.IsActive && !this.Receiving)
                {
                    this.isDoorOpen = false;
                    uint index = this.Sprites["Door"].CurrentFrame;
                    this.Sprites["Door"].CurrentAnimationString = "Close";
                    this.Sprites["Door"].CurrentFrame = index;
                    this.Transitioning = true;
                }

                base.Update(gameTime);
            }
        }

        public override void XmlDeserialize(XElement element)
        {
            if (element != null)
            {
                this.TargetDoor = element.GetAttribute("targetDoor", string.Empty);

                base.XmlDeserialize(element.Element("Actor"));

                this.Sprites["Door"].AnimationComplete += this.DoorChanged;
                this.CreateTrigger();
            }
        }

        public void CreateTrigger()
        {
            this.Body.CollidesWith = PhysicistCategory.Player;
            this.Body.CollisionCategories = PhysicistCategory.Physical;

            AABB aabb;
            this.Body.FixtureList[0].GetAABB(out aabb, 0);
            using (Body triggerBody = new Body(this.World))
            {
                Fixture doorFixture = FixtureFactory.AttachRectangle(aabb.Width, aabb.Height, 0, Vector2.Zero, triggerBody);
                this.doorTrigger = new ProximityTrigger(this.Body, doorFixture, this.World);
                this.doorTrigger.Initialize(null);
                this.doorTrigger.IsSensor = true;
                this.doorTrigger.IsContinuous = true;
                triggerBody.DestroyFixture(doorFixture);

                Fixture layerFixture = FixtureFactory.AttachRectangle(aabb.Width / 7, aabb.Height, 0, Vector2.Zero, triggerBody);
                this.layerTrigger = new ProximityTrigger(this.Body, layerFixture, this.World);
                this.layerTrigger.Initialize(null);
                this.layerTrigger.IsSensor = true;
                this.layerTrigger.IsContinuous = true;
                triggerBody.DestroyFixture(layerFixture);
            }

            this.Body.FixtureList[0].CollisionCategories = PhysicistCategory.None;
            this.Body.FixtureList[0].CollidesWith = PhysicistCategory.None;
        }

        private void DoorChanged(object sender, AnimationCompleteEventArgs e)
        {
            GameSprite sprite = sender as GameSprite;
            if (sprite != null && e != null)
            {
                if (string.Compare(sprite.SpriteName, "Door", StringComparison.CurrentCulture) == 0)
                {
                    if (string.Compare(e.Animation.Name, "Open", StringComparison.CurrentCulture) == 0)
                    {
                        this.Transitioning = false;
                        if (!this.Receiving)
                        {
                            this.isDoorOpen = true;
                            if (this.DoorOpened != null)
                            {
                                this.DoorOpened(this, null);
                            }
                        }
                        else
                        {
                            this.Sprites["Door"].CurrentAnimationString = "Close";
                            this.Sprites["Door"].Depth = 0.5f;
                        }
                    }
                    else if (string.Compare(e.Animation.Name, "Close", StringComparison.CurrentCulture) == 0)
                    {
                        if (this.isDoorOpen && this.layerTrigger.IsActive)
                        {
                            this.isDoorOpen = false;
                            this.Sprites["Door"].Depth = 0.5f;

                            if (this.LayerTransition != null)
                            {
                                this.LayerTransition(this, new LayerTransitionEventArgs(this.Position, this.TargetDoor));
                            }
                        }

                        if (this.Receiving)
                        {
                            this.Receiving = false;
                        }

                        this.Sprites["Door"].CurrentAnimationString = "Idle";
                        this.Transitioning = false;
                        if (this.DoorClosed != null)
                        {
                            this.DoorClosed(this, null);
                        }
                    }
                }
            }
        }
    }
}
