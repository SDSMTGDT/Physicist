namespace Physicist.MainGame.Controls
{
    using System;
    using Microsoft.Xna.Framework;
    using Physicist.Controls.Screens;
    using Physicist.Types.Controllers;
    using Physicist.Types.Enums;
    using Physicist.Types.Interfaces;
    using Physicist.Types.Xna;

    /// <summary>
    /// MenuScreen is the 'main' screen for this section of
    /// your game. It is used to host content and update components.
    /// </summary>
    public partial class MenuScreen : GameScreen, ISystemScreen
    {
        private const SystemScreen SystemScreenType = SystemScreen.MenuScreen;

        public MenuScreen() :
            base(SystemScreenType.ToString())
        {
            this.BackgroundColor = new Color(0, 0, 0, 0.8f);
        }

        public SystemScreen ScreenType
        {
            get { return MenuScreen.SystemScreenType; }
        }

        /// <summary>
        /// Allows the screen to perform any initialization logic before starting.
        /// Use it to load any non-graphical content
        /// </summary>
        public override void Initialize()
        {
        }

        /// <summary>
        /// LoadContent is called once per instance of screen and is used to 
        /// load all of the graphical content
        /// </summary>
        public override bool LoadContent()
        {
            return base.LoadContent();
        }

        /// <summary>
        /// UnloadContent is called once per instance of screen and is used to 
        /// unload all of the graphical content
        /// </summary>
        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public void StartExtras(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(SystemScreen.ExtrasScreen);
        }

        public void StartOptions(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(SystemScreen.OptionsScreen);
        }

        public void StartPhysicist(object sender, EventArgs e)
        {
            using (System.Windows.Forms.OpenFileDialog selectMap = new System.Windows.Forms.OpenFileDialog())
            {
                selectMap.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + @"Content\Levels";
                selectMap.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
                selectMap.FilterIndex = 1;
                selectMap.RestoreDirectory = true;

                if (selectMap.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    ScreenManager.AddScreen(MenuScreen.MakePhysicistScreen(selectMap.FileName));
                }
            }
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
                if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape, true))
                {
                    this.PopScreen();
                }

                base.Update(gameTime);
            }
        }

        /// <summary>
        /// Called every time the screen is to re-draw itself
        /// </summary>
        /// <param name="sb">SpriteBatch for drawing, use sb.draw()</param>
        public override void Draw(FCCSpritebatch sb)
        {
            if (sb != null)
            {
                base.Draw(sb);
            }
        }

        private static GameScreen MakePhysicistScreen(string filePath)
        {
            PhysicistGameScreen screen = null;
            PhysicistGameScreen tempScreen = null;

            try
            {
                tempScreen = new PhysicistGameScreen("MainScreen", filePath);
                screen = tempScreen;
                tempScreen = null;
            }
            finally
            {
                if (tempScreen != null)
                {
                    tempScreen.Dispose();
                }
            }

            return screen;
        }
    }
}
