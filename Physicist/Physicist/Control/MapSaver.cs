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

            XElement mapRoot = new XElement("map");

            mapRoot.Add(SaveMedia());
            mapRoot.Add(SaveLevelObjects());

            document.Add(mapRoot);
            document.Save(stream);
        }

        public static void SaveMap(Map map, string fileName)
        {
            using (Stream stream = File.Create(fileName))
            {
                SaveMap(map, stream);
            }
        }

        private static XElement SaveMedia()
        {
            XElement mediaElements = new XElement("media");

            // TODO: Pull references from 
            IEnumerable<Tuple<Type, string, string>> mediaReferences;
            
            return mediaElements;
        }

        private static XElement SaveLevelObjects()
        {
            XElement levelObjects = new XElement("levelobjects");

            return levelObjects;
        }
    }
}
