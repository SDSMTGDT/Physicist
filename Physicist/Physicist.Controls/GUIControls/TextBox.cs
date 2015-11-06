namespace Physicist.Controls.GUIControls
{
    using System;
    using System.Globalization;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Physicist.Types.Controllers;
    using Physicist.Types.Interfaces;
    using Physicist.Types.Enums;
    using Physicist.Types.Util;
    using MonoButtonState = Microsoft.Xna.Framework.Input.ButtonState;

    public class TextBox : TextBase
    {
        private GraphicsDevice device;
        private Color backgroundColor = Color.White;
        private int cursorPos = 0;
        private Rectangle bounds = new Rectangle();

        private float blinkRate = 1f;
        private float elapsedTime = 0;
        private float cursorScale = 1f;

        public TextBox(object parent, GraphicsDevice device)
            : base(parent, string.Empty)
        {
            this.device = device;
            this.CanEdit = true;
            this.ShowCursor = true;
            this.CursorTexture = new Texture2D(this.device, 1, 1);
            this.CursorTexture.SetData(new Color[1] { Color.Black });
        }

        public Color BackgroundColor 
        {
            get
            {
                return this.backgroundColor;
            }

            set
            {
                if (this.TrySet(ref this.backgroundColor, value))
                {
                    this.UpdateTextures();
                }
            }
        }

        public bool CanEdit { get; set; }

        public bool IsActive { get; set; }

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
                    if (this.BackgroundColorTexture != null)
                    {
                        this.BackgroundColorTexture.Dispose();
                    }

                    this.BackgroundColorTexture = new Texture2D(this.device, this.Bounds.Width, this.Bounds.Height);
                    this.cursorScale = this.Bounds.Height - 2f;

                    this.UpdateTextures();
                }
            }
        }

        private Texture2D BackgroundColorTexture { get; set; }

        private Texture2D CursorTexture { get; set; }

        private bool ShowCursor { get; set; }

        public override void UnloadContent()
        {
            if (this.CursorTexture != null)
            {
                this.CursorTexture.Dispose();
            }

            if (this.BackgroundColorTexture != null)
            {
                this.BackgroundColorTexture.Dispose();
            }

            if (this.device != null)
            {
                this.device = null;
            }

            base.UnloadContent();
        }

        protected override void DrawElement(ISpritebatch sb)
        {
            if (sb != null && this.IsEnabled && this.Visibility == Visibility.Visible)
            {
                sb.Draw(this.BackgroundColorTexture, this.Bounds, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.2f);

                if (this.Font != null)
                {
                    var cropped = this.SubstringToWidth(this.Bounds.Width);
                    sb.DrawString(this.Font, cropped, new Vector2(this.Bounds.X + 2, this.Bounds.Center.Y - (this.TextHeight / 2)), this.TextColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.3f);
                }

                if (this.CanEdit && this.IsActive && this.ShowCursor)
                {
                    sb.Draw(this.CursorTexture, new Vector2(this.Bounds.X + this.WidthToIndex(this.cursorPos) + 2, this.Bounds.Y), null, Color.White, 0f, Vector2.Zero, new Vector2(1f, this.cursorScale), SpriteEffects.None, 0.4f);
                }
            }
        }

        protected override void UpdateElement(GameTime gameTime)
        {
            if (this.IsEnabled)
            {
                var mouseState = MouseController.GetState();
                var mousePoint = new Point(mouseState.X, mouseState.Y);

                if (gameTime != null)
                {
                    this.elapsedTime += gameTime.ElapsedGameTime.Milliseconds / 1000f;

                    if (this.elapsedTime > this.blinkRate)
                    {
                        this.elapsedTime = 0;
                        this.ShowCursor = !this.ShowCursor;
                    }
                }

                if (mouseState.LeftButtonDebounce == MonoButtonState.Pressed)
                {
                    if (this.Bounds.Contains(mousePoint))
                    {
                        this.IsActive = true;
                        this.cursorPos = Math.Min(this.Text.Length == 0 ? 0 : (int)Math.Floor((mousePoint.X - this.Bounds.Left) / (this.TextWidth / this.Text.Length)), this.Text.Length);
                    }
                    else
                    {
                        this.IsActive = false;
                    }
                }

                if (this.IsActive && this.CanEdit)
                {
                    var ks = KeyboardController.GetState();
                    var keys = ks.GetPressedKeys(true);
                    if (keys.Length > 0)
                    {
                        if (keys[0].IsAlpha() && this.cursorPos < (int)(this.Bounds.X / this.MaxCharWidth))
                        {
                            this.Text += ks.IsKeyDown(Keys.LeftShift) || ks.IsKeyDown(Keys.RightShift) ? keys[0].ToString() : keys[0].ToString().ToLower(CultureInfo.CurrentCulture);
                            this.cursorPos++;
                        }
                        else if (keys[0] == Keys.Back && this.cursorPos > 0)
                        {
                            this.Text = this.Text.Remove(this.cursorPos - 1, 1);
                            this.cursorPos = Math.Max(this.cursorPos - 1, 0);
                        }
                        else if (keys[0] == Keys.Delete && this.cursorPos < this.Text.Length)
                        {
                            this.Text = this.Text.Remove(this.cursorPos, 1);
                        }
                    }
                }
            }
        }

        private void UpdateTextures()
        {
            if (this.BackgroundColorTexture != null)
            {
                Color[] data = new Color[this.Bounds.Width * this.Bounds.Height];

                for (int i = 0; i < this.Bounds.Width * this.Bounds.Height; i++)
                {
                    data[i] = this.BackgroundColor;
                }

                this.BackgroundColorTexture.SetData(data);
                this.CursorTexture.SetData(new Color[1] { this.BackgroundColor.Invert() });
            }
        }
    }
}
