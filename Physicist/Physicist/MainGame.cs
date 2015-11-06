namespace Physicist.MainGame
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Physicist.MainGame.Controls;
    using Physicist.Controls.Screens;
    using Physicist.Types.Controllers;
    using Physicist.Types.Interfaces;
    using Physicist.Types.Xna;

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainGame : Game
    {
        private static GraphicsDeviceManager graphics;
        private static GameWindow window;
        private FCCSpritebatch spriteBatch;

        public MainGame()
            : base()
        {
            MainGame.graphics = new GraphicsDeviceManager(this);
            MainGame.window = this.Window;
        }

        public static GraphicsDevice GraphicsDev 
        { 
            get; 
            private set; 
        }

        public static ContentManager ContentManager
        {
            get;
            private set;
        }

        public static void SetWindowBounds(Rectangle bounds)
        {
            OpenTK.GameWindow win = null;
            var type = typeof(OpenTKGameWindow);
            System.Reflection.FieldInfo field = type.GetField("window", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null)
            {
                win = field.GetValue(MainGame.window) as OpenTK.GameWindow;
            }

            if (win != null)
            {
                win.Y = bounds.Y;
                win.X = bounds.X;
                MainGame.graphics.PreferredBackBufferHeight = bounds.Height;
                MainGame.graphics.PreferredBackBufferWidth = bounds.Width;
                MainGame.graphics.ApplyChanges();
            }
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            FarseerPhysics.Settings.UseFPECollisionCategories = true;

            // FarseerPhysics.Settings.DefaultFixtureCollisionCategories = PhysicistCategory.All;
            MainGame.SetWindowBounds(new Rectangle(400, 10, 800, 480));
            ContentController.Instance.Initialize(this.Content, "Content");
            MainGame.GraphicsDev = this.GraphicsDevice;
            MainGame.ContentManager = this.Content;
            this.spriteBatch = new FCCSpritebatch(this.GraphicsDevice);
            AssetCreator.Instance.Initialize(this.GraphicsDevice);
            this.IsMouseVisible = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            ContentController.Instance.LoadContent<SpriteFont>("MenuFont", "System\\Fonts\\Pericles6");
            ContentController.Instance.LoadContent<Texture2D>("ContentLoadError", "System\\Textures\\ContentLoadError");
            ContentController.Instance.LoadContent<SpriteFont>("DebugFount", "System\\Fonts\\DebugFont");
        }

        /// <summary>
        /// Called before first update loop place to initialized components
        /// dependant upon content
        /// </summary>
        protected override void BeginRun()
        {
            ScreenManager.Initialize(
                this.GraphicsDevice, 
                new ISystemScreen[] { new MenuScreen(), new OptionsScreen(), new PauseScreen()});

            ScreenManager.Quit += this.RequestQuit;

            base.BeginRun();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            ScreenManager.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            ScreenManager.Update(gameTime);
            KeyboardController.Update(gameTime);
            MouseController.Update(gameTime);
            SoundController.Update();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            ScreenManager.Draw(this.spriteBatch);
            base.Draw(gameTime);
        }

        protected virtual void RequestQuit(object sender, EventArgs e)
        {
            this.Exit();
        }
    }
}
