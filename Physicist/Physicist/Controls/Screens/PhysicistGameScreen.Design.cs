namespace Physicist.Controls
{
    using Microsoft.Xna.Framework;
    using Physicist.Controls;
    using Physicist.Controls.GUIControls;

    public partial class PhysicistGameScreen : GameScreen
    {
        /// <summary>
        /// LoadContent is called once per instance of screen and is used to 
        /// load all of the GUI elements 
        /// </summary>
        public override bool LoadGUI()
        {
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
            base.UnloadGUI();
        }
    }
}
