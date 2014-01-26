namespace Physicist.Controls
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;
    using Microsoft.Xna.Framework;
    using Physicist.Extensions;

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
    }
}
