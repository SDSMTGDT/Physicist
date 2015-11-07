﻿namespace Physicist.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Physicist.Actors;
    using Physicist.Controls.GUIControls;
    using Physicist.Enums;

    public static class ExtensionMethods
    {
        public static float Angle(this Vector2 vector, Vector2 value)
        {
            return vector.Length() == 0 || value.Length() == 0 ? 0 : (float)Math.Acos(vector.Dot(value) / (vector.Length() * value.Length()));
        }

        public static float Cross(this Vector2 vector, Vector2 value)
        {
            return (vector.X * value.Y) - (vector.Y * value.X); 
        }

        public static float Dot(this Vector2 vector, Vector2 value)
        {
            return (vector.X * value.X) + (vector.Y * value.Y);
        }
        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", Justification = "No new instances are created")]
        public static bool TrySet<T>(this NotifyProperty notify, ref T value, T newValue)
        {
            var change = false;
            if (notify != null)
            {
                if (!value.Equals(newValue))
                {
                    value = newValue;
                    change = true;
                }
            }

            return change;
        }

        public static Color Invert(this Color color)
        {
            return new Color(255 - color.R, 255 - color.G, 255 - color.B, color.A);
        }

        public static Color Brighten(this Color color, float percent)
        {
            percent = percent < 0 ? 0 : percent;
            int r = (int)MathHelper.Clamp(color.R * (1 + (percent / 100f)), 0, 255f);
            int g = (int)MathHelper.Clamp(color.G * (1 + (percent / 100f)), 0, 255f);
            int b = (int)MathHelper.Clamp(color.B * (1 + (percent / 100f)), 0, 255f);

            return new Color(r, g, b, color.A);
        }

        public static Color Darken(this Color color, float percent)
        {
            percent = percent < 0 ? 0 : percent;
            int r = (int)MathHelper.Clamp(color.R * (1 - (percent / 100f)), 0, 255f);
            int g = (int)MathHelper.Clamp(color.G * (1 - (percent / 100f)), 0, 255f);
            int b = (int)MathHelper.Clamp(color.B * (1 - (percent / 100f)), 0, 255f);

            return new Color(r, g, b, color.A);
        }

        public static bool IsAlpha(this Keys key)
        {
            return (int)key >= 65 && (int)key <= 90;
        }

        public static Point ToPoint(this Vector2 vector)
        {
            return new Point((int)vector.X, (int)vector.Y);
        }

        public static Vector2 ToVector(this Point point)
        {
            return new Vector2(point.X, point.Y);
        }

        public static void TryAddNullableAttributeToXml<T>(this XContainer element, string attributeName, T? field) where T : struct
        {            
            if (element != null && !string.IsNullOrEmpty(attributeName) && field.HasValue)
            {
                element.Add(new XAttribute(attributeName, field));
            }
        }

        /// <summary>
        /// Attempts to convert an attribute of an XML element to a given type.
        /// If attribute is not found, will throw an exception.
        /// </summary>
        public static T GetAttribute<T>(this XElement element, string attributeName)
        {
            return element.GetAttribute(attributeName, default(T));
        }

        /// <summary>
        /// Attempts to convert an attribute of an XML element to a given type.
        /// If attribute is not found, or cannot be converted, returns a given default value.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "All exceptions are handled the same")]
        public static T GetAttribute<T>(this XElement element, string attributeName, T defaultValue)
        {
            T value = defaultValue;
            if (element != null)
            {
                try
                {
                    if (element.Attributes(attributeName).Count() > 0)
                    {
                        if (typeof(T).IsEnum)
                        {
                            value = (T)Enum.Parse(typeof(T), element.Attribute(attributeName).Value);
                        }
                        else
                        {
                            value = (T)Convert.ChangeType(element.Attribute(attributeName).Value, typeof(T), CultureInfo.CurrentCulture);
                        }
                    }
                    else
                    {
                        value = defaultValue;
                    }
                }
                catch (InvalidCastException)
                {
                    value = (T)Enum.Parse(typeof(T), element.Attribute(attributeName).Value);
                }
                catch
                {
                    value = defaultValue;
                }              
            }

            return value;
        }

        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action) where T : class
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            foreach (var item in collection)
            {
                action.Invoke(item);
            }            
        }

        public static XElement XmlSerialize(this Size size, string tag)
        {
            return new XElement(
                tag,
                new XAttribute("width", size.Width),
                new XAttribute("height", size.Height));
        }

        public static XElement XmlSerialize(this Vector2 vector, string name)
        {
            return new XElement(
                name,
                new XAttribute("x", vector.X),
                new XAttribute("y", vector.Y));
        }

        public static XElement XmlSerialize(this SpriteAnimation animation, string tag)
        {
            return new XElement(
                tag,
                new XAttribute("rowIndex", animation.RowIndex),
                new XAttribute("frameCount", animation.FrameCount),
                new XAttribute("defaultFrameRate", animation.DefaultFrameRate),
                new XAttribute("playInReverse", animation.PlayInReverse),
                new XAttribute("flipVertical", animation.FlipVertical),
                new XAttribute("flipHorizontal", animation.FlipHorizontal),
                new XAttribute("loopAnimation", animation.LoopAnimation));
        }

        public static SpriteAnimation XmlDeserializeSpriteAnimation(XElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return new SpriteAnimation(
                    element.GetAttribute("rowIndex", (uint)0),
                    element.GetAttribute("frameCount", (uint)1),
                    element.GetAttribute("defaultFrameRate", 0.2f),
                    element.GetAttribute("playInReverse", false),
                    element.GetAttribute("flipVertical", false),
                    element.GetAttribute("flipHorizontal", false),
                    element.GetAttribute("loopAnimation", true),
                    element.GetAttribute("name", element.Name.LocalName));
        }

        public static Size XmlDeserializeSize(XElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return new Size(
                int.Parse(element.Attribute("width").Value, CultureInfo.CurrentCulture), 
                int.Parse(element.Attribute("height").Value, CultureInfo.CurrentCulture));
        }

        public static Vector2 XmlDeserializeVector2(XElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return new Vector2(
                float.Parse(element.Attribute("x").Value, CultureInfo.CurrentCulture), 
                float.Parse(element.Attribute("y").Value, CultureInfo.CurrentCulture));
        }

        public static double Angle(this Vector2 value)
        {
            return Math.Acos(value.X / value.Length());
        }

        public static Vector2 UnitVector(this Vector2 value)
        {
            return value / value.Length();
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
    }
}
