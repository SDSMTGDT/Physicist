namespace Physicist.Actors.Fields
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Physicist.Extensions;

    public abstract class Field : IField
    {
        private Vector2 vector;
        private Rectangle size;

        protected Field(Vector2 fieldVector)
        {
            this.Vector = fieldVector;
            this.size = Rectangle.Empty;
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

        public Rectangle Size
        {
            get
            {
                return this.size;
            }

            set
            {
                this.size = value;
            }
        }

        public abstract void Draw();

        public abstract void AffectPlayer(Player player);
    }
}
