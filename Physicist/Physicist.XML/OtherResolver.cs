namespace Physicist.XML
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using Physicist.XML;

    public class OtherResolver : XmlResolver
    {
        private const string URNScheme = "urn:";

        public override Uri ResolveUri(Uri baseUri, string relativeUri)
        {
            Uri path = null;
            var resourcePath = XmlResourceLoader.Resources.FirstOrDefault(r => r.EndsWith(relativeUri.Replace('/', '.').TrimStart('.')));
            if (resourcePath != null)
            {
                path = new Uri(OtherResolver.URNScheme + resourcePath); // new Uri(basePath, uriPath);
            }

            return path ?? base.ResolveUri(baseUri, relativeUri);
        }

        public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            Stream stream = null;

            stream = XmlResourceLoader.LoaderAssembly.GetManifestResourceStream(absoluteUri.AbsolutePath);

            return stream;
        }
    }
}