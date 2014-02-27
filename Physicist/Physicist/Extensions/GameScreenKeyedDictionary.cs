namespace Physicist.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Physicist.Controls;
    using Physicist.Enums;

    [Serializable]
    public class SystemScreenKeyedDictionary : KeyedDictionary<SystemScreen, GameScreen>
    {
        public SystemScreenKeyedDictionary() :
            base()
        {
        }

        protected SystemScreenKeyedDictionary(SerializationInfo info, StreamingContext context) :
            base(info, context)
        {
        }

        public override SystemScreen GetKeyForItem(GameScreen item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            return (SystemScreen)Enum.Parse(typeof(SystemScreen), item.Name);
        }
    }
}
