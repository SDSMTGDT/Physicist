namespace Physicist.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Schema;
    using System.Xml.Xsl;
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

        private static Map currentMap = null;
        private static XmlSchemaSet schemas = new XmlSchemaSet();
        private static List<XslCompiledTransform> transforms = new List<XslCompiledTransform>();

        static MapLoader()
        {
            var assemblyNames = new List<string>();
            assemblyNames.Add("Physicist");
            assemblyNames.Add("MonoGame.Framework");
            assemblyNames.Add("FarseerPhysics MonoGame");

            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(assembly => assemblyNames.Contains(assembly.GetName().Name));
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

            foreach (string dirName in Directory.EnumerateDirectories("XML\\Schemas"))
            {
                foreach (string filename in Directory.EnumerateFiles(dirName))
                {
                    MapLoader.schemas.Add(null, filename);
                }
            }

            foreach (string filename in Directory.EnumerateFiles("XML\\Templates"))
            {
                var transform = new XslCompiledTransform();
                transform.Load(filename);
                MapLoader.transforms.Add(transform);
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

        public static Map LoadMap(string filePath)
        {
            MapLoader.HasFailed = false;
            MapLoader.HasErrors = false;
                
            List<string> validationErrors = new List<string>();
            XDocument rootDocument = new XDocument();
            XDocument transformDoc = new XDocument();
            using (FileStream rstream = File.OpenRead(filePath))
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.Schemas = MapLoader.schemas;
                settings.ValidationType = ValidationType.Schema;
                settings.ValidationEventHandler += (sender, args) =>
                                                    {
                                                        validationErrors.Add("Line " + args.Exception.LineNumber + ": " + args.Message);
                                                        MapLoader.HasFailed = true;
                                                    };

                rootDocument = XDocument.Load(XmlReader.Create(rstream, settings));
            }

            if (MapLoader.HasFailed)
            {
                MapLoader.ErrorOccured("Map " + filePath + " failed to validate against schema!\nValidation error list: ");
                validationErrors.ForEach(error => MapLoader.ErrorOccured(error));
            }
            else
            {
                using (XmlWriter writer = transformDoc.CreateWriter())
                {
                    foreach (var transform in MapLoader.transforms)
                    {
                        transform.Transform(rootDocument.CreateReader(), writer);
                    }
                }

                MapLoader.RemoveNamespaces(transformDoc);

                XElement rootElement = transformDoc.Root;
                if (rootElement != null && (rootElement.Name.LocalName == "Map"))
                {
                    try
                    {
                        MapLoader.currentMap = new Map(
                                int.Parse(rootElement.Attribute("width").Value, CultureInfo.CurrentCulture),
                                int.Parse(rootElement.Attribute("height").Value, CultureInfo.CurrentCulture));

                        Map.SetCurrentMap(MapLoader.currentMap);

                        MapLoader.LoadMedia(rootElement.Element("Media"));
                        MapLoader.LoadLevelObjects(rootElement.Element("LevelObjects"));
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
            }

            return MapLoader.currentMap;
        }

        public static void RemoveNamespaces(XDocument document)
        {
            if (document != null)
            {
                foreach (var item in document.Root.DescendantNodesAndSelf())
                {
                    var element = item as XElement;
                    if (element != null)
                    {
                        if (element.Name.Namespace != XNamespace.None)
                        {
                            element.Name = element.Name.LocalName;
                        }

                        if (element.Attributes().Where(attribute => attribute.IsNamespaceDeclaration || (attribute.Name.Namespace != XNamespace.None)).Any())
                        {
                            element.ReplaceAttributes(
                                                    element.Attributes()
                                                    .Select(attribute =>
                                                        attribute.IsNamespaceDeclaration ? null :
                                                            attribute.Name.Namespace != XNamespace.None ? new XAttribute(attribute.Name.LocalName, attribute.Value) : attribute));
                        }
                    }
                }
            }
        }

        private static void ErrorOccured(string errorMsg)
        {
            MapLoader.loadErrors.Add(errorMsg);
            Console.WriteLine(errorMsg);
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
                            MapLoader.currentMap.AddBackdrop(backdrop);
                        }
                        else
                        {
                            BackgroundMusic backgroundMusic = backgroundObject as BackgroundMusic;
                            if (backgroundMusic != null)
                            {
                                MapLoader.currentMap.AddBackgroundMusic(backgroundMusic);
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
                            MapLoader.currentMap.AddMapObject(mapObject);
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
                            MainGame.RegisterActor(actor);
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

                if (!classType.IsValueType && classType.GetConstructor(new Type[] { typeof(XElement) }) != null)
                {
                    instance = Activator.CreateInstance(classType, new object[] { element });
                }
                else
                {
                    MapLoader.ErrorOccured("Error Error while loading " + objecttype + " of class: " + element.Attribute(classAttribute).Value + ", Class does not contain a constructor accepting a single XElement");
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
            catch (TargetInvocationException e)
            {
                MapLoader.ErrorOccured("Error while loading " + objecttype + " of class: " + element.Attribute(classAttribute).Value + ", " + e.Message);
            }

            return instance;
        }
    }
}
