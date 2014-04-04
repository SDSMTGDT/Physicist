﻿namespace Physicist.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using FarseerPhysics;
    using FarseerPhysics.Dynamics;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Physicist.Actors;

    public class PhysicistGameScreen : GameScreen, IPhysicistRegistration
    {
        private World world;
        private Map map;

        private List<Actor> actors;
        private List<string> maps;
        private string mapPath;

        public PhysicistGameScreen(string name, string mapPath) :
            base(name)
        {
            this.mapPath = mapPath;
        }

        public World World
        {
            get
            {
                return this.world;
            }
        }

        public Map Map
        {
            get
            {
                return this.map;
            }
        }

        public override void Initialize(GraphicsDevice graphicsDevice)
        {
            base.Initialize(graphicsDevice);
            this.actors = new List<Actor>();
            this.maps = new List<string>() { this.mapPath };

            ConvertUnits.SetDisplayUnitToSimUnitRatio(2f);
            FarseerPhysics.Settings.MaxPolygonVertices = 32;
        }

        public override bool LoadContent()
        {
            bool success = base.LoadContent();
            if (success)
            {
                this.world = new World(new Vector2(0f, 9.81f));

                this.map = MapLoader.Initialize(this.maps[0], this);
                if (this.map == null || !MapLoader.LoadCurrentMap())
                {
                    System.Console.WriteLine(string.Format(CultureInfo.CurrentCulture, "Loading of Map: {0} has failed!", this.maps[0]));
                    success = false;
                }

                this.actors[0].AttackDamage = 5;
                this.actors[0].CanBeDamaged = false;
            }

            return success;
        }

        public override void Update(GameTime gameTime)
        {
            if (gameTime != null)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || this.IsKeyDown(Keys.Escape, true))
                {
                    this.PopScreen();
                }

                if (this.IsKeyDown(Keys.P))
                {
                    ScreenManager.AddScreen(Enums.SystemScreen.PauseScreen);
                }

                this.World.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);

                foreach (var actor in this.actors)
                {
                    actor.Update(gameTime);
                }

                // TODO: Add your update logic here
                this.map.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(ISpritebatch sb)
        {
            this.actors.ForEach(actor => actor.Draw(sb));
            this.map.Draw(sb);

            base.Draw(sb);
        }

        public override void UnloadContent()
        {
            this.map.UnloadMedia();
            base.UnloadContent();
        }

        public void RegisterActor(Actor actor)
        {
            this.actors.Add(actor);
        }
    }
}