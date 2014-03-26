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
    using Physicist.Extensions;

    public static class MapLoader
    {
        private static List<string> loadErrors = new List<string>();
        private static Dictionary<string, Type> assemblyTypes = new Dictionary<string, Type>();
        private static Dictionary<string, Type> quantifiedAssemblyTypes = new Dictionary<string, Type>();

        private static IPhysicistRegistration registration = null;

        private static XElement rootElement = null;

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

        public static Map CurrentMap
        {
            get;
            private set;
        }

        public static Map Initialize(string filePath, IPhysicistRegistration registrationObject)
        {
            MapLoader.CurrentMap = null;
            MapLoader.HasFailed = false;
            MapLoader.HasErrors = false;
            MapLoader.loadErrors.Clear();
            MapLoader.registration = null;
            
            if (registrationObject != null)
            {
                MapLoader.registration = registrationObject;
                MapLoader.loadErrors.Clear();

                XDocument rootDocument = XDocument.Load(filePath);
                MapLoader.rootElement = rootDocument.Root;
                try
                {
                    MapLoader.CurrentMap = new Map(
                            registrationObject.World,
                            int.Parse(rootElement.Attribute("width").Value, CultureInfo.CurrentCulture),
                            int.Parse(rootElement.Attribute("height").Value, CultureInfo.CurrentCulture));
                }
                catch (AggregateException)
                {
                    MapLoader.HasFailed = true;
                }
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
                }
                catch (AggregateException)
                {
                    MapLoader.HasFailed = true;
                }
            }

            return !MapLoader.HasFailed;
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
                    var backgrounds = MapLoader.CreateInstances(backgroundsEle.Elements(), "class");
                    foreach (var backgroundObject in backgrounds)
                    {
                        Backdrop backdrop = backgroundObject as Backdrop;
                        if (backdrop != null)
                        {
                            MapLoader.CurrentMap.AddBackdrop(backdrop);
                        }
                        else
                        {
                            BackgroundMusic backgroundMusic = backgroundObject as BackgroundMusic;
                            if (backgroundMusic != null)
                            {
                                MapLoader.CurrentMap.AddBackgroundMusic(backgroundMusic);
                            }
                        }
                    }                       
                }

                XElement foregroundsEle = levelRoots.Element("Foregrounds");
                if (foregroundsEle != null)
                {
                    var foregrounds = MapLoader.CreateInstances(foregroundsEle.Elements(), "class");
                    foreach (var foregroundObject in foregrounds)
                    {
                        MapObject mapObject = foregroundObject as MapObject;
                        if (mapObject != null)
                        {
                            MapLoader.CurrentMap.AddMapObject(mapObject);
                        }
                    }
                }

                XElement actorsEle = levelRoots.Element("Actors");
                if (actorsEle != null)
                {
                    var actors = MapLoader.CreateInstances(actorsEle.Elements(), "class");
                    foreach (var actorObject in actors)
                    {
                        Actor actor = actorObject as Actor;
                        if (actor != null)
                        {
                            MapLoader.registration.RegisterActor(actor);
                        }
                    }
                }
            }
        }

        private static IEnumerable<object> CreateInstances(IEnumerable<XElement> elements, string classAttribute)
        {
            List<object> instances = new List<object>();
            foreach (var element in elements)
            {
                instances.Add(MapLoader.CreateInstance(element, classAttribute));
            }

            return instances;
        }

        private static object CreateInstance(XElement element, string classAttribute)
        {
            object instance = null;
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

                if (!classType.IsValueType && classType.GetConstructor(new Type[] { }) != null)
                {
                    instance = Activator.CreateInstance(classType);
                    var gameScreenItem = instance as IGameScreenItem;
                    if (gameScreenItem != null)
                    {
                        gameScreenItem.Screen = MapLoader.registration as GameScreen;
                    }

                    var deserialize = instance as IXmlSerializable;
                    if (deserialize != null)
                    {
                        deserialize.XmlDeserialize(element);
                    }
                    else
                    {
                        MapLoader.ErrorOccured("Warning while loading " + objecttype + " of class: " + element.Attribute(classAttribute).Value + ", Class does not implement Physicist.IXmlSerializable");
                    }
                }
                else
                {
                    MapLoader.ErrorOccured("Error while loading " + objecttype + " of class: " + element.Attribute(classAttribute).Value + ", Class is value type or does not contain a default constructor");
                }
            }
            catch (KeyNotFoundException)
            {
                MapLoader.ErrorOccured("Error while loading " + objecttype + " of class: " + element.Attribute(classAttribute).Value + ", Class type not found!");
            }
            catch (NullReferenceException)
            {
                MapLoader.ErrorOccured("Error while loading " + objecttype + ", 'class' attribute not found!");
            }
            catch (Microsoft.Xna.Framework.Content.ContentLoadException e)
            {
                MapLoader.ErrorOccured("Error while loading " + objecttype + " of class: " + element.Attribute(classAttribute).Value + ", " + e.Message);
            }
            catch (TargetInvocationException e)
            {
                MapLoader.ErrorOccured("Error while loading " + objecttype + " of class: " + element.Attribute(classAttribute).Value + ", " + e.Message);
            }

            return instance;
        }
    }
}
