namespace Physicist.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [Serializable]
    public abstract class KeyedDictionary<TKey, TItem> : Dictionary<TKey, TItem> 
    {
        protected KeyedDictionary()
            : base()
        {
        }

        protected KeyedDictionary(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public abstract TKey GetKeyForItem(TItem item);

        public void Add(TItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            base.Add(this.GetKeyForItem(item), item);
        }

        public void Remove(TItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            base.Remove(this.GetKeyForItem(item));
        }

        public bool Contains(TItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            return this.ContainsKey(this.GetKeyForItem(item));
        }
    }
}