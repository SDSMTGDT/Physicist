namespace Physicist.Controls
{
    using Microsoft.Xna.Framework;
    using Physicist.Controls;
    using Physicist.Controls.GUIControls;

    public partial class PauseScreen : GameScreen
    {
        /// <summary>
        /// LoadContent is called once per instance of screen and is used to 
        /// load all of the GUI elements 
        /// </summary>
        public override bool LoadGUI()
        {
            this.AddGUIElement(new Label(this) { Bounds = new Rectangle(100, 50, 100, 25), Text = "PAUSED", TextColor = Color.White });
            this.AddGUIElement(new Label(this) { Bounds = new Rectangle(100, 100, 100, 25), Text = "Esc to return to game", TextColor = Color.White });

            return base.LoadGUI();
        }

        /// <summary>
        /// Allows the screen to run updating logic on GUI element like making property changes,
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void UpdateGUI(GameTime gameTime)
        {
            base.UpdateGUI(gameTime);
            foreach (var element in this.GUIElements)
            {
                element.Update(gameTime);
            }
        }

        /// <summary>
        /// Called every time the screen is to re-draw itself
        /// </summary>
        /// <param name="sb">SpriteBatch for drawing</param>
        public override void DrawGUI(ISpritebatch sb)
        {
            base.DrawGUI(sb);
            foreach (var element in this.GUIElements)
            {
                element.Draw(sb);
            }
        }

        /// <summary>
        /// UnloadContent is called once per instance of screen and is used to 
        /// unload all of the GUI elements
        /// </summary>
        public override void UnloadGUI()
        {
            foreach (var element in this.GUIElements)
            {
                element.UnloadContent();
            }

            base.UnloadGUI();
        }
    }
}
