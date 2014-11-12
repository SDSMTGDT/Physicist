namespace Physicist.Controls.GUIControls
{
    using System;

    public interface IGUIElement : IUpdate, IDraw, IName
    {
        object Parent { get; set; }

        void UnloadContent();
    }
}
