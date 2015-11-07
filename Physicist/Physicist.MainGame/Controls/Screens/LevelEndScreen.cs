namespace Physicist.MainGame.Controls
{
    using System;
    using Microsoft.Xna.Framework;
    using Physicist.Controls.Screens;
    using Physicist.Types.Controllers;
    using Physicist.Types.Enums;
    using Physicist.Types.Xna;

    /// <summary>
    /// LevelEndScreen is the 'main' screen for this section of
    /// your game. It is used to host content and update components.
    /// </summary>
    public partial class LevelEndScreen : GameScreen
    {
        public LevelEndScreen() :
            base("LevelEndScreen")
        {
            this.BackgroundColor = new Color(0, 0, 0, 0.8f);
        }

        /// <summary>
        /// Allows the screen to perform any initialization logic before starting.
        /// Use it to load any non-graphical content
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent is called once per instance of screen and is used to 
        /// load all of the graphical content
        /// </summary>
        public override bool LoadContent()
        {
            ScreenManager.RemoveScreen("MainScreen");

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

        private void Return(object sender, EventArgs e)
        {
            ScreenManager.PopBackTo(SystemScreen.MenuScreen);
        }
    }
}
