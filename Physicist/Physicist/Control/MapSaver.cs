namespace Physicist.Controls
{
    using Microsoft.Xna.Framework;
    using Physicist.Extensions;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;



    public static class MapSaver
    {
        public static void SaveMap(Map map, Stream stream)
        {
            XDocument document = new XDocument();

            XElement mapRoot = new XElement("Map");
            



            // Media

            // Level Objects
            XElement levelObjects = new XElement("LevelObects");


            document.Add(mapRoot);
            document.Save(stream);
        }
        public static void SaveMap(Map map, string filename)
        {
            using(Stream stream = File.Create(filename))
            {
                SaveMap(map, stream);
            }
        }

        private static XElement CreateXElement(Backdrop backdrop)
        {
            XElement element = new XElement("Backdrop",
                CreateXElement("Location", backdrop.Location),
                CreateXElement("Dimenstions", backdrop.Dimensions),
                new XElement("Depth", backdrop.Depth),
                new XElement("TextureRef", backdrop.Texture.Name));

            return element;
        }

        private static XElement CreateXElement(string name, Vector2 vector)
        {
            XElement element = new XElement(name,
                new XAttribute("X", vector.X),
                new XAttribute("Y", vector.Y));

            return element;
        }
    }
}
