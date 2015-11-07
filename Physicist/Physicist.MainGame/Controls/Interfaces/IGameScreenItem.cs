namespace Physicist.MainGame.Controls
{
    using Physicist.Types.Interfaces;

    public interface IPhysicistGameScreenItem : IXmlSerializable
    {
        PhysicistGameScreen Screen { get; set; }
    }
}
