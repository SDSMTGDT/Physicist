namespace Physicist.Controls.GUIControls
{
    using System;
    using Microsoft.Xna.Framework;
    using Physicist.Types.Enums;
    using Physicist.Types.Interfaces;

    public class GUIElement : NotifyProperty, IGUIElement
    {
        public GUIElement(object parent)
        {
            this.Parent = parent;
            this.Name = string.Empty;
            this.IsEnabled = true;
            this.Visibility = Visibility.Visible;
        }

        public object Parent { get; set; }

        public string Name { get; set; }

        public bool IsEnabled { get; set; }

        public Visibility Visibility { get; set; }

        public void Update(GameTime gameTime)
        {
            if (gameTime != null && this.IsEnabled)
            {
                this.UpdateElement(gameTime);
            }
        }

        public void Draw(ISpritebatch sb)
        {
            if (sb != null && this.Visibility == Visibility.Visible)
            {
                this.DrawElement(sb);
            }
        }

        public virtual void UnloadContent()
        {
        }

        protected virtual void UpdateElement(GameTime gameTime)
        {
        }

        protected virtual void DrawElement(ISpritebatch sb)
        {
        }
    }
}
