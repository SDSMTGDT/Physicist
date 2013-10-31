namespace Physicist.Actors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public abstract class Actor
    {
        // coordinates in the map
        private float x;
        private float y;

        private float angle; // for rotation

        public Actor(float xin = 0, float yin = 0, float anglein = 0)
        {
            this.x = xin;
            this.y = yin;
            this.angle = anglein;
        }
    }
}
