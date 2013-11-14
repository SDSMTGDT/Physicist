namespace Physicist.Actors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Actor
    {
        public Actor()
        {
            this.Sprites = new Dictionary<string, GameSprite>();

            this.Position = Vector2.Zero;
            this.Velocity = Vector2.Zero;
            this.Acceleration = Vector2.Zero;
            this.VisibleState = Visibility.Visible;
            this.IsEnabled = true;
            this.Rotation = 0f;
            this.Health = 1;
        }

        // 2space variables
        public Vector2 Position { get; set; }

        public Vector2 Velocity { get; set; }
        
        public Vector2 Acceleration { get; set; }
        
        public float Rotation { get; set; }

        // gameplay state variables
        public int Health { get; set; }
        
        public bool IsEnabled { get; set; }
        
        public bool IsDead 
        {
            get
            {
                return this.Health <= 0;
            }
        }

        // draw properties
        public Dictionary<string, GameSprite> Sprites { get; set; }

        public Visibility VisibleState { get; set; }

        public virtual void Draw(SpriteBatch sb)
        {
            if (this.VisibleState == Visibility.Visible)
            {
                foreach (var sprite in this.Sprites.Values)
                {
                    sb.Draw(
                        sprite.SpriteSheet,
                        new Vector2(this.Position.X + sprite.Offset.X, this.Position.Y + sprite.Offset.Y),
                        sprite.CurrentSprite,
                        Color.White,
                        this.Rotation,
                        Vector2.Zero,
                        1f,
                        SpriteEffects.None,
                        sprite.Depth);
                }
            }
        }

        public virtual void AddSprite(string name, GameSprite sprite)
        {
            if (sprite == null)
            {
                throw new ArgumentNullException("Sprite cannot be null");
            }

            this.Sprites.Add(name, sprite);
        }

        public virtual void Update(GameTime time)
        {
            // update every sprite in the sprite collection
            if (this.IsEnabled)
            {
                foreach (var sprite in this.Sprites.Values)
                {
                    sprite.Update(time);
                }
            }
        }
    }
}
