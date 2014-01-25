namespace Physicist.Actors
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;
    using System.Xml;
    using System.Xml.Linq;

    interface IXmlSerializable
    {
        //overloading
        void Serialize(Stream iostream);

        Object Deserialize(XElement element);
    }
}