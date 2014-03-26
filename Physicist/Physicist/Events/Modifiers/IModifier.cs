namespace Physicist.Events
{
    using System;
    using Microsoft.Xna.Framework;
    using Physicist.Controls;

    public interface IModifier : IName, IUpdate
    {
        bool IsActive { get; set; }

        bool IsEnabled { get; set; }
    }
}
