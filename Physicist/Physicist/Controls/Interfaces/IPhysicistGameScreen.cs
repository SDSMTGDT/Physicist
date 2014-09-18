namespace Physicist.Controls
{
    using FarseerPhysics.Dynamics;
    using Physicist.Actors;

    public interface IPhysicistGameScreen
    {
        World World { get; }

        Map Map { get; }
    }
}
