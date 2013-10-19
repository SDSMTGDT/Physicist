using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Physicist.Actors
{
    abstract class Actor
    {
        //coordinates in the map
        float x;
        float y;

        float angle; //for rotation

        public Actor(float xin = 0, float yin = 0, float anglein = 0)
        {
            this.x = xin;
            this.y = yin;
            this.angle = anglein;
        }
    }
}
