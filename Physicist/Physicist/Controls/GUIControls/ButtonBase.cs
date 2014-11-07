namespace Physicist.Controls.GUIControls
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Physicist.Enums;
    using Physicist.Extensions;

    public abstract class ButtonBase : IUpdate, IDraw
    {
        private ButtonState state = ButtonState.Released;
        private Rectangle bounds = new Rectangle();
        private int borderSize = 1;
        private GraphicsDevice device = null;
        
        private Color releasedBackgroundColor = new Color(Color.Transparent, 0);
        private Color hoverBackgroundColor = new Color(Color.Transparent, 0);
        private Color heldBackgroundColor = new Color(Color.Transparent, 0);
        private Color pressedBackgroundColor = new Color(Color.Transparent, 0);

        private Color leftBorderColor = new Color(Color.Transparent, 0);
        private Color topBorderColor = new Color(Color.Transparent, 0);
        private Color rightBorderColor = new Color(Color.Transparent, 0);
        private Color bottomBorderColor = new Color(Color.Transparent, 0);

        protected ButtonBase(GraphicsDevice device)
        {
            this.device = device;
            this.TextColor = Color.Black;
            this.Visibility = Visibility.Visible;
            this.IsEnabled = true;
        }

        public event EventHandler OnPressed;

        public event EventHandler OnReleased;

        public bool IsEnabled { get; set; }

        public Texture2D BackgroundImage { get; set; }

        public Color BackgroundColor
        {
            get
            {
                return this.ReleasedBackgroundColor;
            }

            set
            {
                this.pressedBackgroundColor = value;
                this.heldBackgroundColor = value;
                this.releasedBackgroundColor = value;
                this.hoverBackgroundColor = value;
                this.SetBackgroundColor();
            }
        }

        public Color PressedBackgroundColor 
        {
            get
            {
                return this.pressedBackgroundColor;
            }

            set
            {
                this.pressedBackgroundColor = value;
                this.SetBackgroundColor();
            }
        }

        public Color HoverBackgroundColor
        {
            get
            {
                return this.hoverBackgroundColor;
            }

            set
            {
                this.hoverBackgroundColor = value;
                this.SetBackgroundColor();
            }
        }

        public Color HeldBackgroundColor
        {
            get
            {
                return this.heldBackgroundColor;
            }

            set
            {
                this.heldBackgroundColor = value;
                this.SetBackgroundColor();
            }
        }

        public Color ReleasedBackgroundColor
        {
            get
            {
                return this.releasedBackgroundColor;
            }

            set
            {
                this.releasedBackgroundColor = value;
                this.SetBackgroundColor();
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
                this.SetBorderColor();
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
                this.SetBorderColor();
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
                this.SetBorderColor();
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
                this.SetBorderColor();
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
                this.SetBorderColor();
            }
        }

        public string Text { get; set; }

        public Color TextColor { get; set; }

        public Visibility Visibility { get; set; }

        public ButtonState State 
        {
            get
            {
                return this.state;
            }

            protected set
            {
                var previous = this.state;
                this.state = value;
                if (this.state == ButtonState.Pressed && (previous == ButtonState.Released || previous == ButtonState.Hover))
                {
                    if (this.OnPressed != null)
                    {
                        this.OnPressed(this, EventArgs.Empty);
                    }
                }
                else if (this.state == ButtonState.Released && (previous == ButtonState.Pressed || previous == ButtonState.Held))
                {
                    if (this.OnReleased != null)
                    {
                        this.OnReleased(this, EventArgs.Empty);
                    }
                }

                this.SetBackgroundColor();
                this.SetBorderColor();
            }
        }

        public Rectangle Bounds 
        {
            get
            {
                return this.bounds;
            }

            set
            {
                this.bounds = value;

                if (this.BackgroundColorTexture != null)
                {
                    this.BackgroundColorTexture.Dispose();
                }

                this.BackgroundColorTexture = new Texture2D(this.device, this.Bounds.Width, this.Bounds.Height);

                // Reset background color to fill new bounds
                this.SetBackgroundColor();
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

                if (this.LeftBorderTexture != null)
                {
                    this.LeftBorderTexture.Dispose();
                }

                if (this.TopBorderTexture != null)
                {
                    this.TopBorderTexture.Dispose();
                }

                if (this.RightBorderTexture != null)
                {
                    this.RightBorderTexture.Dispose();
                }

                if (this.BottomBorderTexture != null)
                {
                    this.BottomBorderTexture.Dispose();
                }

                this.LeftBorderTexture = new Texture2D(this.device, this.borderSize, this.Bounds.Height);
                this.TopBorderTexture = new Texture2D(this.device, this.Bounds.Width, this.borderSize);
                this.RightBorderTexture = new Texture2D(this.device, this.borderSize, this.Bounds.Height);
                this.BottomBorderTexture = new Texture2D(this.device, this.Bounds.Width, this.borderSize);

                // Reset background color to fill new bounds
                this.SetBorderColor();
            }
        }

        protected Texture2D LeftBorderTexture { get; private set; }

        protected Texture2D TopBorderTexture { get; private set; }

        protected Texture2D RightBorderTexture { get; private set; }

        protected Texture2D BottomBorderTexture { get; private set; }

        protected Texture2D BackgroundColorTexture { get; private set; }

        public void Draw(ISpritebatch sb)
        {
            if (this.Visibility == Visibility.Visible)
            {
                this.DrawButton(sb);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (this.IsEnabled)
            {
                this.UpdateButton(gameTime);
            }
        }

        protected abstract void UpdateButton(GameTime gameTime);

        protected abstract void DrawButton(ISpritebatch sb);

        private void SetBorderColor()
        {
            Color[] heightData = new Color[this.Bounds.Height * this.BorderSize];
            Color[] widthData = new Color[this.Bounds.Width * this.BorderSize];

            if (this.LeftBorderTexture != null)
            {
                for (int i = 0; i < this.Bounds.Height * this.BorderSize; i++)
                {
                    heightData[i] = this.LeftBorderColor;
                }

                this.LeftBorderTexture.SetData(heightData);
            }

            if (this.RightBorderTexture != null)
            {
                for (int i = 0; i < this.Bounds.Height * this.BorderSize; i++)
                {
                    heightData[i] = this.RightBorderColor;
                }

                this.RightBorderTexture.SetData(heightData);
            }

            if (this.TopBorderTexture != null)
            {
                for (int i = 0; i < this.Bounds.Width * this.BorderSize; i++)
                {
                    widthData[i] = this.TopBorderColor;
                }

                this.TopBorderTexture.SetData(heightData);
            }

            if (this.BottomBorderTexture != null)
            {
                for (int i = 0; i < this.Bounds.Width * this.BorderSize; i++)
                {
                    widthData[i] = this.BottomBorderColor;
                }

                this.BottomBorderTexture.SetData(heightData);
            }
        }

        private void SetBackgroundColor()
        {
            Color[] data = new Color[this.Bounds.Width * this.Bounds.Height];
            Color color = Color.Transparent;
            switch (this.state)
            {
                case ButtonState.Pressed:
                    color = this.PressedBackgroundColor;
                    break;

                case ButtonState.Held:
                    color = this.HeldBackgroundColor;
                    break;

                case ButtonState.Released:
                    color = this.ReleasedBackgroundColor;
                    break;

                case ButtonState.Hover:
                    color = this.HoverBackgroundColor;
                    break;
            }

            if (this.BackgroundColorTexture != null)
            {
                for (int i = 0; i < this.Bounds.Width * this.Bounds.Height; i++)
                {
                    data[i] = color;
                }

                this.BackgroundColorTexture.SetData(data);
            }
        }
    }
}
