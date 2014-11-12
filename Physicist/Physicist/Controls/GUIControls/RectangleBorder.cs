namespace Physicist.Controls.GUIControls
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class RectangleBorder : Border<Rectangle>
    {
        public RectangleBorder(GraphicsDevice device)
            : base(device)
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Texture Tracked by base class")]
        protected override void SetBorderTexture()
        {
            if (this.BorderTexture != null)
            {
                this.BorderTexture.Dispose();
            }

            this.BorderTexture = new Texture2D(this.Device, this.Bounds.Width, this.Bounds.Height);

            if (this.BorderTexture != null)
            {
                Color[] data = new Color[this.Bounds.Width * this.Bounds.Height];

                for (int j = 0; j < this.BorderSize; j++)
                {
                    for (int i = 0 + j; i < this.Bounds.Width - j; i++)
                    {
                        data[i + (this.Bounds.Width * j)] = this.TopBorderColor;
                        data[i + (this.Bounds.Width * (this.Bounds.Height - j - 1))] = this.BottomBorderColor;
                    }
                }

                for (int i = 0; i < this.BorderSize; i++)
                {
                    for (int j = 0 + i; j < this.Bounds.Height - i; j++)
                    {
                        data[i + (this.Bounds.Width * j)] = this.LeftBorderColor;
                        data[(this.Bounds.Width * (j + 1)) - i - 1] = this.RightBorderColor;
                    }
                }

                this.BorderTexture.SetData(data);
            }
        }
    }
}
