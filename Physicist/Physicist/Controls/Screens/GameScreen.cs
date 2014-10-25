namespace Physicist.Controls
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Physicist.Extensions;

    public class GameScreen : IDisposable
    {
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
            this.Camera.CameraViewport = new Viewport(new Size(this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Width));
            this.Camera.Bounds = new Vector2(GraphicsDevice.Viewport.Width * 2, GraphicsDevice.Viewport.Height * 2);
        }
        
        public virtual bool LoadContent() 
        {
            return true;
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
