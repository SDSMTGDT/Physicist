namespace Physicist.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Physicist.Enums;

    public class MenuScreen : GameScreen
    {
        private Texture2D menuBack;

        public MenuScreen() :
            base(SystemScreen.MenuScreen.ToString())
        {
            this.BackgroundColor = Color.Black;            
        }

        public override void LoadContent()
        {
            Color[] menuColor = new Color[ScreenManager.GraphicsDevice.Viewport.Width * ScreenManager.GraphicsDevice.Viewport.Height];
            for (int i = 0; i < ScreenManager.GraphicsDevice.Viewport.Width * ScreenManager.GraphicsDevice.Viewport.Height; i++)
            {
                menuColor[i] = new Color(0, 0, 0, 0.8f);
            }
            
            this.menuBack = new Texture2D(ScreenManager.GraphicsDevice, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height);
            this.menuBack.SetData(menuColor);

            base.LoadContent();
        }

        public override void UnloadContent()
        {
            this.menuBack.Dispose();
            this.menuBack = null;

            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (this.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                this.PopScreen();
            }
            else if (this.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter))
            {
                ScreenManager.AddScreen(MenuScreen.MakePhysicistScreen());
            }

            base.Update(gameTime);
        }

        public override void Draw(ISpritebatch sb)
        {
            if (sb != null)
            {
                base.Draw(sb);
                sb.Draw(this.menuBack, Vector2.Zero, Color.White);
                sb.DrawString(
                                ContentController.Instance.GetContent<SpriteFont>("MenuFont"),
                                "Menu Screen: \n\n Press enter to begin game \n\n Esc to Quit",
                                new Vector2(100, 50),
                                Color.White,
                                0f,
                                Vector2.Zero,
                                1f,
                                SpriteEffects.None,
                                1f);
            }
        }

        private static GameScreen MakePhysicistScreen()
        {
            PhysicistGameScreen screen = null;
            PhysicistGameScreen tempScreen = null;

            try
            {
                tempScreen = new PhysicistGameScreen("MainScreen", "Content\\Levels\\TestLevel.xml");
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
