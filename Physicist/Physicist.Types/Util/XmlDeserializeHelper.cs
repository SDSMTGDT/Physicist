namespace Physicist.Types.Util
{
    using System;
    using System.Globalization;
    using System.Xml.Linq;
    using Microsoft.Xna.Framework;
    using Physicist.Types.Common;
    using System.Collections.Generic;

    public static class XmlDeserializeHelper
    {
        private delegate object Deserializer(XElement element);

        private static Dictionary<Type, Deserializer> DeserailizerMap = new Dictionary<Type, Deserializer>()
        {
            {typeof(SpriteAnimation), XmlDeserializeHelper.XmlDeserializeSpriteAnimation},
            {typeof(Size), XmlDeserializeHelper.XmlDeserializeSize},
            {typeof(Vector2), XmlDeserializeHelper.XmlDeserializeVector2}
        };


        public static T XmlDeserialize<T>(XElement element)
        {
            var type = typeof(T);
            if(XmlDeserializeHelper.DeserailizerMap.ContainsKey(type))
            {
                return (T)XmlDeserializeHelper.DeserailizerMap[type](element);
            }
            else
            {
                throw new InvalidOperationException("Deserialization method for type: " + type.ToString() + " not found");
            }

        }

        private static object XmlDeserializeSpriteAnimation(XElement element)
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

        private static object XmlDeserializeSize(XElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return new Size(
                int.Parse(element.Attribute("width").Value, CultureInfo.CurrentCulture),
                int.Parse(element.Attribute("height").Value, CultureInfo.CurrentCulture));
        }

        private static object XmlDeserializeVector2(XElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return new Vector2(
                float.Parse(element.Attribute("x").Value, CultureInfo.CurrentCulture),
                float.Parse(element.Attribute("y").Value, CultureInfo.CurrentCulture));
        }

    }
}
