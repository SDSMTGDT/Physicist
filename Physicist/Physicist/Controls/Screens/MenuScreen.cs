namespace Physicist.Controls
{
    using System;
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

        public override void UnloadContent()
        {
            this.menuBack.Dispose();
            this.menuBack = null;

            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            var state = KeyboardController.GetState();
            if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape, true))
            {
                this.PopScreen();
            }
            else if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter))
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
