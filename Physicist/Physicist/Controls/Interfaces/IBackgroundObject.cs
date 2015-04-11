namespace Physicist.Controls
{
    using Microsoft.Xna.Framework;
    using Physicist.Extensions;
 
    public interface IBackgroundObject
    {
        Vector2 Location { get; set; }

        Size Dimensions { get; }
    }
}
