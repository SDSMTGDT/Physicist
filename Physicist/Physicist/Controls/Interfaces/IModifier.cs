namespace Physicist.Controls
{
    using System;
    using Microsoft.Xna.Framework;

    public interface IModifier : IPhysicistGameScreenItem, IName, IUpdate, IXmlSerializable
    {
        bool IsActive { get; set; }

        bool IsEnabled { get; set; }
    }
}
