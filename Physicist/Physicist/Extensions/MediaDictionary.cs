namespace Physicist.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Runtime.Serialization;
    using Physicist.Extensions.Primitives;

    public class MediaElementKeyedCollection<T> : KeyedCollection<string, T> where T : MediaElement
    {
        public string Location(string key)
        {
            return base[key].Location;
        }

        public object MediaData(string key)
        {
            return base[key].Asset;
        }

        protected override string GetKeyForItem(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            return item.Name;
        }
    }
}
