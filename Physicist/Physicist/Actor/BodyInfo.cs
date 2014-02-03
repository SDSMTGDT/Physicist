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
        public Category CollidesWith;
        public float Width;
        public float Height;
    }
}
