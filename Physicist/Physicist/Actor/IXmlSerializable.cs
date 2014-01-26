namespace Physicist.Actors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;

    public interface IXmlSerializable
    {
        // overloading
        XElement Serialize();

        void Deserialize(XElement element);
    }
}