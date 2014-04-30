namespace Physicist.Controls
{
    using FarseerPhysics.Dynamics;
    using Physicist.Actors;

    public interface IPhysicistRegistration
    {
        World World { get; }

        Map Map { get; }
    }
}
