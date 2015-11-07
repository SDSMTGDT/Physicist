namespace Physicist.Controls.GUIControls
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Physicist.Types.Controllers;

    public class TextBase : GUIElement
    {
        private string text = string.Empty;
        private SpriteFont font = ContentController.Instance.GetContent<SpriteFont>("MenuFont");
        private float scale = 1f;

        public TextBase(object parent, string text)
            : base(parent)
        {
            this.text = text;
            this.TextColor = Color.Black;
            this.UpdateTextDimensions();
        }

        public event EventHandler TextChanged;

        public string Text
        {
            get
            {
                return this.text;
            }

            set
            {
                if (this.TrySetNotify(ref this.text, value))
                {
                    this.TextWidth = this.Font.MeasureString(this.Text).X * this.scale;
                    this.TextHeight = this.Font.MeasureString(this.Text).Y * this.scale;
                    if (this.TextChanged != null)
                    {
                        this.TextChanged(this, null);
                    }
                }
            }
        }

        public SpriteFont Font 
        {
            get
            {
                return this.font;
            }

            set
            {
                if (this.TrySetNotify(ref this.font, value))
                {
                    this.UpdateTextDimensions();
                }
            }
        }

        public float TextScale 
        { 
            get
            {
                return this.scale;
            }

            set
            {
                if (this.TrySetNotify(ref this.scale, value < 0 ? 0 : value))
                {
                    this.UpdateTextDimensions();
                }
            }
        }

        public Color TextColor { get; set; }

        public float TextWidth { get; private set; }

        public float TextHeight { get; private set; }

        public float MaxCharWidth { get; private set; }

        public float MinCharWidth { get; private set; }

        public override string ToString()
        {
            return this.Text;
        }

        public float WidthToIndex(int index)
        {
            return this.Font.MeasureString(this.Text.Substring(0, index)).X;
        }

        public string SubstringToWidth(int width)
        {
            var value = this.Text;
            if (this.TextWidth > width)
            {
                for (int i = this.Text.Length - 1; i >= 0; i--)
                {
                    var temp = this.Text.Substring(0, i);
                    if (this.Font.MeasureString(temp).X < width)
                    {
                        value = temp;
                        break;
                    }
                }
            }

            return value;
        }

        private void UpdateTextDimensions()
        {
            this.TextWidth = this.Font.MeasureString(this.Text).X * this.scale;
            this.TextHeight = this.Font.MeasureString(this.Text).Y * this.scale;
            this.MaxCharWidth = this.Font.MeasureString("W").X * this.scale;
            this.MinCharWidth = this.Font.MeasureString(".").X * this.scale;
        }
    }
}
