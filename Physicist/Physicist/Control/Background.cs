using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Physicist.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Physicist.Controls
{
    public class Backdrop
    {
        public Vector2 Location { get; set; }
        public Size Dimensions { get; set; }
        public float Depth { get; set; }
        public Texture2D Texture { get; set; }

        public Backdrop(Vector2 location, Size dimensions, float depth, string textureReference, Texture2D texture)
        {
            this.Location = location;
            this.Dimensions = dimensions;
            this.Depth = depth;
            this.Texture = texture;
        }
    }

    public class BackgroundMusic
    {
        public Vector2 Location {get; set;}
        private Size Dimensions {get; set;}
        private SoundEffect soundEffect {get; set;}

        public BackgroundMusic(Vector2 location, Size dimensions, SoundEffect soundEffect)
        {
            this.Location = location;
            this.Dimensions = dimensions;
            this.soundEffect = soundEffect;
        }

        // TODO: Implement background video
    }

    public class Background
    {
        private List<Backdrop> backdrops;
        private List<BackgroundMusic> backgroundMusic;


    }
}
