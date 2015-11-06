namespace Physicist.Types.Interfaces
{
    using System.Xml.Linq;

    public interface IXmlSerializable
    {
        XElement XmlSerialize();

        void XmlDeserialize(XElement element);
    }
}
