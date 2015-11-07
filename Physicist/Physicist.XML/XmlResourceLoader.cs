namespace Physicist.XML
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Xsl;

    public static class XmlResourceLoader
    {
        private static readonly XmlSchemaSet schemas = new XmlSchemaSet();
        private static readonly List<XslCompiledTransform> templates = new List<XslCompiledTransform>();
        private static readonly List<XslCompiledTransform> layers = new List<XslCompiledTransform>();

        static XmlResourceLoader()
        {
            foreach (var dir in Directory.EnumerateDirectories("XML\\Schemas"))
            {
                foreach (var file in Directory.EnumerateFiles(dir))
                {
                    schemas.Add(null, XmlReader.Create(file));
                }
            }

            foreach (var file in Directory.EnumerateFiles("XML\\Transforms\\Templates"))
            {
#if DEBUG
                var template = new XslCompiledTransform(true);
#else
                var template = new XslCompiledTransform();
#endif
                template.Load(file);
                templates.Add(template);
            }

            foreach (var file in Directory.EnumerateFiles("XML\\Transforms\\Layer"))
            {
#if DEBUG
                var template = new XslCompiledTransform(true);
#else
                var template = new XslCompiledTransform();
#endif
                template.Load(file);
                layers.Add(template);
            }
        }

        public static XmlSchemaSet Schemas { get { return schemas; } }

        public static IEnumerable<XslCompiledTransform> Templates { get { return templates; } }

        public static IEnumerable<XslCompiledTransform> Layers { get { return layers; } }

    }
}
