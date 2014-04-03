namespace Physicist.Controls
{
    using System;
    using Microsoft.Xna.Framework;

    public interface IModifier : IXmlSerializable
    {
        bool IsActive { get; set; }

        bool IsEnabled { get; set; }

        string Name { get; set; }

        void Update(GameTime gameTime);
    }
}
