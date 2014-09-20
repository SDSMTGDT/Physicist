namespace Physicist.Controls
{
    using Microsoft.Xna.Framework;

    public interface IPosition
    {
        Vector2 Position { get; }
        Vector2 CenteredPosition { get; }
    }
}