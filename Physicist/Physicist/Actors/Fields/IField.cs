using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Physicist.Actors.Fields
{
    internal interface IField
    {

        //sprite is a back color, translucent - > to be used with Actor

        //overloading
        void Draw();
        void AffectPlayer(Player p);

    }
}
