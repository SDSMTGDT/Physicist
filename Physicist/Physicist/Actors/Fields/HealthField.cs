using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Physicist.Actors.Fields
{
    class HealthField : IField, Actor
    {
        public Boolean healing;
            //True - heals
            //False - false

        //magnitude is the % of HP healed

        public float hitsPerSecond;

    }
}
