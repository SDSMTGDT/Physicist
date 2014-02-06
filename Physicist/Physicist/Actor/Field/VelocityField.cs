namespace Physicist.Actors.Fields
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;

    public class VelocityField : Field
    {
        public VelocityField(Vector2 vector)
            : base(vector)
        {
        }

        public override void Draw()
        {
            throw new NotImplementedException();
        }

        public override void AffectPlayer(Player player)
        {
            throw new NotImplementedException();
        }
    }
}
