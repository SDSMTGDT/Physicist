namespace Physicist.Actors.Fields
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;

    public abstract class Field : IField
    {
        private Vector2 vector;

        public Field(Vector2 fieldVector)
        {
            this.Vector = fieldVector;
        }

        public Vector2 Vector
        {
            get
            {
                return this.vector;
            }

            set
            {
                this.vector = value;
            }
        }

        public abstract void Draw();

        public abstract void AffectPlayer(Player p);
    }
}
