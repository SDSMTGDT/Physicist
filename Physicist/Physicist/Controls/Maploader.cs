namespace Physicist.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Media;

    public static class MapLoader
    {
        private static List<string> loadErrors = new List<string>();

        public static bool HasFailed
        {
            get;
            private set;
        }

        public static bool HasErrors
        {
            get;
            private set;
        }

        public static IEnumerable<string> Errors
        {
            get
            {
                return new List<string>(MapLoader.loadErrors);
            }
        }

        public static bool LoadMap(string filePath)
        {
            MapLoader.HasFailed = false;
            MapLoader.HasErrors = false;

            XDocument rootDocument = XDocument.Load(filePath);

            XElement rootElement = rootDocument.Root;
            if (rootElement != null  && (rootElement.Name.ToString() == "map"))
            {
                try
                {
                    MapLoader.LoadMedia(rootElement.Element("media"));
                    MapLoader.LoadLevelObjects(rootElement.Element("levelobjects"));
                }
                catch (AggregateException)
                {
                    MapLoader.HasFailed = true;
                }
            }
            else
            {
                MapLoader.HasFailed = true;
            }

            return MapLoader.HasErrors;
        }

        private static void ErrorOccured(string errorMsg)
        {
            MapLoader.loadErrors.Add(errorMsg);
            MapLoader.HasErrors = true;
        }

        private static void LoadMedia(XElement mediaRoot)
        {
            if (mediaRoot != null)
            {
                MapLoader.LoadContent<Texture2D>(mediaRoot.Elements("texture"), "name", "location");
                MapLoader.LoadContent<SoundEffect>(mediaRoot.Elements("sound"), "name", "location");
                MapLoader.LoadContent<Video>(mediaRoot.Elements("video"), "name", "location");
            }
        }

        private static void LoadContent<T>(IEnumerable<XElement> elements, string idFilter, string pathFilter) where T : class
        {
            if (elements != null)
            {
                foreach (var element in elements)
                {
                    try
                    {
                        ContentController.Instance.LoadContent<T>(element.Attribute(idFilter).Value, element.Attribute(pathFilter).Value);
                    }
                    catch (Microsoft.Xna.Framework.Content.ContentLoadException e)
                    {
                        MapLoader.ErrorOccured("Error while loading " + typeof(T).Name + ": " + element.Attribute(idFilter).Value + ", " + e.Message);
                    }
                }
            }
        }

        private static void LoadLevelObjects(XElement levelRoots)
        {
            if (levelRoots != null)
            {
                XElement backgrounds = levelRoots.Element("backgrounds");
                if (backgrounds != null)
                {
                    MapLoader.CreateInstances(backgrounds.Elements(), "class");
                }

                XElement foregrounds = levelRoots.Element("foregrounds");
                if (foregrounds != null)
                {
                    MapLoader.CreateInstances(foregrounds.Elements(), "class");
                }

                XElement actors = levelRoots.Element("actors");
                if (actors != null)
                {
                    MapLoader.CreateInstances(actors.Elements(), "class");
                }
            }
        }

        private static void CreateInstances(IEnumerable<XElement> elements, string classAttribute)
        {
            foreach (var element in elements)
            {
                string objecttype = element.Name.ToString();

                try
                {
                    Type classType = Type.GetType(element.Attribute(classAttribute).Value);
                    if (!classType.IsValueType && classType.GetConstructor(Type.EmptyTypes) != null)
                    {
                        IXmlSerializable instance = Activator.CreateInstance(classType) as IXmlSerializable;
                        if (instance != null)
                        {
                            instance.XmlDeserialize(element);
                        }
                    }
                    else
                    {
                        throw new TargetInvocationException("Class does not contain parameterless constructor or is a value type", null);
                    }
                }
                catch (NullReferenceException)
                {
                    string nullErrorStr = ", 'class' attribute not found!";
                    if (element.Attribute(classAttribute) != null)
                    {
                        nullErrorStr = " of class: " + element.Attribute(classAttribute).Value + ", Class type not found!";                       
                    }

                    MapLoader.ErrorOccured("Error Error while loading " + objecttype + nullErrorStr);
                }
                catch (Microsoft.Xna.Framework.Content.ContentLoadException e)
                {
                    MapLoader.ErrorOccured("Error while loading " + objecttype + " of class: " + element.Attribute(classAttribute).Value + ", " + e.Message);
                }
            }
        }
    }
}
