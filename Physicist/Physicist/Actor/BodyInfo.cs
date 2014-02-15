namespace Physicist.Actors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using FarseerPhysics.Dynamics;
    using FarseerPhysics.Factories;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public struct BodyInfo
    {
        private Category collidesWith;
        private float width;
        private float height;

        public Category CollidesWith { get; set; }

        public float Width { get; set; }

        public float Height { get; set; }
    }
}
