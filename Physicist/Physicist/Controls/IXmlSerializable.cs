namespace Physicist.Controls
{
    using System.Xml.Linq;

    public interface IXmlSerializable
    {
        XElement XmlSerialize();

        void XmlDeserialize(XElement classData);
    }
}
