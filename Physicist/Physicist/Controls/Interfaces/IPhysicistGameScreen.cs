namespace Physicist.Controls
{
    using FarseerPhysics.Dynamics;

    public interface IPhysicistGameScreen
    {
        World World { get; }

        Map Map { get; }
    }
}
