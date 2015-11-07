namespace Physicist.Types.Interfaces
{
    using Microsoft.Xna.Framework;

    public interface IPosition : IReadOnlyPosition
    {
        new Vector2 Position { get; set; }
    }
}
