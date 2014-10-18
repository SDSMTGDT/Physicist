namespace Physicist.Controls
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Physicist.Enums;

    public class PauseScreen : GameScreen
    {
        private Texture2D menuBack;

        public PauseScreen() :
            base(SystemScreen.PauseScreen.ToString())
        {
            this.IsPopup = true;
            this.BackgroundColor = Color.TransparentBlack;
        }

        public override bool LoadContent()
        {
            Color[] menuColor = new Color[ScreenManager.GraphicsDevice.Viewport.Width * ScreenManager.GraphicsDevice.Viewport.Height];
            for (int i = 0; i < ScreenManager.GraphicsDevice.Viewport.Width * ScreenManager.GraphicsDevice.Viewport.Height; i++)
            {
                menuColor[i] = new Color(0, 0, 0, 0.8f);
            }

            this.menuBack = new Texture2D(ScreenManager.GraphicsDevice, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height);
            this.menuBack.SetData(menuColor);

            return base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var state = KeyboardController.GetState();

            if (state.IsKeyDown(Keys.Escape, true))
            {
                this.PopScreen();
            }
        }

        public override void Draw(ISpritebatch sb)
        {
            if (sb != null)
            {
                base.Draw(sb);
                sb.Draw(this.menuBack, Vector2.Zero, Color.White);
                sb.DrawString(
                                ContentController.Instance.GetContent<SpriteFont>("MenuFont"),
                                "PAUSED",
                                new Vector2(100, 50),
                                Color.White,
                                0f,
                                Vector2.Zero,
                                5f,
                                SpriteEffects.None,
                                1f);
            }
        }
    }
}
