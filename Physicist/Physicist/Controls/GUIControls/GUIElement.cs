namespace Physicist.Controls.GUIControls
{
    using System;
    using Microsoft.Xna.Framework;

    public class GUIElement : NotifyProperty, IGUIElement
    {
        public GUIElement(object parent)
        {
            this.Parent = parent;
            this.Name = string.Empty;
        }

        public object Parent { get; set; }

        public string Name { get; set; }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(ISpritebatch sb)
        {
        }

        public virtual void UnloadContent()
        {
        }
    }
}
