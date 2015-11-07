namespace Physicist.MainGame.Controls
{
    using Physicist.Types.Interfaces;

    public interface IModifier : IPhysicistGameScreenItem, IName, IUpdate, IXmlSerializable
    {
        bool IsActive { get; set; }

        bool IsEnabled { get; set; }
    }
}
