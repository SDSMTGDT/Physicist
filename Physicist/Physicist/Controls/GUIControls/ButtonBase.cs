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
            this.TextFont = ContentController.Instance.GetContent<SpriteFont>("MenuFont");
        }

        public event EventHandler OnPressed;

        public event EventHandler OnReleased;

        public Texture2D BackgroundImage { get; set; }

        public Color BackgroundColor
        {
            get
            {
                return this.ReleasedBackgroundColor;
            }

            set
            {
                var press = this.TrySet(ref this.pressedBackgroundColor, value);
                var held = this.TrySet(ref this.heldBackgroundColor, value);
                var release = this.TrySet(ref this.releasedBackgroundColor, value);
                var hover = this.TrySet(ref this.hoverBackgroundColor, value);

                if (press || held || release || hover)
                {
                    this.BeginSetBackgroundTexture();
                }
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
                if (this.TrySet(ref this.pressedBackgroundColor, value))
                {
                    this.BeginSetBackgroundTexture();
                }
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
                if (this.TrySet(ref this.hoverBackgroundColor, value))
                {
                    this.BeginSetBackgroundTexture();
                }
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
                if (this.TrySet(ref this.heldBackgroundColor, value))
                {
                    this.BeginSetBackgroundTexture();
                }
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
                if (this.TrySet(ref this.releasedBackgroundColor, value))
                {
                    this.BeginSetBackgroundTexture();
                }
            }
        }

        public string Text { get; set; }

        public Color TextColor { get; set; }

        public SpriteFont TextFont { get; set; }

        public ButtonState State 
        {
            get
            {
                return this.state;
            }

            protected set
            {
                var previous = this.state;
                if (this.TrySetNotify(ref this.state, value))
                {
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

                    this.BeginSetBackgroundTexture();
                }
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
                if (this.TrySetNotify(ref this.bounds, value))
                {
                    // Reset background color to fill new bounds
                    this.BeginSetBackgroundTexture();
                }
            }
        }

        protected Texture2D BackgroundColorTexture { get; set; }

        protected GraphicsDevice Device { get; set; }

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

        protected void BeginSetBackgroundTexture()
        {
            if (this.Device != null)
            {
                this.SetBackgroundTexture();
            }
        }

        protected abstract void SetBackgroundTexture();
    }
}
