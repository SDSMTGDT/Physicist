namespace Physicist.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;
    using FarseerPhysics;
    using FarseerPhysics.Collision;
    using FarseerPhysics.Common;
    using FarseerPhysics.Dynamics;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Physicist.Actors;
    using Physicist.Enums;

    public static class ExtensionMethods
    {
        public static XElement XmlSerialize(this Size size, string tag)
        {
            XElement element = new XElement(
                tag,
                new XAttribute("width", size.Width),
                new XAttribute("height", size.Height));

            return element;
        }

        public static XElement XmlSerialize(this Vector2 vector, string name)
        {
            XElement element = new XElement(
                name,
                new XAttribute("x", vector.X),
                new XAttribute("y", vector.Y));

            return element;
        }

        public static XElement XmlSerialize(this SpriteAnimation animation, string tag)
        {
            XElement animationElement = new XElement(tag);
            animationElement.Add(new XAttribute("rowIndex", animation.RowIndex));
            animationElement.Add(new XAttribute("frameCount", animation.FrameCount));
            animationElement.Add(new XAttribute("defaultFrameRate", animation.DefaultFrameRate));
            animationElement.Add(new XAttribute("playInReverse", animation.PlayInReverse));
            animationElement.Add(new XAttribute("flipVertical", animation.FlipVertical));
            animationElement.Add(new XAttribute("flipHorizontal", animation.FlipHorizontal));

            return animationElement;
        }

        public static SpriteAnimation XmlDeserializeSpriteAnimation(XElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return new SpriteAnimation(
                                       uint.Parse(element.Attribute("rowIndex").Value, CultureInfo.CurrentCulture),
                                       uint.Parse(element.Attribute("frameCount").Value, CultureInfo.CurrentCulture),
                                       float.Parse(element.Attribute("defaultFrameRate").Value, CultureInfo.CurrentCulture),
                                       bool.Parse(element.Attribute("playInReverse").Value),
                                       bool.Parse(element.Attribute("flipVertical").Value),
                                       bool.Parse(element.Attribute("flipHorizontal").Value));
        }

        public static Size XmlDeserializeSize(XElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return new Size(int.Parse(element.Attribute("width").Value, CultureInfo.CurrentCulture), int.Parse(element.Attribute("height").Value, CultureInfo.CurrentCulture));
        }

        public static Vector2 XmlDeserializeVector2(XElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            float x = float.Parse(element.Attribute("x").Value, CultureInfo.CurrentCulture);
            float y = float.Parse(element.Attribute("y").Value, CultureInfo.CurrentCulture);

            return new Vector2(x, y);
        }

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

        public static Texture2D TileTexture(this Texture2D value, Size bounds)
        {
            Texture2D tiledTexture = null;
            if (value != null)
            {
                Color[] textColor = new Color[value.Width * value.Height];
                value.GetData(textColor);

                Color[] tiledTextColor = new Color[bounds.Width * bounds.Height];
                for (int i = 0; i < bounds.Height; i++)
                {
                    for (int j = 0; j < bounds.Width; j++)
                    {
                        tiledTextColor[(i * bounds.Width) + j] = textColor[((i % value.Height) * value.Width) + (j % value.Width)];
                    }
                }

                try
                {
                    value = new Texture2D(MainGame.GraphicsDev, bounds.Width, bounds.Height);
                    value.SetData(tiledTextColor);
                    tiledTexture = value;
                    value = null;
                }
                finally
                {
                    if (value != null)
                    {
                        value.Dispose();
                    }
                }
            }

            return tiledTexture;
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
