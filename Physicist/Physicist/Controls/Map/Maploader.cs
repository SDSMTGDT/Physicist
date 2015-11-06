namespace Physicist.MainGame.Controls
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
    using Microsoft.Xna.Framework;
    using Physicist.Types.Controllers;
    using Physicist.Types.Enums;
    using Physicist.Types.Interfaces;

    public static class MapLoader
    {
        private static List<string> loadErrors = new List<string>();
        private static Dictionary<string, Type> assemblyTypes = new Dictionary<string, Type>();
        private static Dictionary<string, Type> quantifiedAssemblyTypes = new Dictionary<string, Type>();

        private static IPhysicistGameScreen registration = null;

        private static XElement rootElement = null;
        private static XmlSchemaSet schemas = new XmlSchemaSet();
        private static List<XslCompiledTransform> templates = new List<XslCompiledTransform>();
        private static XslCompiledTransform layerTrans;

        private static string currentLayer = "No-processing-done";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = "Simplistic init code")]
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

            foreach (string filename in Directory.EnumerateFiles("XML\\Transforms\\Templates"))
            {
#if DEBUG
                var transform = new XslCompiledTransform(true);
#else
                var transform = new XslCompiledTransform();
#endif
                transform.Load(filename);
                MapLoader.templates.Add(transform);
            }

            layerTrans = new XslCompiledTransform();
            layerTrans.Load("XML\\Transforms\\Layer\\LayerTransform.xslt");
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

        public static Map Initialize(string filePath, IPhysicistGameScreen registrationObject)
        {
            MapLoader.CurrentMap = null;
            MapLoader.HasFailed = false;
            MapLoader.HasErrors = false;
            MapLoader.loadErrors.Clear();
            MapLoader.registration = null;           
            
            if (registrationObject != null)
            {
                bool hasValidationErrors = false;

                MapLoader.registration = registrationObject;

                List<string> validationErrors = new List<string>();
                XDocument rootDocument = new XDocument();
                using (FileStream rstream = File.OpenRead(filePath))
                {
                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.Schemas = MapLoader.schemas;
                    settings.ValidationType = ValidationType.Schema;
                    settings.ValidationEventHandler += (sender, args) =>
                    {
                        validationErrors.Add("Line " + args.Exception.LineNumber + ": " + args.Message);
                        hasValidationErrors = true;
                    };

                    rootDocument = XDocument.Load(XmlReader.Create(rstream, settings));
                }

                if (hasValidationErrors)
                {
                    MapLoader.ErrorOccured("Map " + filePath + " failed to validate against schema!\nValidation error list: ");
                    validationErrors.ForEach(error => MapLoader.ErrorOccured(error));
                    MapLoader.ErrorOccured("Attempting to Load without validation");

                    rootDocument = XDocument.Load(filePath);
                }

                if (rootDocument != null)
                {
                    XsltArgumentList paramlist = new XsltArgumentList();
                    paramlist.AddParam("mapheight", string.Empty, rootDocument.Root.Attribute("height").Value);

                    XDocument transformDoc = new XDocument();
                    XDocument currentReader = rootDocument;
                    foreach (var transform in MapLoader.templates)
                    {
                        transformDoc = new XDocument();
                        using (XmlWriter writer = transformDoc.CreateWriter())
                        {
                            transform.Transform(currentReader.CreateReader(), paramlist, writer);
                            currentReader = transformDoc; // Swap to current doc state
                        }
                    }

                    MapLoader.RemoveNamespaces(transformDoc);

#if DEBUG
                    using (XmlWriter writer = XmlWriter.Create("testout.xml"))
                    {
                        transformDoc.WriteTo(writer);
                    }
#endif
                    MapLoader.rootElement = transformDoc.Root;

                    try
                    {
                        MapLoader.CurrentMap = new Map(
                                registrationObject.World,
                                int.Parse(MapLoader.rootElement.Attribute("width").Value, CultureInfo.CurrentCulture),
                                int.Parse(MapLoader.rootElement.Attribute("height").Value, CultureInfo.CurrentCulture));
                    }
                    catch (AggregateException)
                    {
                        MapLoader.HasFailed = true;
                    }
                }
            }

            return MapLoader.CurrentMap;
        }

        public static bool LoadCurrentMap()
        {
            if (!MapLoader.HasFailed)
            {
                if (rootElement != null && (rootElement.Name.LocalName == "Map"))
                {
                    try
                    {
                        var layers = rootElement.Elements("MapLayer");
                        if (layers.Count() > 0)
                        {
                            for (int i = 0; i < layers.Count(); i++)
                            {
                                var layerEle = layers.ElementAt(i);
                                int xoff = int.Parse(layerEle.Attribute("xoffset").Value, CultureInfo.CurrentCulture);
                                int yoff = int.Parse(layerEle.Attribute("yoffset").Value, CultureInfo.CurrentCulture);
                                var layer = new MapLayer(
                                        MapLoader.registration.World,
                                        layerEle.Attribute("name").Value,
                                        int.Parse(layerEle.Attribute("width").Value, CultureInfo.CurrentCulture),
                                        int.Parse(layerEle.Attribute("height").Value, CultureInfo.CurrentCulture),
                                        int.Parse(layerEle.Attribute("depth").Value, CultureInfo.CurrentCulture),
                                        new Vector2(xoff, yoff),
                                        MapLoader.CurrentMap.Height,
                                        (uint)i + 1);

                                MapLoader.CurrentMap.AddMapLayer(layer);

                                XsltArgumentList paramlist = new XsltArgumentList();
                                paramlist.AddParam("xoffset", string.Empty, xoff);
                                paramlist.AddParam("yoffset", string.Empty, yoff);

                                XDocument layerDoc = new XDocument();
                                using (XmlWriter writer = layerDoc.CreateWriter())
                                {
                                    layerTrans.Transform(layerEle.CreateReader(), paramlist, writer);
                                }

                                XElement rootLayerEle = layerDoc.Root;

                                MapLoader.currentLayer = layer.Name;
                                MapLoader.LoadMedia(rootLayerEle.Element("Media"));
                                MapLoader.LoadLevelObjects(rootLayerEle.Element("LevelObjects"));
                                MapLoader.LoadEvents(rootLayerEle.Element("MapEvents"));                               
                            }
                        }
                        else
                        {
                            var layer = new MapLayer(
                                        MapLoader.registration.World,
                                        Guid.NewGuid().ToString(),
                                        MapLoader.CurrentMap.Width,
                                        MapLoader.CurrentMap.Height,
                                        0,
                                        Vector2.Zero,
                                        MapLoader.CurrentMap.Height,
                                        0);

                            MapLoader.CurrentMap.AddMapLayer(layer);

                            MapLoader.currentLayer = layer.Name;
                            MapLoader.LoadMedia(rootElement.Element("Media"));
                            MapLoader.LoadLevelObjects(rootElement.Element("LevelObjects"));
                            MapLoader.LoadEvents(rootElement.Element("MapEvents"));
                        }
                    }
                    catch (AggregateException)
                    {
                        MapLoader.HasFailed = true;
                        MapLoader.ErrorOccured(string.Format(CultureInfo.CurrentCulture, "Error while loading layer: {0}", MapLoader.currentLayer));
                    }
                }
            }

            return !MapLoader.HasFailed;
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

        public static object CreateInstance(XElement element, string classAttribute)
        {
            object instance = null;

            if (element != null)
            {
                string objecttype = element.Name.ToString();
                string className = null;
                string classAttValue = null;
                var classEle = element.Attribute(classAttribute);
                if (classEle != null)
                {
                    classAttValue = classEle.Value;
                }

                string errorMessage = string.Format(CultureInfo.CurrentCulture, "while loading {0} of class: {1}, ", objecttype, classAttValue);

                try
                {
                    if (classAttValue == null)
                    {
                        className = objecttype;
                    }
                    else
                    {
                        className = classAttValue;
                    }

                    // Try to create type from fully quantified name in current assembly
                    Type classType = Type.GetType(className);

                    // If failure, try to find type in registered assemblies, first by short name, then by full name
                    if (classType == null && !MapLoader.assemblyTypes.TryGetValue(className, out classType))
                    {
                        classType = MapLoader.quantifiedAssemblyTypes[className];
                    }

                    if (!classType.IsValueType && classType.GetConstructor(new Type[] { }) != null)
                    {
                        instance = Activator.CreateInstance(classType);
                    }
                    else
                    {
                        MapLoader.ErrorOccured(string.Format(CultureInfo.CurrentCulture, "Error {0}{1}", errorMessage, "Class is value type or does not contain a default constructor"));
                    }
                }
                catch (KeyNotFoundException)
                {
                    MapLoader.ErrorOccured(string.Format(CultureInfo.CurrentCulture, "Error {0}{1}", errorMessage, "Class type not found!"));
                }
                catch (NullReferenceException)
                {
                    MapLoader.ErrorOccured(string.Format(CultureInfo.CurrentCulture, "Error {0}, 'class' attribute not found!", objecttype));
                }
                catch (Microsoft.Xna.Framework.Content.ContentLoadException e)
                {
                    MapLoader.ErrorOccured(string.Format(CultureInfo.CurrentCulture, "Error {0}{1}", errorMessage, e.Message));
                }
                catch (TargetInvocationException e)
                {
                    MapLoader.ErrorOccured(string.Format(CultureInfo.CurrentCulture, "Error {0}{1}", errorMessage, e.Message));
                }
            }

            return instance;
        }

        private static void LoadEvents(XElement eventRoot)
        {
            if (eventRoot != null)
            {
                MapLoader.CreateAndInitializeInstances(eventRoot.Elements(), "class");
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
                        MapLoader.ErrorOccured(string.Format(CultureInfo.CurrentCulture, "Error while loading {0}: {1}, {2}", typeof(T).Name, element.Attribute(idFilter).Value, e.Message));
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
                    MapLoader.CreateAndInitializeInstances(backgroundsEle.Elements(), "class");
                }

                XElement foregroundsEle = levelRoots.Element("Foregrounds");
                if (foregroundsEle != null)
                {
                    MapLoader.CreateAndInitializeInstances(foregroundsEle.Elements(), "class");
                }

                XElement actorsEle = levelRoots.Element("Actors");
                if (actorsEle != null)
                {
                    MapLoader.CreateAndInitializeInstances(actorsEle.Elements(), "class");
                }
            }
        }

        private static IEnumerable<object> CreateAndInitializeInstances(IEnumerable<XElement> elements, string classAttribute)
        {
            List<object> instances = new List<object>();
            if (elements != null)
            {
                foreach (var element in elements)
                {
                    var instance = MapLoader.CreateInstance(element, classAttribute);
                    MapLoader.InitializeInstance(instance, element);
                    instances.Add(instance);
                }
            }

            return instances;
        }

        private static void InitializeInstance(object instance, XElement element)
        {
            if (instance != null && element != null)
            {
                bool canAdd = true;
                string errorMessage = string.Format(CultureInfo.CurrentCulture, "while loading {0} of class: {1}, ", element.Name, instance.GetType());
                string description = string.Empty;

                var gameScreenItem = instance as IPhysicistGameScreenItem;
                if (gameScreenItem != null)
                {
                    gameScreenItem.Screen = MapLoader.registration as PhysicistGameScreen;
                }

                var deserialize = instance as IXmlSerializable;
                if (deserialize != null)
                {
                    try
                    {
                        deserialize.XmlDeserialize(element);
                    }
                    catch (NullReferenceException)
                    {
                        canAdd = false;
                        description = "Null Reference: try checking element attribute values and media references";
                    }
                    catch (ArgumentNullException)
                    {
                        canAdd = false;
                        description = "Argument Null: try checking media references";
                    }
                    catch (AggregateException)
                    {
                        canAdd = false;
                        description = "Aggregate: One or more unknown errors occurred :(";
                    }
                }
                else
                {
                    MapLoader.ErrorOccured(string.Format(CultureInfo.CurrentCulture, "Warning {0}{1}", errorMessage, "Class does not implement Physicist.IXmlSerializable"));
                }

                if (canAdd)
                {
                    MapLoader.CurrentMap.AddObjectToMap(instance, MapLoader.currentLayer);
                }
                else
                {
                    MapLoader.ErrorOccured(string.Format(CultureInfo.CurrentCulture, "Error {0}{1}", errorMessage, description));
                }
            }
        }
    }
}
