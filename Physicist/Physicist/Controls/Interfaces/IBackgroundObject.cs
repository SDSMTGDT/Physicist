namespace Physicist.Controls
{
    using Microsoft.Xna.Framework;
    using Physicist.Extensions;
 
    public interface IBackgroundObject : IXmlSerializable
    {
        Vector2 Location { get; }

        Size Dimensions { get; }
    }
}
