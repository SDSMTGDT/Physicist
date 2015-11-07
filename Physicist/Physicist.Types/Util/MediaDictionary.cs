namespace Physicist.Types.Util
{
    using System;
    using System.Collections.ObjectModel;
    using Physicist.Types.Common;

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
