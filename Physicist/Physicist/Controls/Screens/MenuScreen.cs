namespace Physicist.Controls
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Physicist.Controls.GUIControls;
    using Physicist.Enums;

    public class MenuScreen : GameScreen
    {
        private Texture2D menuBack;

        private Button playButton;
        private Button optionsButton;
        private Button extrasButton;

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

            this.playButton = new Button(this.GraphicsDevice);
            this.playButton.Bounds = new Rectangle((ScreenManager.GraphicsDevice.Viewport.Width / 2) - 155, this.GraphicsDevice.Viewport.Height - 130, 310, 50);
            this.playButton.BackgroundColor = Color.Goldenrod;
            this.playButton.Text = "Play Game";
            this.playButton.OnPressed += this.StartPhysicist;

            this.optionsButton = new Button(this.GraphicsDevice);
            this.optionsButton.Bounds = new Rectangle((ScreenManager.GraphicsDevice.Viewport.Width / 2) - 155, this.GraphicsDevice.Viewport.Height - 75, 153, 50);
            this.optionsButton.BackgroundColor = Color.Goldenrod;
            this.optionsButton.Text = "Options";
            this.optionsButton.OnPressed += this.StartOptions;

            this.extrasButton = new Button(this.GraphicsDevice);
            this.extrasButton.Bounds = new Rectangle((ScreenManager.GraphicsDevice.Viewport.Width / 2) + 3, this.GraphicsDevice.Viewport.Height - 75, 153, 50);
            this.extrasButton.BackgroundColor = Color.Goldenrod;
            this.extrasButton.Text = "Extras";
            this.extrasButton.OnPressed += this.StartExtras;

            return base.LoadContent();
        }

        public override void UnloadContent()
        {
            this.menuBack.Dispose();
            this.menuBack = null;

            base.UnloadContent();
        }

        public void StartExtras(object sender, EventArgs e)
        {
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

        public override void Update(GameTime gameTime)
        {
            this.playButton.Update(gameTime);
            this.optionsButton.Update(gameTime);
            this.extrasButton.Update(gameTime);

            var state = KeyboardController.GetState();
            if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape, true))
            {
                this.PopScreen();
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
                                "Menu Screen: \n\n Esc to Quit",
                                new Vector2(100, 50),
                                Color.White,
                                0f,
                                Vector2.Zero,
                                1f,
                                SpriteEffects.None,
                                1f);

                this.playButton.Draw(sb);
                this.optionsButton.Draw(sb);
                this.extrasButton.Draw(sb);
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
