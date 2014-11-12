namespace Physicist.Controls.GUIControls
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Physicist.Enums;

    public class Label : GUIElement
    {
        private SpriteFont font;

        public Label(object parent)
            : base(parent)
        {
            this.font = ContentController.Instance.GetContent<SpriteFont>("MenuFont");
            this.Text = string.Empty;
            this.TextColor = Color.White;
            this.Visibility = Visibility.Visible;
        }

        public string Text { get; set; }

        public Color TextColor { get; set; }

        public Rectangle Bounds { get; set; }

        public Visibility Visibility { get; set; }

        public override void Draw(ISpritebatch sb)
        {
            if (sb != null && this.Visibility == Visibility.Visible)
            {
                sb.DrawString(this.font, this.Text, new Vector2(this.Bounds.X, this.Bounds.Center.Y - (this.font.MeasureString(this.Text).Y / 2f)), this.TextColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, .1f);
            }
        }
    }
}
