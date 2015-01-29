namespace Physicist.Controls.GUIControls
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public interface IBorder<T>
    {
        Color BorderColor { get; set; }

        Color LeftBorderColor { get; set; }

        Color RightBorderColor { get; set; }

        Color TopBorderColor { get; set; }

        Color BottomBorderColor { get; set; }

        int BorderSize { get; set; }

        Texture2D BorderTexture { get; }

        T Bounds { get; set; }

        void UnloadContent();
    }
}
