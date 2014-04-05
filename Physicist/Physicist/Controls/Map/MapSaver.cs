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
    using Physicist.Actors;
    using Physicist.Extensions;
    using Physicist.Extensions.Primitives;

    public static class MapSaver
    {
        public static void SaveMap(Map map, IEnumerable<Actor> actors, Stream stream)
        {
            XDocument document = new XDocument();

            XElement mapRoot = new XElement("map");

            mapRoot.Add(SaveMedia());
            mapRoot.Add(SaveLevelObjects(map, actors));

            document.Add(mapRoot);
            document.Save(stream);
        }

        public static void SaveMap(Map map, IEnumerable<Actor> actors, string fileName)
        {
            using (Stream stream = File.Create(fileName))
            {
                SaveMap(map, actors, stream);
            }
        }

        private static XElement SaveMedia()
        {
            XElement mediaElements = new XElement("media");

            foreach (IMediaInfo info in ContentController.Instance.MediaReferences)
            {
                XElement mediaElement = new XElement(
                    info.Format.ToString(),
                    new XAttribute("name", info.Name),
                    new XAttribute("location", info.Location));

                mediaElements.Add(mediaElement);
            }

            return mediaElements;
        }

        private static XElement SaveLevelObjects(Map map, IEnumerable<Actor> actors)
        {
            XElement levelObjects = new XElement("levelobjects");

            // Save background objects
            XElement backgrounds = new XElement("backgrounds");

            foreach (IXmlSerializable item in map.BackgroundObjects)
            {
                backgrounds.Add(item.XmlSerialize());
            }

            levelObjects.Add(backgrounds);

            // Save foreground objects
            XElement foregrounds = new XElement("foregrounds");

            foreach (IXmlSerializable mapObject in map.MapObjects)
            {
                foregrounds.Add(mapObject.XmlSerialize());
            }

            levelObjects.Add(foregrounds);

            // Save actors
            XElement actorElements = new XElement("actors");

            foreach (IXmlSerializable actor in actors)
            {
                actorElements.Add(actor.XmlSerialize());
            }

            levelObjects.Add(actorElements);

            return levelObjects;
        }
    }
}
