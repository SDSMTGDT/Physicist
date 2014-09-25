namespace Physicist.Events
{
    using System;
    using Microsoft.Xna.Framework;
    using Physicist.Controls;

    public interface IModifier : IPhysicistGameScreenItem, IName, IUpdate, IXmlSerializable
    {
        bool IsActive { get; set; }

        bool IsEnabled { get; set; }
    }
}
