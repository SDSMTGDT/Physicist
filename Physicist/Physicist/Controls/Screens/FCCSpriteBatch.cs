namespace Physicist.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class FCCSpritebatch : SpriteBatch, ISpritebatch
    {
        private float depthScale = 1f;
        private float lowBound = 0f;

        public FCCSpritebatch(GraphicsDevice graphicsDevice) :
            base(graphicsDevice)
        {
        }

        public void SetBandwidth(float upperBound, float lowerBound)
        {
            if (upperBound > 1f || lowerBound > 1f || upperBound < 0f || lowerBound < 0f)
            {
                throw new ArgumentException("Bounds must be between 0 and 1");
            }

            if (lowerBound > upperBound)
            {
                var temp = lowerBound;
                lowerBound = upperBound;
                upperBound = temp;
            }

            this.depthScale = upperBound - lowerBound;
            this.lowBound = lowerBound;
        }

        public new void Draw(Texture2D texture, Rectangle rectangle, Color color)
        {
            base.Draw(texture, rectangle, null, color, 0f, Vector2.Zero, SpriteEffects.None, this.lowBound);
        }

        public new void Draw(Texture2D texture, Vector2 position, Color color)
        {
            base.Draw(texture, position, null, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, this.lowBound);
        }

        public new void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color)
        {
            base.Draw(texture, destinationRectangle, sourceRectangle, color, 0f, Vector2.Zero, SpriteEffects.None, this.lowBound); 
        }

        public new void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color)
        {
            base.Draw(texture, position, sourceRectangle, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, this.lowBound);
        }

        public new void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effect, float depth)
        {
            base.Draw(texture, destinationRectangle, sourceRectangle, color, rotation, origin, effect, (this.depthScale * depth) + this.lowBound);
        }

        public new void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effect, float depth)
        {
            base.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effect, (this.depthScale * depth) + this.lowBound);
        }

        public new void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effect, float depth)
        {
            base.Draw(texture, position, sourceRectangle, color, rotation, origin, scale, effect, (this.depthScale * depth) + this.lowBound);
        }

        public new void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color)
        {
            base.DrawString(spriteFont, text, position, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, this.lowBound);
        }

        public new void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color)
        {
            base.DrawString(spriteFont, text, position, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, this.lowBound);
        }

        public new void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float depth)
        {
            base.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, (this.depthScale * depth) + this.lowBound);
        }

        public new void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effect, float depth)
        {
            base.DrawString(spriteFont, text, position, color, rotation, origin, scale, effect, (this.depthScale * depth) + this.lowBound);
        }

        public new void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float depth)
        {
            base.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, (this.depthScale * depth) + this.lowBound);
        }

        public new void DrawString(SpriteFont spriteFont, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effect, float depth)
        {
            base.DrawString(spriteFont, text, position, color, rotation, origin, scale, effect, (this.depthScale * depth) + this.lowBound);
        }
    }
}
