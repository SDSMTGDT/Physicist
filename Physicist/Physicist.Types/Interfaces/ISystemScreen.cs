namespace Physicist.Types.Interfaces
{
    using Physicist.Types.Enums;

    public interface ISystemScreen : IGameScreen
    {
        SystemScreen ScreenType { get; }
    }
}
