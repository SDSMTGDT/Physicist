namespace Physicist.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using Physicist.Extensions.Primitives;

    [Serializable]
    public class MediaElementKeyedDictionary<T> : KeyedDictionary<string, T> where T : MediaElement
    {
        public MediaElementKeyedDictionary() :
            base()
        {
        }

        protected MediaElementKeyedDictionary(SerializationInfo info, StreamingContext context) :
            base(info, context)
        {
        }

        public string Location(string key)
        {
            return base[key].Location;
        }

        public object MediaData(string key)
        {
            return base[key].Asset;
        }

        public override string GetKeyForItem(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            return item.Name;
        }
    }
}
