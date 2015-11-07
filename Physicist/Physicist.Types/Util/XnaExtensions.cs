namespace Physicist.Types.Util
{
    using System;
    using System.Xml.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Graphics;
    using Common;

    public static class XnaExtensions
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

        public static double Angle(this Vector2 value)
        {
            return Math.Acos(value.X / value.Length());
        }

        public static Vector2 UnitVector(this Vector2 value)
        {
            return value / value.Length();
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

        public static XElement XmlSerialize(this Vector2 vector, string name)
        {
            return new XElement(
                name,
                new XAttribute("x", vector.X),
                new XAttribute("y", vector.Y));
        }

        public static Texture2D TileTexture(this Texture2D value, Size bounds, GraphicsDevice graphicsDev)
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
                    value = new Texture2D(graphicsDev, bounds.Width, bounds.Height);
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
