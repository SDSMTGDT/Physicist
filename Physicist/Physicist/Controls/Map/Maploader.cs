namespace Physicist.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Media;
    using Physicist.Actors;
    using Physicist.Enums;
    using Physicist.Events;
    using Physicist.Extensions;

    public static class MapLoader
    {
        private static List<string> loadErrors = new List<string>();
        private static Dictionary<string, Type> assemblyTypes = new Dictionary<string, Type>();
        private static Dictionary<string, Type> quantifiedAssemblyTypes = new Dictionary<string, Type>();

        private static XElement rootElement;

        static MapLoader()
        {
            int typeCount = 0;
            int assemblyCount = 0;            
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                typeCount = 0;
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

                    typeCount++;
                }

                assemblyCount++;
            }
        }

        // !!!!!!!!EXTREMELY DISLIKE OF THIS AT THE MOMENT!!!!!!!!!!!!!!! //
        public static Map CurrentMap { get; set; }

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

        public static Map InitializeLoader(string filePath)
        {
            MapLoader.HasFailed = false;
            MapLoader.HasErrors = false;
            MapLoader.loadErrors.Clear();

            XDocument rootDocument = XDocument.Load(filePath);
            MapLoader.rootElement = rootDocument.Root;
            try
            {
                MapLoader.CurrentMap = new Map(
                        int.Parse(rootElement.Attribute("width").Value, CultureInfo.CurrentCulture),
                        int.Parse(rootElement.Attribute("height").Value, CultureInfo.CurrentCulture));
            }
            catch (AggregateException)
            {
                MapLoader.HasFailed = true;
            }

            return MapLoader.CurrentMap;
        }

        public static bool LoadCurrentMap()
        {
            if (!MapLoader.HasFailed)
            {
                try
                {
                    MapLoader.LoadMedia(rootElement.Element("Media"));
                    MapLoader.LoadLevelObjects(rootElement.Element("LevelObjects"));
                    MapLoader.LoadEvents(rootElement.Element("MapEvents"));
                }
                catch (AggregateException)
                {
                    MapLoader.HasFailed = true;
                }
            }

            return !MapLoader.HasFailed;
        }

        public static IEnumerable<object> CreateInstances(IEnumerable<XElement> elements, string classAttribute)
        {
            List<object> instances = new List<object>();
            if (elements != null)
            {
                foreach (var element in elements)
                {
                    instances.Add(MapLoader.CreateInstance(element, classAttribute));
                }
            }

            return instances;
        }

        public static object CreateInstance(XElement element, string classAttribute)
        {
            object instance = null;
            if (element != null)
            {
                string objecttype = element.Name.ToString();
                string className = null;

                try
                {
                    if (classAttribute == null)
                    {
                        className = objecttype;
                    }
                    else
                    {
                        className = element.Attribute(classAttribute).Value;
                    }

                    // Try to create type from fully quantified name in current assembly
                    Type classType = Type.GetType(className);

                    // If failure, try to find type in registered assemblies, first by short name, then by full name
                    if (classType == null && !MapLoader.assemblyTypes.TryGetValue(className, out classType))
                    {
                        classType = MapLoader.quantifiedAssemblyTypes[className];
                    }

                    if (classType.GetConstructor(new Type[] { typeof(XElement) }) != null)
                    {
                        instance = Activator.CreateInstance(classType, new object[] { element });
                    }
                    else
                    {
                        MapLoader.ErrorOccured("Error Error while loading " + objecttype + " of class: " + className + ", Class does not contain a constructor accepting a single XElement");
                    }
                }
                catch (KeyNotFoundException)
                {
                    MapLoader.ErrorOccured("Error Error while loading " + objecttype + " of class: " + className + ", Class type not found!");
                }
                catch (NullReferenceException)
                {
                    MapLoader.ErrorOccured("Error Error while loading " + objecttype + ", 'class' attribute not found!");
                }
                catch (Microsoft.Xna.Framework.Content.ContentLoadException e)
                {
                    MapLoader.ErrorOccured("Error while loading " + objecttype + " of class: " + className + ", " + e.Message);
                }
                catch (TargetInvocationException e)
                {
                    MapLoader.ErrorOccured("Error while loading " + objecttype + " of class: " + className + ", " + e.Message);
                }
            }

            return instance;
        }

        private static void LoadEvents(XElement eventRoot)
        {
            MapLoader.CreateInstances(eventRoot.Elements("Event"), "class").ForEach(eventObj => MapLoader.CurrentMap.AddObjectToMap(eventObj));
        }

        private static void ErrorOccured(string errorMsg)
        {
            MapLoader.loadErrors.Add(errorMsg);
            System.Console.WriteLine(errorMsg);
            MapLoader.HasErrors = true;
        }

        private static void LoadMedia(XElement mediaRoot)
        {
            if (mediaRoot != null)
            {
                foreach (var mediatype in Enum.GetNames(typeof(MediaFormat)))
                {                    
                    Type classType = null;
                    if (MapLoader.assemblyTypes.TryGetValue(mediatype, out classType))
                    {
                        // Invoke LoadContent with anonymous generic type
                        typeof(MapLoader).GetMethod("LoadContent", BindingFlags.NonPublic | BindingFlags.Static)
                                         .MakeGenericMethod(new Type[] { classType })
                                         .Invoke(null, new object[] { mediaRoot.Elements(mediatype), "name", "location" });
                    }
                }
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
                XElement backgroundsEle = levelRoots.Element("Backgrounds");
                if (backgroundsEle != null)
                {                    
                    MapLoader.CreateInstances(backgroundsEle.Elements(), "class").ForEach(backgroundObj => MapLoader.CurrentMap.AddObjectToMap(backgroundObj));
                }

                XElement foregroundsEle = levelRoots.Element("Foregrounds");
                if (foregroundsEle != null)
                {
                    MapLoader.CreateInstances(foregroundsEle.Elements(), "class").ForEach(foregroundObj => MapLoader.CurrentMap.AddObjectToMap(foregroundObj));
                }

                XElement actorsEle = levelRoots.Element("Actors");
                if (actorsEle != null)
                {
                    MapLoader.CreateInstances(actorsEle.Elements(), "class").ForEach(actor => MapLoader.CurrentMap.AddObjectToMap(actor));
                }
            }
        }
    }
}
