namespace Physicist.Types.Util
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Linq;

    public static class LinqToXmlExtensions
    {
        public static void TryAddNullableAttributeToXml<T>(this XContainer element, string attributeName, T? field) where T : struct
        {
            if (element != null && !string.IsNullOrEmpty(attributeName) && field.HasValue)
            {
                element.Add(new XAttribute(attributeName, field));
            }
        }

        /// <summary>
        /// Attempts to convert an attribute of an XML element to a given type.
        /// If attribute is not found, will throw an exception.
        /// </summary>
        public static T GetAttribute<T>(this XElement element, string attributeName)
        {
            return element.GetAttribute(attributeName, default(T));
        }

        /// <summary>
        /// Attempts to convert an attribute of an XML element to a given type.
        /// If attribute is not found, or cannot be converted, returns a given default value.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "All exceptions are handled the same")]
        public static T GetAttribute<T>(this XElement element, string attributeName, T defaultValue)
        {
            T value = defaultValue;
            if (element != null)
            {
                try
                {
                    if (element.Attributes(attributeName).Count() > 0)
                    {
                        if (typeof(T).IsEnum)
                        {
                            value = (T)Enum.Parse(typeof(T), element.Attribute(attributeName).Value);
                        }
                        else
                        {
                            value = (T)Convert.ChangeType(element.Attribute(attributeName).Value, typeof(T), CultureInfo.CurrentCulture);
                        }
                    }
                    else
                    {
                        value = defaultValue;
                    }
                }
                catch (InvalidCastException)
                {
                    value = (T)Enum.Parse(typeof(T), element.Attribute(attributeName).Value);
                }
                catch
                {
                    value = defaultValue;
                }
            }

            return value;
        }
    }
}
