namespace Physicist.Controls.Fields
{
    using System;
    using System.Linq;
    using System.Xml.Linq;
    using Physicist.Controls;
    using Physicist.Extensions;

    public class FieldContainer : MapObject, IName
    {
        private Field containedField = null;

        public FieldContainer()
        {
        }

        public FieldContainer(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }

        public Field ContainedField 
        {
            get
            {
                return this.containedField;
            }

            set
            {
                if (this.World != null && this.World.ControllerList.Contains(this.containedField))
                {
                    this.World.RemoveController(this.containedField);
                }

                this.containedField = value;
                if (this.World != null)
                {
                    this.World.AddController(this.containedField);
                }
            }
        }

        public override XElement XmlSerialize()
        {
            return new XElement(
                        "FieldContainer",
                        this.ContainedField.XmlSerialize(),
                        new XElement("BodyInfo", base.XmlSerialize()));
        }

        public override void XmlDeserialize(XElement element)
        {
            if (element != null)
            {
                var bodyInfoEle = element.Element("BodyInfo");
                if (bodyInfoEle != null)
                {
                    base.XmlDeserialize(bodyInfoEle.Elements().ElementAt(0));

                    this.Name = element.GetAttribute("name", string.Empty);

                    this.ContainedField = (Field)MapLoader.CreateInstance(element, null);

                    if (this.ContainedField != null)
                    {
                        this.ContainedField.XmlDeserialize(element);
                        this.ContainedField.FieldBody = this.MapBody;
                    }
                }
            }
        }
    }
}
