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
    using Physicist.Extensions;

    public static class MapLoader
    {
        private static List<string> loadErrors = new List<string>();
        private static Dictionary<string, Type> assemblyTypes = new Dictionary<string, Type>();
        private static Dictionary<string, Type> quantifiedAssemblyTypes = new Dictionary<string, Type>();

        static MapLoader()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (!MapLoader.assemblyTypes.ContainsKey(type.Name))
                    {
                        MapLoader.assemblyTypes.Add(type.Name, type);
                    }

                    if (!MapLoader.quantifiedAssemblyTypes.ContainsKey(type.FullName))
                    {
                        MapLoader.quantifiedAssemblyTypes.Add(type.FullName, type);
                    }
                }
            }
        }

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
                    // Try to create type from fully quantified name in current assembly
                    Type classType = Type.GetType(element.Attribute(classAttribute).Value);

                    // If failure, try to find type in registered assemblies, first by short name, then by full name
                    if (classType == null && !MapLoader.assemblyTypes.TryGetValue(element.Attribute(classAttribute).Value, out classType))
                    {
                        classType = MapLoader.quantifiedAssemblyTypes[element.Attribute(classAttribute).Value];
                    }

                    if (!classType.IsValueType && classType.GetConstructor(Type.EmptyTypes) != null)
                    {
                        Activator.CreateInstance(classType, new object[] { element });
                    }
                    else
                    {
                         MapLoader.ErrorOccured("Error Error while loading " + objecttype + " of class: " + element.Attribute(classAttribute).Value + ", Class does not contain parameterless constructor or is a value type");
                    }
                }
                catch (KeyNotFoundException)
                {
                    MapLoader.ErrorOccured("Error Error while loading " + objecttype + " of class: " + element.Attribute(classAttribute).Value + ", Class type not found!");
                }
                catch (NullReferenceException)
                {
                    MapLoader.ErrorOccured("Error Error while loading " + objecttype + ", 'class' attribute not found!");
                }
                catch (Microsoft.Xna.Framework.Content.ContentLoadException e)
                {
                    MapLoader.ErrorOccured("Error while loading " + objecttype + " of class: " + element.Attribute(classAttribute).Value + ", " + e.Message);
                }
            }
        }
    }
}
