﻿namespace Physicist.Actors.Fields
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;

    public class AccelerationField : Field
    {
        public AccelerationField(Vector2 vector, float velocityDampenRate = 0)
            : base(vector)
        {
            this.VelocityDampenRate = velocityDampenRate;
        }

        public float VelocityDampenRate { get; set; } // the rate at which to dampen player velocity when they enter the field. Negative number instantly removes all velocity

        public override void Draw()
        {
            throw new NotImplementedException();
        }

        public override void AffectPlayer(Player p)
        {
            throw new NotImplementedException();
        }
    }
}
