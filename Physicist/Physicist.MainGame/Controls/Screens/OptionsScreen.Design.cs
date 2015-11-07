namespace Physicist.MainGame.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using Physicist.Controls.Screens;
    using Physicist.Controls.GUIControls;
    using Physicist.Types.Enums;
    using Physicist.Types.Util;
    using Physicist.Types.Controllers;
    using Physicist.Types.Xna;

    public partial class OptionsScreen : GameScreen
    {
        private Dictionary<StandardKeyAction, TextBox> keyBindings = new Dictionary<StandardKeyAction, TextBox>();

        /// <summary>
        /// LoadContent is called once per instance of screen and is used to 
        /// load all of the GUI elements 
        /// </summary>
        public override bool LoadGUI()
        {
            this.AddGUIElement(new Label(this) { Bounds = new Rectangle(275, 25, 200, 25), Text = "Set Key Bindings" });

            Button backButton = new Button(this, this.GraphicsDevice) { Bounds = new Rectangle((this.GraphicsDevice.Viewport.Width / 2) - 35, this.GraphicsDevice.Viewport.Height - 50, 70, 50), BackgroundColor = Color.Gray, HoverBackgroundColor = Color.Gray.Brighten(15), Text = "Back" };
            backButton.OnPressed += (s, e) => { this.PopScreen(); };
            this.AddGUIElement(backButton);

            TextBox textBox = new TextBox(this, this.GraphicsDevice) { Bounds = new Rectangle(150, 100, 75, 25), Text = KeyboardController.UpKey.ToString(), CanEdit = false };
            this.AddGUIElement(textBox);
            this.AddGUIElement(new Label(this) { Bounds = new Rectangle(25, 100, 50, 25), Text = "Up Key:" });
            this.keyBindings.Add(StandardKeyAction.Up, textBox);

            TextBox downBox = new TextBox(this, this.GraphicsDevice) { Bounds = new Rectangle(150, 150, 75, 25), Text = KeyboardController.DownKey.ToString(), CanEdit = false };
            this.AddGUIElement(downBox);
            this.AddGUIElement(new Label(this) { Bounds = new Rectangle(25, 150, 50, 25), Text = "Down Key:" });
            this.keyBindings.Add(StandardKeyAction.Down, downBox);

            TextBox leftBox = new TextBox(this, this.GraphicsDevice) { Bounds = new Rectangle(150, 200, 75, 25), Text = KeyboardController.LeftKey.ToString(), CanEdit = false };
            this.AddGUIElement(leftBox);
            this.AddGUIElement(new Label(this) { Bounds = new Rectangle(25, 200, 50, 25), Text = "Left Key:" });
            this.keyBindings.Add(StandardKeyAction.Left, leftBox);

            TextBox rightBox = new TextBox(this, this.GraphicsDevice) { Bounds = new Rectangle(150, 250, 75, 25), Text = KeyboardController.RightKey.ToString(), CanEdit = false };
            this.AddGUIElement(rightBox);
            this.AddGUIElement(new Label(this) { Bounds = new Rectangle(25, 250, 50, 25), Text = "Right Key:" });
            this.keyBindings.Add(StandardKeyAction.Right, rightBox);

            TextBox jumpBox = new TextBox(this, this.GraphicsDevice) { Bounds = new Rectangle(150, 300, 75, 25), Text = KeyboardController.JumpKey.ToString(), CanEdit = false };
            this.AddGUIElement(jumpBox);
            this.AddGUIElement(new Label(this) { Bounds = new Rectangle(25, 300, 50, 25), Text = "Jump Key:" });
            this.keyBindings.Add(StandardKeyAction.Jump, jumpBox);

            TextBox clockBox = new TextBox(this, this.GraphicsDevice) { Bounds = new Rectangle(450, 100, 75, 25), Text = KeyboardController.RotateRightKey.ToString(), CanEdit = false };
            this.AddGUIElement(clockBox);
            this.AddGUIElement(new Label(this) { Bounds = new Rectangle(275, 100, 50, 25), Text = "Rotate CW Key:" });
            this.keyBindings.Add(StandardKeyAction.RotateRight, clockBox);

            TextBox counterBox = new TextBox(this, this.GraphicsDevice) { Bounds = new Rectangle(450, 150, 75, 25), Text = KeyboardController.RotateLeftKey.ToString(), CanEdit = false };
            this.AddGUIElement(counterBox);
            this.AddGUIElement(new Label(this) { Bounds = new Rectangle(275, 150, 50, 25), Text = "Rotate CCW Key:" });
            this.keyBindings.Add(StandardKeyAction.RotateLeft, counterBox);

            TextBox interactBox = new TextBox(this, this.GraphicsDevice) { Bounds = new Rectangle(450, 200, 75, 25), Text = KeyboardController.InteractionKey.ToString(), CanEdit = false };
            this.AddGUIElement(interactBox);
            this.AddGUIElement(new Label(this) { Bounds = new Rectangle(275, 200, 50, 25), Text = "Interact Key:" });
            this.keyBindings.Add(StandardKeyAction.Interact, interactBox);

            return base.LoadGUI();
        }

        /// <summary>
        /// Allows the screen to run updating logic on GUI element like making property changes,
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void UpdateGUI(GameTime gameTime)
        {
            base.UpdateGUI(gameTime);

            var pressedKeys = KeyboardController.GetState().GetPressedKeys();
            foreach (var element in this.GUIElements)
            {
                element.Update(gameTime);
                var textbox = element as TextBox;
                if (pressedKeys.Length > 0 && pressedKeys[0] != Keys.Escape)
                {
                    if (textbox != null && textbox.IsActive)
                    {
                        var index = this.keyBindings.Keys.ToList().FindIndex(k => this.keyBindings[k] == textbox);
                        if (index != -1)
                        {
                            var key = this.keyBindings.ElementAt(index).Key;
                            var value = pressedKeys[0].ToString();
                            if (KeyboardController.TrySetKey(key, (Keys)Enum.Parse(typeof(Keys), value)))
                            {
                                textbox.Text = value;
                                this.keyBindings[key].Text = value;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Called every time the screen is to re-draw itself
        /// </summary>
        /// <param name="sb">SpriteBatch for drawing</param>
        public override void DrawGUI(FCCSpritebatch sb)
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
