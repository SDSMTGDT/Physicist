namespace Physicist.Controls.GUIControls
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Physicist.Enums;

    public class Label : GUIElement
    {
        public Label(object parent)
            : base(parent)
        {
            this.Font = ContentController.Instance.GetContent<SpriteFont>("MenuFont");
            this.Text = string.Empty;
            this.TextColor = Color.White;
            this.Visibility = Visibility.Visible;
        }

        public string Text { get; set; }

        public Color TextColor { get; set; }

        public SpriteFont Font { get; set; }

        public Rectangle Bounds { get; set; }

        protected override void DrawElement(ISpritebatch sb)
        {
            if (sb != null)
            {
                sb.DrawString(this.Font, this.Text, new Vector2(this.Bounds.X, this.Bounds.Center.Y - (this.Font.MeasureString(this.Text).Y / 2f)), this.TextColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, .1f);
            }
        }
    }
}
