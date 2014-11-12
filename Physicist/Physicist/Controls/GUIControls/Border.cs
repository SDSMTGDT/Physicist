namespace Physicist.Controls.GUIControls
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public abstract class Border<T> : IBorder<T>
    {
        private Color leftBorderColor = Color.Transparent;
        private Color topBorderColor = Color.Transparent;
        private Color rightBorderColor = Color.Transparent;
        private Color bottomBorderColor = Color.Transparent;
        private int borderSize = 1;
        private T bounds;

        protected Border(GraphicsDevice device)            
        {
            this.Device = device;
        }

        public T Bounds 
        {
            get
            {
                return this.bounds;
            }

            set
            {
                this.bounds = value;
                this.SetBorderTexture();
            }
        }

        public Color BorderColor
        {
            get
            {
                return this.LeftBorderColor;
            }

            set
            {
                this.leftBorderColor = value;
                this.topBorderColor = value;
                this.rightBorderColor = value;
                this.bottomBorderColor = value;
                this.SetBorderTexture();
            }
        }

        public Color LeftBorderColor
        {
            get
            {
                return this.leftBorderColor;
            }

            set
            {
                this.leftBorderColor = value;
                this.SetBorderTexture();
            }
        }

        public Color TopBorderColor
        {
            get
            {
                return this.topBorderColor;
            }

            set
            {
                this.topBorderColor = value;
                this.SetBorderTexture();
            }
        }

        public Color RightBorderColor
        {
            get
            {
                return this.rightBorderColor;
            }

            set
            {
                this.rightBorderColor = value;
                this.SetBorderTexture();
            }
        }

        public Color BottomBorderColor
        {
            get
            {
                return this.bottomBorderColor;
            }

            set
            {
                this.bottomBorderColor = value;
                this.SetBorderTexture();
            }
        }

        public int BorderSize
        {
            get
            {
                return this.borderSize;
            }

            set
            {
                this.borderSize = value;
                this.SetBorderTexture();
            }
        }

        public Texture2D BorderTexture { get; protected set; }

        protected GraphicsDevice Device { get; set; }

        public virtual void UnloadContent()
        {
            if (this.BorderTexture != null)
            {
                this.BorderTexture.Dispose();
                this.BorderTexture = null;
            }

            if (this.Device != null)
            {
                this.Device = null;
            }
        }

        protected abstract void SetBorderTexture();
    }
}
