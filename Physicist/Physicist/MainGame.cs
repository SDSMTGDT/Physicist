namespace Physicist
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using FarseerPhysics;
    using FarseerPhysics.Common;
    using FarseerPhysics.Dynamics;
    using FarseerPhysics.Factories;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Physicist.Actors;
    using Physicist.Controls;
    using Physicist.Extensions;

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainGame : Game
    {
        private static World world;
        private static Map map;
        private static List<Actor> actors;
        private static List<string> maps;

        private Physicist.Controls.Viewport viewport;
        private CameraController camera;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public MainGame()
            : base()
        {
            this.graphics = new GraphicsDeviceManager(this);
        }

        public static World World
        {
            get
            {
                return MainGame.world;
            }
        }

        public static void RegisterActor(Actor actor)
        {
            MainGame.actors.Add(actor);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            FarseerPhysics.Settings.MaxPolygonVertices = 32;
            ContentController.Instance.Initialize(this.Content, "Content");
            MainGame.actors = new List<Actor>();
            MainGame.maps = new List<string>() { "Content\\Levels\\TestLevel.xml" };
                        
            //// TODO: Add your initialization logic here
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            this.spriteBatch = new SpriteBatch(GraphicsDevice);

            MainGame.world = new World(new Vector2(0f, 9.81f));
            ConvertUnits.SetDisplayUnitToSimUnitRatio(2f);

            MainGame.map = MapLoader.LoadMap(MainGame.maps[0]);
            if (MapLoader.HasErrors)
            {
                foreach (var error in MapLoader.Errors)
                {
                    System.Console.WriteLine(error);
                }
            }

            if (MapLoader.HasFailed || map == null)
            {
                throw new AggregateException(string.Format(CultureInfo.CurrentCulture, "Loading of Map: {0} has failed!", MainGame.maps[0]));
            }

            this.viewport = new Physicist.Controls.Viewport(new Extensions.Size(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height));
            this.camera = new CameraController();
            this.camera.CameraViewport = this.viewport;
            this.camera.Bounds = new Vector2(this.GraphicsDevice.Viewport.Width * 2, this.GraphicsDevice.Viewport.Height * 2);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (gameTime != null)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    this.Exit();
                }

                MainGame.World.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);

                foreach (var actor in MainGame.actors)
                {
                    Player player = actor as Player;
                    if (player != null)
                    {
                        this.camera.Following = player;
                        player.Update(gameTime, Keyboard.GetState());
                    }
                    else
                    {
                        actor.Update(gameTime);
                    }
                }

                // TODO: Add your update logic here
                this.camera.CenterOnFollowing();
                MainGame.map.Update(gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            this.spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, this.camera.Transform);

            MainGame.actors.ForEach(actor => actor.Draw(this.spriteBatch));
            MainGame.map.Draw(this.spriteBatch);

            base.Draw(gameTime);

            this.spriteBatch.End();
        }
    }
}
