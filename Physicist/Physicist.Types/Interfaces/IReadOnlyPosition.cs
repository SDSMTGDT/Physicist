namespace Physicist.Types.Interfaces
{
    using Microsoft.Xna.Framework;

    public interface IReadOnlyPosition
    {
        Vector2 Position { get; }

        Vector2 CenteredPosition { get; }
    }
}