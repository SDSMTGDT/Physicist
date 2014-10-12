namespace Physicist.Extensions
{
    using System;
    using System.Collections.ObjectModel;
    using Physicist.Controls;
    using Physicist.Enums;

    public class SystemScreenKeyedCollection : KeyedCollection<SystemScreen, GameScreen>
    {
        protected override SystemScreen GetKeyForItem(GameScreen item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            return (SystemScreen)Enum.Parse(typeof(SystemScreen), item.Name);
        }
    }
}
