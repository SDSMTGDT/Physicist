namespace Physicist.Types.Interfaces
{
    using System;
    using Microsoft.Xna.Framework;

    public interface ITrigger : IName, IUpdate, IXmlSerializable
    {
        bool IsReusable { get; }

        bool IsEnabled { get; set; }

        bool IsActive { get; set; }
    }
}
