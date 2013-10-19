using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Physicist.Actors.Fields
{
    internal class AccelerationField : Actor, IField
    {
        public float dampenRate; //the rate at which to dampen player velocity when they enter the field. Negative number instantly removes all velocity
        public float Direction; //angle in radians
        public uint Magnitude; //magnitude of the affecting vector

        public void Draw()
        {
            throw new NotImplementedException();
        }

        public void AffectPlayer(Player p)
        {
            throw new NotImplementedException();
        }

        public AccelerationField(float directionIn = 0, uint magnitudeIn = 0, float dampenRateIn = 0)
        {
            this.Direction = directionIn;
            this.Magnitude = magnitudeIn;
            this.dampenRate = dampenRateIn;
        }
    }
}
