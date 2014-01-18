namespace Physicist.Actors.Fields
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;

    public class HealthField : Field
    {
        public HealthField(Vector2 vector, float hitsPerSecond)
            : base(vector)
        {
            this.HitsPerSecond = hitsPerSecond;
        }

        // Vector2 notes:
        // magnitude of Vector2 is the % of HP healed
        // direction of Vector2 : positive - heals, negative - damages
        public float HitsPerSecond { get; set; }

        public override void Draw()
        {
        }

        public override void AffectPlayer(Player player)
        {
        }
    }
}
