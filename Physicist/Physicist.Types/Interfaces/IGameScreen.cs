namespace Physicist.Types.Interfaces
{
    using System;
    using System.Collections.ObjectModel;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Physicist.Types.Xna;

    public interface IGameScreen : IName, IDisposable
    {
        bool IsActive { get; }

        bool IsPopup { get; }

        bool IsModal { get; }

        ICamera Camera { get; }

        GraphicsDevice GraphicsDevice { get; }

        ReadOnlyCollection<IGUIElement> GUIElements { get; }

        bool AddGUIElement(IGUIElement element);

        void Draw(FCCSpritebatch sb);

        void DrawScreen(FCCSpritebatch sb);

        void DrawGUI(FCCSpritebatch sb);

        IGUIElement GetElement(string name);

        void Initialize();

        void InitializeScreen(GraphicsDevice graphicsDevice);

        bool LoadContent();

        bool LoadGUI();

        bool LoadScreenContent();

        void PopScreen();

        void UnloadContent();

        void UnloadScreenContent();

        void UnloadGUI();

        void UpdateScreen(GameTime gameTime);

        void Update(GameTime gameTime);

        void UpdateGUI(GameTime gameTime);
    }
}
