namespace Physicist.Controls
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Physicist.Controls.GUIControls;
    using Physicist.Enums;

    public class OptionsScreen : GameScreen
    {
        private Texture2D screenBack;
        
        private Button backButton;

        public OptionsScreen()
            : base(SystemScreen.OptionsScreen.ToString())
        {
            this.IsModal = true;
            this.BackgroundColor = Color.Black;
        }

        public override bool LoadContent()
        {
            Color[] screenColor = new Color[ScreenManager.GraphicsDevice.Viewport.Width * ScreenManager.GraphicsDevice.Viewport.Height];
            for (int i = 0; i < ScreenManager.GraphicsDevice.Viewport.Width * ScreenManager.GraphicsDevice.Viewport.Height; i++)
            {
                screenColor[i] = new Color(0, 0, 0, 0.8f);
            }

            this.screenBack = new Texture2D(ScreenManager.GraphicsDevice, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height);
            this.screenBack.SetData(screenColor);

            this.backButton = new Button(this.GraphicsDevice);
            this.backButton.Bounds = new Rectangle((ScreenManager.GraphicsDevice.Viewport.Width / 2) - 35, this.GraphicsDevice.Viewport.Height - 50, 70, 50);
            this.backButton.BackgroundColor = Color.Gray;
            this.backButton.Text = "Back";
            this.backButton.OnPressed += (s, e) => { this.PopScreen(); };

            return base.LoadContent();
        }

        public override void UnloadContent()
        {
            this.screenBack.Dispose();
            this.screenBack = null;

            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            this.backButton.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(ISpritebatch sb)
        {
            if (sb != null)
            {
                base.Draw(sb);
                sb.Draw(this.screenBack, Vector2.Zero, Color.White);

                this.backButton.Draw(sb);
            }
        }
    }
}
