using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Physicist.Actors.Fields
{
    internal class HealthField : Actor, IField
    {
        public Boolean healing;
        public float Direction; //angle in radians
        public uint Magnitude; //magnitude of the affecting vector
            //True - heals
            //False - false

        //magnitude is the % of HP healed

        public float hitsPerSecond;

        public HealthField(float directionIn = 0, uint magnitudeIn = 0, float hitsPerSecondIn = 0, bool healingIn = false)
        {
            this.Direction = directionIn;
            this.Magnitude = magnitudeIn;
            this.healing = healingIn;
            this.hitsPerSecond = hitsPerSecondIn;
        }

        void IField.Draw()
        {
            throw new NotImplementedException();
        }

        void IField.AffectPlayer(Player p)
        {
            throw new NotImplementedException();
        }
    }
}
