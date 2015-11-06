﻿namespace Physicist.MainGame.Controls
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using Physicist.Controls.Screens;
    using Physicist.Types.Controllers;
    using Physicist.Types.Enums;
    using Physicist.Types.Interfaces;
    using Physicist.Types.Xna;

    /// <summary>
    /// OptionsScreen is the 'main' screen for this section of
    /// your game. It is used to host content and update components.
    /// </summary>
    public partial class OptionsScreen : GameScreen, ISystemScreen
    {
        private const SystemScreen SystemScreenType = SystemScreen.OptionsScreen;

        public OptionsScreen() :
            base(OptionsScreen.SystemScreenType.ToString())
        {
            this.BackgroundColor = new Color(0, 0, 0, 0.8f);
        }

        public SystemScreen ScreenType
        {
            get { return OptionsScreen.SystemScreenType; }
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
                var ks = KeyboardController.GetState();

                if (ks.IsKeyDown(Keys.Escape, true))
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
    }
}
