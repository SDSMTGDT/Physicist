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
        public Button(GraphicsDevice device)
            : base(device)
        {
        }

        protected override void UpdateButton(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();

            if (this.Bounds.Contains(new Point(mouseState.X, mouseState.Y)))
            {
                if (mouseState.LeftButton == MonoButtonState.Pressed)
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
                else if (mouseState.LeftButton == MonoButtonState.Released)
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

        protected override void DrawButton(ISpritebatch sb)
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

                if (this.LeftBorderTexture != null)
                {
                    sb.Draw(this.LeftBorderTexture, new Vector2(this.Bounds.Left, this.Bounds.Top), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, .21f);
                }

                if (this.TopBorderTexture != null)
                {
                    sb.Draw(this.TopBorderTexture, new Vector2(this.Bounds.Left, this.Bounds.Top), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, .21f); 
                }

                if (this.RightBorderTexture != null)
                {
                    sb.Draw(this.RightBorderTexture, new Vector2(this.Bounds.Right - this.RightBorderTexture.Width, this.Bounds.Top), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, .21f);
                }

                if (this.BottomBorderTexture != null)
                {
                    sb.Draw(this.BottomBorderTexture, new Vector2(this.Bounds.Left, this.Bounds.Bottom - this.BottomBorderTexture.Height), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, .21f);
                }

                var font = ContentController.Instance.GetContent<SpriteFont>("MenuFont");
                if (font != null)
                {
                    var length = font.MeasureString(this.Text);
                    sb.DrawString(font, this.Text, this.Bounds.Center.ToVector() - (length / 2f), this.TextColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.3f);
                }
            }
        }
    }
}
