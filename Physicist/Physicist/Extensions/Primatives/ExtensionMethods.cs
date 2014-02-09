namespace Physicist.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;
    using FarseerPhysics;
    using FarseerPhysics.Common;
    using Microsoft.Xna.Framework;
    using Physicist.Actors;

    public static class ExtensionMethods
    {
        public static XElement XmlSerialize(this Size size, string tag)
        {
            XElement element = new XElement(
                tag,
                new XAttribute("Width", size.Width),
                new XAttribute("Height", size.Height));

            return element;
        }

        public static XElement XmlSerialize(this Vector2 vector, string name)
        {
            XElement element = new XElement(
                name,
                new XAttribute("X", vector.X),
                new XAttribute("Y", vector.Y));

            return element;
        }

        public static XElement XmlSerialize(this SpriteAnimation animation, string tag)
        {
            XElement animationElement = new XElement(tag);
            animationElement.Add(new XAttribute("struct", typeof(SpriteAnimation).ToString()));

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

            return new Size(int.Parse(element.Attribute("Width").Value, CultureInfo.CurrentCulture), int.Parse(element.Attribute("Height").Value, CultureInfo.CurrentCulture));
        }

        public static Vector2 XmlDeserializeVector2(XElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            float x = float.Parse(element.Attribute("X").Value, CultureInfo.CurrentCulture);
            float y = float.Parse(element.Attribute("Y").Value, CultureInfo.CurrentCulture);

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
    }
}
