namespace Physicist.Types.Interfaces
{
    using Microsoft.Xna.Framework;
    using Physicist.Types.Common;
 
    public interface IBackgroundObject : IXmlSerializable
    {
        Vector2 Location { get; set; }

        Size Dimensions { get; }
    }
}
