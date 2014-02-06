namespace Physicist.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;
    using Microsoft.Xna.Framework;

    public static class ExtensionMethods
    {
        public static XElement Serialize(this Vector2 vector, string name)
        {
            XElement element = new XElement(
                name,
                new XAttribute("X", vector.X),
                new XAttribute("Y", vector.Y));

            return element;
        }

        public static Vector2 DeserializeVector2(XElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            float x = float.Parse(element.Attribute("X").ToString(), CultureInfo.CurrentCulture);
            float y = float.Parse(element.Attribute("Y").ToString(), CultureInfo.CurrentCulture);

            return new Vector2(x, y);
        }
    }
}
