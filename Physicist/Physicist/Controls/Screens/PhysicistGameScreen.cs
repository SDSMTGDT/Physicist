namespace Physicist.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using FarseerPhysics;
    using FarseerPhysics.DebugView;
    using FarseerPhysics.Dynamics;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Physicist.Actors;

    /// <summary>
    /// PhysicistGameScreen is the 'main' screen for this section of
    /// your game. It is used to host content and update components.
    /// </summary>
    public partial class PhysicistGameScreen : GameScreen, IPhysicistGameScreen
    {
        private World world;
        private Map map;
        private DebugViewXNA debugView = null;
        private Matrix debugViewMatrix;

        private List<string> maps;
        private string mapPath;
        private float gravityScalar;

        private bool showDebugView = false;

        public PhysicistGameScreen(string name, string mapPath) :
            base(name)
        {
            this.gravityScalar = 5.0f;
            this.mapPath = mapPath;
        }

        public float ScreenRotation { get; set; }

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

        /// <summary>
        /// Allows the screen to perform any initialization logic before starting.
        /// Use it to load any non-graphical content
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            this.maps = new List<string>() { this.mapPath };

            ConvertUnits.SetDisplayUnitToSimUnitRatio(2f);
            FarseerPhysics.Settings.MaxPolygonVertices = 32;
        }

        /// <summary>
        /// LoadContent is called once per instance of screen and is used to 
        /// load all of the graphical content
        /// </summary>
        public override bool LoadContent()
        {
            bool success = base.LoadContent();
            if (success)
            {
                this.world = new World(new Vector2(0f, 9.81f * this.gravityScalar));

                this.map = MapLoader.Initialize(this.maps[0], this);
                if (this.map == null || !MapLoader.LoadCurrentMap())
                {
                    System.Console.WriteLine(string.Format(CultureInfo.CurrentCulture, "Loading of Map: {0} has failed!", this.maps[0]));
                    success = false;
                }
                else
                {
                    this.debugView = new DebugViewXNA(this.world);
                    this.debugView.DefaultShapeColor = Color.White;
                    this.debugView.SleepingShapeColor = Color.LightGray;
                    this.debugView.LoadContent(this.GraphicsDevice, MainGame.ContentManager);
                    this.debugViewMatrix = Matrix.CreateOrthographicOffCenter(0f, ConvertUnits.ToSimUnits(this.map.Width), ConvertUnits.ToSimUnits(this.map.Height), 0f, 0f, .01f);
                }

                if (this.map.Players.Count() > 0)
                {
                    this.Camera.Following = this.map.Players.ElementAt(0);
                    SoundController.Map = this.map;
                    SoundController.Listener = this.map.Players.ElementAt(0).Listener;
                }
            }

            return success;
        }

        /// <summary>
        /// UnloadContent is called once per instance of screen and is used to 
        /// unload all of the graphical content
        /// </summary>
        public override void UnloadContent()
        {
            this.map.UnloadMedia();
            base.UnloadContent();
        }

        /// <summary>
        /// Allows the screen to run updating logic like checking user inputs,
        /// changing item properties or playing music
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (gameTime != null)
            {
                var state = KeyboardController.GetState();

                // Control debug view
                if (this.debugView != null && state.IsKeyDown(Keys.F1, true))
                {
                    if ((this.debugView.Flags & DebugViewFlags.Shape) == DebugViewFlags.Shape)
                    {
                        this.debugView.RemoveFlags(DebugViewFlags.Shape);
                    }
                    else
                    {
                        this.debugView.AppendFlags(DebugViewFlags.Shape);
                    }

                    this.showDebugView = !this.showDebugView;
                }

                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || state.IsKeyDown(Keys.Escape, true))
                {
                    this.PopScreen();
                }

                if (state.IsKeyDown(Keys.P))
                {
                    ScreenManager.AddScreen(Enums.SystemScreen.PauseScreen);
                }

                this.World.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);

                this.map.Update(gameTime);
                this.Camera.Rotation = this.ScreenRotation;
                this.Camera.CenterOnFollowing();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Called every time the screen is to re-draw itself
        /// </summary>
        /// <param name="sb">SpriteBatch for drawing, use sb.draw()</param>
        public override void Draw(FCCSpritebatch sb)
        {
            if (this.showDebugView)
            {
                this.debugView.RenderDebugData(this.debugViewMatrix, Matrix.Identity);
            }
            else
            {
                this.map.Draw(sb);
            }

            base.Draw(sb);
        }

        public void SetWorldRotation(float theta)
        {
            if (this.Camera != null)
            {
                theta = theta % (float)(Math.PI * 2);
                if (theta < 0)
                {
                    theta = (float)(2 * Math.PI) + theta;
                }

                this.World.Gravity = new Vector2((float)Math.Sin(theta) * 9.8f * this.gravityScalar, (float)Math.Cos(theta) * 9.8f * this.gravityScalar);
                this.Camera.Rotation = theta;
                this.ScreenRotation = theta;
            }
        }

        public void RotateWorld(float theta)
        {
            this.SetWorldRotation(this.ScreenRotation + theta);
        }

        public void ResetCameraGravity()
        {
            this.World.Gravity = new Vector2(0, 9.81f * this.gravityScalar);
            this.Camera.Reset();
        }
    }
}
