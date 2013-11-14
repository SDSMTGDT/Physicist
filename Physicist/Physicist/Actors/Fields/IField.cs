namespace Physicist.Actors.Fields
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;

    public interface IField
    {
        // sprite is a back color, translucent - > to be used with Actor
        Vector2 Vector { get; set; }

        Rectangle Size { get; set; }

        // overloading
        void Draw();

        void AffectPlayer(Player p);
    }
}
