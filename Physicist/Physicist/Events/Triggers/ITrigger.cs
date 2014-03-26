namespace Physicist.Events
{
    using System;
    using Microsoft.Xna.Framework;
    using Physicist.Controls;

    public interface ITrigger : IUpdate
    {
        bool IsReusable { get; }

        bool IsEnabled { get; set; }

        bool IsActive { get; set; }
    }
}
