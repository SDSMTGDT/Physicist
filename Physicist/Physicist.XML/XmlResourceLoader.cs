namespace Physicist.XML
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Xsl;

    public static class XmlResourceLoader
    {
        internal static readonly Assembly LoaderAssembly = Assembly.GetExecutingAssembly();

        internal static readonly string ResourceRoot = LoaderAssembly.GetName().Name;

        internal static readonly IEnumerable<string> Resources = LoaderAssembly.GetManifestResourceNames();

        public static readonly SchemaResolver resolver = new SchemaResolver();

        private const string URNScheme = "urn:";

        private static readonly XmlSchemaSet schemas = new XmlSchemaSet()
        {
            XmlResolver = resolver,
        };

        private static HashSet<string> cache = new HashSet<string>();

        private static readonly List<XslCompiledTransform> templates = new List<XslCompiledTransform>();
        private static readonly List<XslCompiledTransform> layers = new List<XslCompiledTransform>();

        static XmlResourceLoader()
        {
            foreach(var dir in Directory.EnumerateDirectories("Schemas"))
            {
                foreach(var file in Directory.EnumerateFiles(dir))
                {
                    schemas.Add(null, XmlReader.Create(file));
                }
            }

            foreach(var file in Directory.EnumerateFiles("Transforms\\Templates"))
            {
                var template = new XslCompiledTransform();
                template.Load(file);
                templates.Add(template);
            }

            foreach (var file in Directory.EnumerateFiles("Transforms\\Layer"))
            {
                var template = new XslCompiledTransform();
                template.Load(file);
                layers.Add(template);
            }

            //XmlResourceLoader.ProcesstXmlResourceContentReader("Schemas",
            //    reader =>
            //    {
            //        schemas.Add(null, reader);
            //    });

            //foreach(XmlSchema schema in schemas.Schemas())
            //{
            //    schemas.Reprocess(schema);
            //}
            //schemas.Compile();
            //XmlResourceLoader.ProcesstXmlResourceContentReader("Templates",
            //    reader =>
            //    {
            //        var template = new XslCompiledTransform(true);
            //        template.Load(reader, null, XmlResourceLoader.resolver);
            //        templates.Add(template);
            //    });

            //XmlResourceLoader.ProcesstXmlResourceContentReader("Layer",
            //    reader =>
            //    {
            //        var template = new XslCompiledTransform(true);
            //        template.Load(reader, null, XmlResourceLoader.resolver);
            //        layers.Add(template);
            //    });
        }


        public static XmlSchemaSet Schemas { get { return schemas; } }

        public static IEnumerable<XslCompiledTransform> Templates { get { return templates; } }

        public static IEnumerable<XslCompiledTransform> Layers { get { return layers; } }

        private static void ProcesstXmlResourceContentReader(string relativeRoot, Action<XmlReader> action)
        {
            foreach(var r in Resources.Where(r => r.Substring(ResourceRoot.Length + 1).StartsWith(relativeRoot)))
            {
                using (var stream = LoaderAssembly.GetManifestResourceStream(r))
                {
                    using (var reader = XmlReader.Create(stream, null, r))
                    {
                        if (!cache.Contains(r))
                        {
                            cache.Add(r);
                            action(reader);
                        }
                    }
                }
            }
        }

        private static void ForEachUsing<TSource>(this IEnumerable<TSource> source, Action<TSource> action) where TSource : IDisposable
        {
            foreach (var dispose in source)
            {
                using (dispose)
                {
                    action(dispose);
                }
            }
        }

        public class SchemaResolver : XmlResolver
        {
            public override Uri ResolveUri(Uri baseUri, string relativeUri)
            {
                Uri path = null;
                var resourcePath = XmlResourceLoader.Resources.FirstOrDefault(r => r.EndsWith(relativeUri.Replace('/', '.').TrimStart('.')));
                if (resourcePath != null)
                {
                    path = new Uri(XmlResourceLoader.URNScheme + resourcePath); // new Uri(basePath, uriPath);
                }

                return path ?? base.ResolveUri(baseUri, relativeUri);
            }

            public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
            {
                Stream stream = null;

                if (!cache.Contains(absoluteUri.AbsolutePath))
                {
                    stream = XmlResourceLoader.LoaderAssembly.GetManifestResourceStream(absoluteUri.AbsolutePath);
                    using (var r = XmlReader.Create(stream, null, absoluteUri.AbsolutePath))
                    {
                        cache.Add(absoluteUri.AbsolutePath);
                        schemas.Add(null, r);
                    }
                }

                return stream;
            }
        }
    }
}
