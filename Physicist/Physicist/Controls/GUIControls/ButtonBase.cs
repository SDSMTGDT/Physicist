namespace Physicist.Controls.GUIControls
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Physicist.Enums;
    using Physicist.Extensions;

    public abstract class ButtonBase : GUIElement
    {
        private ButtonState state = ButtonState.Released;
        private Rectangle bounds = new Rectangle();
        
        private Color releasedBackgroundColor = Color.Transparent;
        private Color hoverBackgroundColor = Color.Transparent;
        private Color heldBackgroundColor = Color.Transparent;
        private Color pressedBackgroundColor = Color.Transparent;

        protected ButtonBase(object parent, GraphicsDevice device)
            : base(parent)
        {
            this.Device = device;
            this.TextColor = Color.Black;
            this.Visibility = Visibility.Visible;
            this.IsEnabled = true;
            this.TextFont = ContentController.Instance.GetContent<SpriteFont>("MenuFont");
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
                this.SetBackgroundTexture();
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
                this.SetBackgroundTexture();
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
                this.SetBackgroundTexture();
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
                this.SetBackgroundTexture();
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
                this.SetBackgroundTexture();
            }
        }

        public string Text { get; set; }

        public Color TextColor { get; set; }

        public SpriteFont TextFont { get; set; }

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

                this.SetBackgroundTexture();
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

                // Reset background color to fill new bounds
                this.SetBackgroundTexture();
            }
        }

        protected Texture2D BackgroundColorTexture { get; set; }

        protected GraphicsDevice Device { get; set; }

        public override void Draw(ISpritebatch sb)
        {
            if (this.Visibility == Visibility.Visible)
            {
                this.DrawButton(sb);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (this.IsEnabled)
            {
                this.UpdateButton(gameTime);
            }
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            if (this.BackgroundColorTexture != null)
            {
                this.BackgroundColorTexture.Dispose();
                this.BackgroundColorTexture = null;
            }

            if (this.Device != null)
            {
                this.Device = null;
            }

            base.UnloadContent();
        }

        protected abstract void UpdateButton(GameTime gameTime);

        protected abstract void DrawButton(ISpritebatch sb);

        protected abstract void SetBackgroundTexture();
    }
}
