namespace Physicist.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class GameScreen : IDisposable
    {
        private Dictionary<Keys, KeyDebouncer> keyPressedStates = new Dictionary<Keys, KeyDebouncer>();

        public GameScreen(string name)
        {
            this.Name = name;
            this.IsPopup = false;
            this.IsModal = true;
            this.IsActive = true;
            this.BackgroundColor = Color.CornflowerBlue;
        }

        public string Name { get; private set; }

        public bool IsActive { get; set; }

        public bool IsPopup { get; set; }
        
        public bool IsModal { get; set; }

        public CameraController Camera { get; private set; }

        public GraphicsDevice GraphicsDevice { get; private set; }

        public Color BackgroundColor { get; set; }

        public virtual void Initialize(GraphicsDevice graphicsDevice) 
        {
            this.GraphicsDevice = graphicsDevice;
            this.Camera = new CameraController();
            this.Camera.CameraViewport = new Controls.Viewport(new Extensions.Size(this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Width));
            this.Camera.Bounds = new Vector2(GraphicsDevice.Viewport.Width * 2, GraphicsDevice.Viewport.Height * 2);
        }
        
        public virtual void LoadContent() 
        { 
        }

        public virtual void NavigatingTo()
        {
            var pressedKeys = Keyboard.GetState().GetPressedKeys();
            foreach (var key in pressedKeys)
            {
                if (!this.keyPressedStates.ContainsKey(key))
                {
                    this.keyPressedStates.Add(key, new KeyDebouncer());
                }

                this.keyPressedStates[key].IsPressed = true;
                this.keyPressedStates[key].PreviousState = KeyState.Up;
            }
        }

        public void UpdateKeys()
        {
            var pressedKeys = Keyboard.GetState().GetPressedKeys();
            foreach (var key in pressedKeys)
            {
                if (!this.keyPressedStates.ContainsKey(key))
                {
                    this.keyPressedStates.Add(key, new KeyDebouncer() { IsPressed = true, PreviousState = KeyState.Up });
                }
                else
                {
                    if (this.keyPressedStates[key].PreviousState == KeyState.Up)
                    {
                        this.keyPressedStates[key].IsPressed = false;
                    }
                    else
                    {
                        this.keyPressedStates[key].IsPressed = true;
                        this.keyPressedStates[key].PreviousState = KeyState.Up;
                    }
                }
            }

            foreach (var key in this.keyPressedStates)
            {
                if (!pressedKeys.Contains(key.Key))
                {
                    this.keyPressedStates[key.Key].IsPressed = false;
                    this.keyPressedStates[key.Key].PreviousState = KeyState.Down;
                }
            }
        }

        public virtual void Update(GameTime gameTime) 
        {
        }
        
        public virtual void Draw(ISpritebatch sb) 
        { 
        }
        
        public virtual void UnloadContent() 
        {             
        }

        public virtual bool IsKeyDown(Keys key)
        {
            if (!this.keyPressedStates.ContainsKey(key))
            {
                this.keyPressedStates.Add(key, new KeyDebouncer() { IsPressed = false, PreviousState = KeyState.Down });
            }

            return this.keyPressedStates[key].IsPressed;
        }

        public void PopScreen()
        {
            ScreenManager.RemoveScreen(this.Name);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.UnloadContent();
            }
        }
    }
}
