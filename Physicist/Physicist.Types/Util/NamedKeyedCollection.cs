namespace Physicist.Types.Util
{
    using System;
    using System.Collections.ObjectModel;
    using Physicist.Types.Interfaces;

    public class NamedKeyedCollection<T> : KeyedCollection<string, T> where T : IName
    {
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
