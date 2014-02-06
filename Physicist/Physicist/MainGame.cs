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

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainGame : Game
    {
        private static World world;
        private static List<Actor> actors;
        private static List<string> maps;

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

            this.SetupWorld(MainGame.maps[0]);

            // TODO: use this.Content to load your game content here
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
                        player.Update(gameTime, Keyboard.GetState());
                    }
                    else
                    {
                        actor.Update(gameTime);
                    }
                }

                // TODO: Add your update logic here
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
            this.spriteBatch.Begin();

            MainGame.actors.ForEach(actor => actor.Draw(this.spriteBatch));

            base.Draw(gameTime);

            this.spriteBatch.End();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Loop Body is tracked and disposed by world")]
        private void SetupWorld(string mapPath)
        {
            MainGame.world = new World(new Vector2(0f, 9.81f));
            ConvertUnits.SetDisplayUnitToSimUnitRatio(100f);

            if (MapLoader.LoadMap(mapPath))
            {
                foreach (var error in MapLoader.Errors)
                {
                    System.Console.WriteLine(error);
                }
            }

            if (MapLoader.HasFailed)
            {
                System.Console.WriteLine(string.Format(CultureInfo.CurrentCulture, "Loading of Map: {0} has failed!", mapPath));
                throw new AggregateException("Error: Map Load Failure!");
            }

            Vertices borderVerts = new Vertices();
            borderVerts.Add(Vector2.Zero);
            borderVerts.Add(new Vector2(0, this.GraphicsDevice.Viewport.Height));
            borderVerts.Add(new Vector2(this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height));
            borderVerts.Add(new Vector2(this.GraphicsDevice.Viewport.Width, 0));

            BodyFactory.CreateLoopShape(MainGame.World, borderVerts).Friction = 10f;
        }
    }
}
