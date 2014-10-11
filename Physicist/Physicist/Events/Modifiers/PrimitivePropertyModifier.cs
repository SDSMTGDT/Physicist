namespace Physicist.Events.Modifiers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;
    using Physicist.Enums;

    public class PrimitivePropertyModifier : Modifier<object>
    {
        private string targetPropertyName = string.Empty;
        private Type targetPropertyType = null;
        private object newValue;
        private bool hasMemory = false;

        private Dictionary<int, PropertyInfo> propertyInfo = new Dictionary<int, PropertyInfo>();
        private Dictionary<int, object> previousValues = new Dictionary<int, object>();

        public override void XmlDeserialize(XElement element)
        {
            if (element != null)
            {
                var propNameAtt = element.Attribute("PropertyName");
                if (propNameAtt != null)
                {
                    this.targetPropertyName = propNameAtt.Value;
                }

                var propTypeAtt = element.Attribute("PropertyType");
                if (propTypeAtt != null)
                {
                    // Checking supported type in event of unvalidated schema
                    var supportedPropertyType = PrimitiveTypesHelper.ToSystemType((PrimitiveType)Enum.Parse(typeof(PrimitiveType), propTypeAtt.Value));
                    if (supportedPropertyType != null)
                    {
                        this.targetPropertyType = supportedPropertyType;
                    }
                }

                var hasMemoryAtt = element.Attribute("HasMemory");
                if (hasMemoryAtt != null)
                {
                    this.hasMemory = bool.Parse(hasMemoryAtt.Value);
                }

                var newValueAtt = element.Attribute("NewValue");
                if (newValueAtt != null)
                {
                    var converter = TypeDescriptor.GetConverter(this.targetPropertyType);
                    this.newValue = converter.ConvertFrom(newValueAtt.Value);
                }

                base.XmlDeserialize(element.Element("Modifier"));
            }
        }

        protected override void OnActivated()
        {
            foreach (var obj in this.Targets)
            {
                var property = this.propertyInfo[obj.GetHashCode()];

                if (this.hasMemory)
                {
                    this.previousValues[obj.GetHashCode()] = property.GetValue(obj, null);
                }
                
                property.SetValue(obj, this.newValue, null);
            }
        }

        protected override void OnDeactivated()
        {
            if (this.hasMemory)
            {
                foreach (var obj in this.Targets)
                {
                    if (this.previousValues[obj.GetHashCode()] != null)
                    {
                        this.propertyInfo[obj.GetHashCode()].SetValue(obj, this.previousValues[obj.GetHashCode()], null);
                    }
                }
            }
        }

        protected override void SetTargets(IEnumerable<object> targetObjects)
        {
            if (targetObjects != null)
            {
                foreach (var obj in targetObjects) 
                {
                    if (obj != null)
                    {
                        var objPropertyInfo = obj.GetType().GetProperties().FirstOrDefault(p => p.Name == this.targetPropertyName && p.PropertyType == this.targetPropertyType);
                        if (objPropertyInfo != null) 
                        {
                            this.AddTarget(obj);
                            this.propertyInfo.Add(obj.GetHashCode(), objPropertyInfo);
                            this.previousValues.Add(obj.GetHashCode(), null);
                        }
                    }
                }
            }
        }
    }
}
