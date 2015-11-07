namespace Physicist.Types.Interfaces
{
    using System;
    using Physicist.Types.Util;

    public interface ILayerTransition : IName
    {
        event EventHandler<LayerTransitionEventArgs> LayerTransition;

        string TargetDoor { get; }

        void SetPlayer(IPosition player);
    }
}
