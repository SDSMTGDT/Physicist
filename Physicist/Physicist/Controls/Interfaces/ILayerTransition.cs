namespace Physicist.Controls
{
    using System;
    using Physicist.Actors;
    using Physicist.Extensions;

    public interface ILayerTransition : IName
    {
        event EventHandler<LayerTransitionEventArgs> LayerTransition;

        string TargetDoor { get; }

        void SetPlayer(Player player);
    }
}
