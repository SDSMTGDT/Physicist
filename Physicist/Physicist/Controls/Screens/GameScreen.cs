namespace Physicist.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Physicist.Controls.GUIControls;
    using Physicist.Extensions;

    public partial class GameScreen : IDisposable
    {
        private List<IGUIElement> guiElements = new List<IGUIElement>();
        private Texture2D screenBack;
        private Color backgroundColor;

        public GameScreen(string name)
        {
            this.Name = name;
            this.IsPopup = false;
            this.IsModal = true;
            this.IsActive = true;
            this.BackgroundColor = Color.Transparent;
        }

        public string Name { get; private set; }

        public bool IsActive { get; set; }

        public bool IsPopup { get; set; }
        
        public bool IsModal { get; set; }

        public CameraController Camera { get; private set; }

        public GraphicsDevice GraphicsDevice { get; private set; }

        public Color BackgroundColor 
        {
            get
            {
                return this.backgroundColor;
            }

            set
            {
                this.backgroundColor = value;

                if (value != null)
                {
                    Color[] screenColor = new Color[ScreenManager.GraphicsDevice.Viewport.Width * ScreenManager.GraphicsDevice.Viewport.Height];
                    for (int i = 0; i < ScreenManager.GraphicsDevice.Viewport.Width * ScreenManager.GraphicsDevice.Viewport.Height; i++)
                    {
                        screenColor[i] = this.backgroundColor;
                    }

                    if (this.screenBack != null)
                    {
                        this.screenBack.Dispose();
                    }

                    this.screenBack = new Texture2D(ScreenManager.GraphicsDevice, ScreenManager.GraphicsDevice.Viewport.Width, ScreenManager.GraphicsDevice.Viewport.Height);
                    this.screenBack.SetData(screenColor);
                }
            }
        }

        public ReadOnlyCollection<IGUIElement> GUIElements 
        {
            get
            {
                return new ReadOnlyCollection<IGUIElement>(this.guiElements);
            }
        }

        public bool AddGUIElement(IGUIElement element)
        {
            bool success = false;
            if (element != null && !this.guiElements.Contains(element))
            {
                this.guiElements.Add(element);
                success = true;
            }

            return success;
        }

        public IGUIElement GetElement(string name)
        {
            IGUIElement element = null;
            if (!string.IsNullOrEmpty(name))
            {
                element = this.guiElements.Find(e => { return string.Compare(e.Name, name, StringComparison.CurrentCulture) == 0; });
            }

            return element;
        }

        public void InitializeScreen(GraphicsDevice graphicsDevice) 
        {
            this.GraphicsDevice = graphicsDevice;
            this.Camera = new CameraController();
            this.Camera.CameraViewport = new Viewport(new Size(this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Width));
            this.Camera.Bounds = new Vector2(GraphicsDevice.Viewport.Width * 2, GraphicsDevice.Viewport.Height * 2);
            this.Initialize();
        }

        public virtual void Initialize()
        {
        }

        public bool LoadScreenContent()
        {
            return this.LoadContent() && this.LoadGUI();
        }

        public virtual bool LoadContent() 
        {
            return true;
        }

        public virtual bool LoadGUI()
        {
            return true;
        }

        public void UpdateScreen(GameTime gameTime)
        {
            this.Update(gameTime);
            this.UpdateGUI(gameTime);
        }

        public virtual void Update(GameTime gameTime) 
        {
        }

        public virtual void UpdateGUI(GameTime gameTime)
        {
        }

        public void DrawScreen(FCCSpritebatch sb)
        {
            if (sb != null && this.screenBack != null)
            {
                sb.Draw(this.screenBack, Vector2.Zero, Color.White);
            }

            this.Draw(sb);
            this.DrawGUI(sb);
        }

        public virtual void Draw(FCCSpritebatch sb) 
        {
        }

        public virtual void DrawGUI(FCCSpritebatch sb)
        {
        }

        public void UnloadScreenContent()
        {
            this.UnloadContent();
            this.UnloadGUI();

            if (this.screenBack != null)
            {
                this.screenBack.Dispose();
            }
        }

        public virtual void UnloadContent()
        {
        }

        public virtual void UnloadGUI()
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
                this.UnloadScreenContent();
            }
        }
    }
}
