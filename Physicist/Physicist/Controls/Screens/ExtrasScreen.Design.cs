namespace Physicist.Controls
{
    using Microsoft.Xna.Framework;
    using Physicist.Controls;
    using Physicist.Controls.GUIControls;
    using Physicist.Extensions;

    public partial class ExtrasScreen : GameScreen
    {
        /// <summary>
        /// LoadContent is called once per instance of screen and is used to 
        /// load all of the GUI elements 
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", Justification = "Proper Names")]
        public override bool LoadGUI()
        {
            Button backButton = new Button(this, this.GraphicsDevice) { Bounds = new Rectangle((this.GraphicsDevice.Viewport.Width / 2) - 35, this.GraphicsDevice.Viewport.Height - 50, 70, 50), BackgroundColor = Color.Gray, HoverBackgroundColor = Color.Gray.Brighten(15), Text = "Back" };
            backButton.OnPressed += (s, e) => { this.PopScreen(); };
            this.AddGUIElement(backButton);

            this.AddGUIElement(new Label(this) { Bounds = new Rectangle(275, 50, 100, 25), Text = "SDSMT Game Development Team" });
            this.AddGUIElement(new Label(this) { Bounds = new Rectangle(50, 100, 100, 25), Text = "Design:" });
            this.AddGUIElement(new Label(this) { Bounds = new Rectangle(100, 130, 100, 25), Text = "Aiden Brady" });
            this.AddGUIElement(new Label(this) { Bounds = new Rectangle(100, 160, 100, 25), Text = "James Tillma" });
            this.AddGUIElement(new Label(this) { Bounds = new Rectangle(100, 190, 100, 25), Text = "Evan Doughty" });
            this.AddGUIElement(new Label(this) { Bounds = new Rectangle(50, 230, 100, 25), Text = "Artwork/Media:" });
            this.AddGUIElement(new Label(this) { Bounds = new Rectangle(100, 260, 100, 25), Text = "Erik Hattervig" });
            this.AddGUIElement(new Label(this) { Bounds = new Rectangle(50, 300, 100, 25), Text = "Code Monkeys:" });
            this.AddGUIElement(new Label(this) { Bounds = new Rectangle(100, 330, 100, 25), Text = "Hunter Feltman" });
            this.AddGUIElement(new Label(this) { Bounds = new Rectangle(100, 360, 100, 25), Text = "Derek Stotz" });
            this.AddGUIElement(new Label(this) { Bounds = new Rectangle(100, 390, 100, 25), Text = "Dean Laganiere" });

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
            foreach (var element in this.GUIElements)
            {
                element.UnloadContent();
            }
        }
    }
}
