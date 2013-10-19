using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Physicist.Actors.Fields
{
    interface IField
    {
        public float Direction; //angle in radians
        public uint Magnitude; //magnitude of the affecting vector

        //sprite is a back color, translucent - > to be used with Actor

        //overloading
        public void Draw();
        public void AffectPlayer(Player p);

    }
}
