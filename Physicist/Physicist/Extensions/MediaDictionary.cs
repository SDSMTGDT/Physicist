namespace Physicist.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using Physicist.Extensions.Primitives;

    [Serializable]
    public class MediaDictionary<T> : Dictionary<string, T> where T : MediaElement
    {
        public MediaDictionary()
            : base()
        {
        }

        protected MediaDictionary(SerializationInfo info, StreamingContext context)
            : base(info, context)
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

        public void Add(T item)
        {
            base.Add(item.Name, item);
        }
    }
}
