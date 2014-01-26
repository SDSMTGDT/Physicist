namespace Physicist.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;
    using Microsoft.Xna.Framework;

    public static class ExtensionMethods
    {
        public static XElement Serialize(this Vector2 vector, string name)
        {
            XElement element = new XElement(name,
                new XAttribute("X", vector.X),
                new XAttribute("Y", vector.Y));

            return element;
        }

        public static void Deserialize(XElement element, out Vector2 vector)
        {
            float x = float.Parse(element.Attribute("X").Value);
            float y = float.Parse(element.Attribute("Y").Value);

            vector = new Vector2(x,y);
        }
    }
}
