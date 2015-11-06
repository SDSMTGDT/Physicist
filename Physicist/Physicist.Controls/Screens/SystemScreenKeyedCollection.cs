namespace Physicist.Controls.Screens
{
    using System;
    using System.Collections.ObjectModel;
    using Physicist.Types.Enums;
    using Physicist.Types.Interfaces;

    public class SystemScreenKeyedCollection : KeyedCollection<SystemScreen, ISystemScreen>
    {
        protected override SystemScreen GetKeyForItem(ISystemScreen item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            return item.ScreenType;
        }
    }
}
