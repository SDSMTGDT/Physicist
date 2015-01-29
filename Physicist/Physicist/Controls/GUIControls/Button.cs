namespace Physicist.Controls.GUIControls
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Physicist.Extensions;

    using MonoButtonState = Microsoft.Xna.Framework.Input.ButtonState;

    public class Button : ButtonBase
    {
        public Button(object parent, GraphicsDevice device)
            : base(parent, device)
        {
            this.Border = new RectangleBorder(device);
        }

        public new Rectangle Bounds
        {
            get
            {
                return base.Bounds;
            }

            set
            {
                base.Bounds = value;
                this.Border.Bounds = value;
            }
        }

        public IBorder<Rectangle> Border { get; protected set; }

        public Color BorderColor
        {
            get
            {
                return this.Border.BorderColor;
            }

            set
            {
                this.Border.BorderColor = value;
            }
        }

        public int BorderSize
        {
            get
            {
                return this.Border.BorderSize;
            }

            set
            {
                this.Border.BorderSize = value;
            }
        }

        public override void UnloadContent()
        {
            this.Border.UnloadContent();
            base.UnloadContent();
        }

        protected override void UpdateElement(GameTime gameTime)
        {
            var mouseState = MouseController.GetState();

            if (this.Bounds.Contains(new Point(mouseState.X, mouseState.Y)))
            {
                if (mouseState.LeftButtonDebounce == MonoButtonState.Pressed)
                {
                    if (this.State == ButtonState.Released || this.State == ButtonState.Hover)
                    {
                        this.State = ButtonState.Pressed;
                    }
                    else if (this.State == ButtonState.Pressed)
                    {
                        this.State = ButtonState.Held;
                    }
                }
                else if (mouseState.LeftButtonDebounce == MonoButtonState.Released)
                {
                    if (this.State == ButtonState.Pressed || this.State == ButtonState.Held)
                    {
                        this.State = ButtonState.Released;
                    }
                    else if (this.State == ButtonState.Released)
                    {
                        this.State = ButtonState.Hover;
                    }
                }
            }
            else
            {
                this.State = ButtonState.Released;
            }
        }

        protected override void DrawElement(ISpritebatch sb)
        {
            if (sb != null)
            {
                if (this.BackgroundImage != null)
                {
                    sb.Draw(this.BackgroundImage, this.Bounds, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, .8f);
                }

                if (this.BackgroundColorTexture != null)
                {
                    sb.Draw(this.BackgroundColorTexture, this.Bounds, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.2f);
                }

                if (this.Border.BorderTexture != null)
                {
                    sb.Draw(this.Border.BorderTexture, this.Border.Bounds, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.25f);
                }

                if (this.TextFont != null)
                {
                    var length = this.TextFont.MeasureString(this.Text);
                    sb.DrawString(this.TextFont, this.Text, this.Bounds.Center.ToVector() - (length / 2f), this.TextColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.3f);
                }
            }
        }

        protected override void SetBackgroundTexture()
        {
            if (this.BackgroundColorTexture != null)
            {
                this.BackgroundColorTexture.Dispose();
            }

            this.BackgroundColorTexture = new Texture2D(this.Device, this.Bounds.Width, this.Bounds.Height);

            Color[] data = new Color[this.Bounds.Width * this.Bounds.Height];
            Color color = Color.Transparent;
            switch (this.State)
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
