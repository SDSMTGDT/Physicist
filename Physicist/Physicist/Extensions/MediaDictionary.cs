namespace Physicist.Extensions
{
    using System;
    using System.Collections.ObjectModel;
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

        public new void ChangeItemKey(T item, string newKey)
        {
            this.ChangeItemKey(item, newKey);
        }

        public bool ContainsLocation(string location)
        {
            bool found = false;
            foreach (var media in this.Items)
            {
                if (string.Compare(location, media.Location, StringComparison.CurrentCulture) == 0)
                {
                    found = true;
                    break;
                }
            }

            return found;
        }

        public string KeyForLocation(string location)
        {
            string key = null;
            foreach (var media in this.Dictionary.Values)
            {
                if (string.Compare(location, media.Location, StringComparison.CurrentCulture) == 0)
                {
                    key = media.Name;
                    break;
                }
            }

            return key;
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
