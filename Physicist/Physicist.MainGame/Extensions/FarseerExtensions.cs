namespace Physicist.MainGame.Extensions
{
    using Microsoft.Xna.Framework;
    using FarseerPhysics;
    using FarseerPhysics.Collision;
    using FarseerPhysics.Common;
    using FarseerPhysics.Dynamics;
    using Physicist.Types.Enums;

    public static class FarseerExtensions
    {
        public static Vector2 ToSimUnits(this Vector2 value)
        {
            return ConvertUnits.ToSimUnits(value);
        }

        public static float ToSimUnits(this float value)
        {
            return ConvertUnits.ToSimUnits(value);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "Follows Farseer pattern")]
        public static Vertices ToSimUnits(this Vertices value)
        {
            Vertices convertVerts = new Vertices();
            if (value != null)
            {
                foreach (var vert in value)
                {
                    convertVerts.Add(vert.ToSimUnits());
                }
            }

            return convertVerts;
        }

        public static Vector2 ToDisplayUnits(this Vector2 value)
        {
            return ConvertUnits.ToDisplayUnits(value);
        }

        public static Vector2 FixtureOffset(this Fixture value, BodyCategory category, Vector2 position, Vector2 shapeOffset)
        {
            Vector2 offset = Vector2.Zero;
            if (value != null)
            {
                Transform bodyTrans;
                value.Body.GetTransform(out bodyTrans);

                AABB bounds;
                value.Shape.ComputeAABB(out bounds, ref bodyTrans, 0);

                switch (category)
                {
                    case BodyCategory.ChainShape:
                    case BodyCategory.CompoundPolygon:
                    case BodyCategory.Gear:
                    case BodyCategory.Polygon:
                    case BodyCategory.RoundedRectangle:
                    case BodyCategory.SolidArc:
                        offset = bounds.LowerBound.ToDisplayUnits() - position + shapeOffset;
                        break;

                    case BodyCategory.LineArc:
                        offset = new Vector2(-2.5f, 0); // Slight offset for generated arc textures
                        break;
                }
            }

            return offset;
        }
    }
}
