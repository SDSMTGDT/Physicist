using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Physicist.Actors.Fields
{
    internal class VelocityField : Actor, IField 
    {
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

        public VelocityField(float directionIn = 0, uint magnitudeIn = 0)
        {
            this.Direction = directionIn;
            this.Magnitude = magnitudeIn;
        }
    }
}
