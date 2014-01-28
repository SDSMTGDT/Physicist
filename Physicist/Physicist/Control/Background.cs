namespace Physicist.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Graphics;
    using Physicist.Extensions;

    public class Background
    {
        private List<Backdrop> backdrops = new List<Backdrop>();
        private List<BackgroundMusic> backgroundMusic = new List<BackgroundMusic>();

        public IEnumerable<Backdrop> Backdrops
        {
            get
            {
                return this.backdrops;
            }
        }

        public IEnumerable<BackgroundMusic> BackgroundMusic
        {
            get
            {
                return this.backgroundMusic;
            }
        }

        // TODO: Implement background video
    }
}
