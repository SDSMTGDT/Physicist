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
            this.Position = new Vector2();
            this.Velocity = new Vector2();
            this.Acceleration = new Vector2();
            this.Rotation = 0;

            this.Health = 1;
            this.IsEnabled = true;
            this.Sprites = new Dictionary<string, SpriteSegment>();
            this.VisibleState = Visibility.Visible;
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
                return this.Health == 0;
            }
        }

        // draw properties
        public Dictionary<string, SpriteSegment> Sprites { get; set; }

        public Visibility VisibleState { get; set; }

        public virtual void Draw(SpriteBatch sb)
        {
            foreach (var item in this.Sprites)
            {
                sb.Draw(item.Value.Sprite.SpriteSheet, new Vector2(this.Position.X + item.Value.Offset.X, this.Position.Y + item.Value.Offset.Y), item.Value.Sprite.CurrentSprite, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0);
            }
        }

        public virtual void AddSprite(string name, GameSprite sprite, Vector2 offset)
        {
            this.Sprites.Add(name, new SpriteSegment(sprite, offset));
        }

        public virtual void AddSprite(string name, GameSprite sprite)
        {
            this.Sprites.Add(name, new SpriteSegment(sprite, new Vector2(0, 0)));
        }

        public virtual void Update(GameTime time)
        {
            // update every sprite in the sprite collection
            foreach (var item in this.Sprites)
            {
                item.Value.Sprite.Update(time);
            }
        }

        // A class linking a GameSprite with a local offset
        public class SpriteSegment
        {
            public SpriteSegment(GameSprite sprite, Vector2 offset)
            {
                this.Sprite = sprite;
                this.Offset = offset;
            }

            // The GameSprite and Vector2
            public GameSprite Sprite { get; set; }

            public Vector2 Offset { get; set; }
        }
    }
}
