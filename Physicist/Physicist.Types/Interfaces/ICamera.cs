namespace Physicist.Types.Interfaces
{
    using Microsoft.Xna.Framework;
    using Physicist.Types.Camera;

    public interface ICamera
    {
        Viewport Viewport { get; }

        Matrix Transform { get; }

        IReadOnlyPosition Following { get; set; }

        float Rotation { get; set; }

        void CenterOnFollowing();

        void Reset();
    }
}
