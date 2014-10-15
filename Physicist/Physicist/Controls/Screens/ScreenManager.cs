namespace Physicist.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Physicist.Enums;
    using Physicist.Extensions;

    public static class ScreenManager
    {
        private static SystemScreenKeyedCollection systemScreens = new SystemScreenKeyedCollection();
        private static List<GameScreen> activeScreens = new List<GameScreen>();

        private static bool isInitialized = false;
        private static int popupCount = 0;
        private static int modalCount = 0;
        private static GameScreen currentScreen;

        public static event EventHandler<EventArgs> Quit;

        public static GraphicsDevice GraphicsDevice { get; private set; }
        
        public static bool IsInitialized 
        {
            get
            {
                return ScreenManager.isInitialized;
            }

            private set
            {
                ScreenManager.isInitialized = value;
            }
        }

        private static GameScreen CurrentScreen
        {
            get
            {
                return ScreenManager.currentScreen;
            }

            set
            {
                if (value != null && ScreenManager.currentScreen != value)
                {
                    ScreenManager.currentScreen = value;
                }
            }
        }

        public static void Initialize(GraphicsDevice graphicsDev)
        {
            ScreenManager.GraphicsDevice = graphicsDev;
            ScreenManager.IsInitialized = true;

            systemScreens.Add(ScreenManager.AddSystemScreen(SystemScreen.MenuScreen));
            systemScreens.Add(ScreenManager.AddSystemScreen(SystemScreen.PauseScreen));

            ScreenManager.CurrentScreen = ScreenManager.systemScreens[SystemScreen.MenuScreen];
        }

        public static void Draw(FCCSpritebatch sb)
        {
            if (sb != null)
            {
                if (ScreenManager.activeScreens.Count == 0)
                {
                    sb.SetBandwidth(1f, 0f);
                    sb.Begin(
                        SpriteSortMode.FrontToBack, 
                        BlendState.AlphaBlend, 
                        SamplerState.LinearClamp, 
                        DepthStencilState.Default, 
                        RasterizerState.CullCounterClockwise, 
                        null, 
                        ScreenManager.systemScreens[SystemScreen.MenuScreen].Camera.Transform);

                    ScreenManager.systemScreens[SystemScreen.MenuScreen].Draw(sb);
                    sb.End();
                }
                else
                {
                    for (int i = ScreenManager.activeScreens.Count - popupCount - 1; i < ScreenManager.activeScreens.Count; i++)
                    {
                        var screen = ScreenManager.activeScreens[i];
                        if (screen.IsActive)
                        {
                            if (screen.IsPopup)
                            {
                                sb.SetBandwidth(1f, .8f);
                            }
                            else
                            {
                                sb.SetBandwidth(.79f, 0f);
                            }

                            sb.Begin(
                                SpriteSortMode.FrontToBack, 
                                BlendState.AlphaBlend, 
                                SamplerState.LinearClamp, 
                                DepthStencilState.Default, 
                                RasterizerState.CullCounterClockwise, 
                                null,
                                screen.Camera.Transform);

                            screen.Draw(sb);
                            sb.End();
                        }
                    }
                }

                sb.End();
            }
        }

        public static void Update(GameTime gameTime)
        {
            // Update popups
            for (int i = modalCount; i < ScreenManager.activeScreens.Count; i++)
            {
                var screen = ScreenManager.activeScreens[i];
                if (screen.IsActive)
                {
                    screen.Update(gameTime);
                }
            }

            ScreenManager.CurrentScreen.Update(gameTime);
        }

        public static void UnloadContent()
        {
            foreach (var screen in ScreenManager.activeScreens)
            {
                screen.Dispose();                
            }

            foreach (var screen in ScreenManager.systemScreens)
            {
                screen.Dispose();
            }

            ScreenManager.activeScreens.Clear();
        }

        public static void AddScreen(SystemScreen screen)
        {
            switch (screen)
            {
                case SystemScreen.PauseScreen:
                    ScreenManager.activeScreens.Add(ScreenManager.systemScreens[SystemScreen.PauseScreen]);
                    ScreenManager.currentScreen = ScreenManager.systemScreens[SystemScreen.PauseScreen];
                    ScreenManager.popupCount++;
                    ScreenManager.modalCount++;
                    break;
            }
        }

        public static void AddScreen(GameScreen screen)
        {
            if (ScreenManager.IsInitialized && screen != null && !ScreenManager.activeScreens.Contains(screen))
            {
                screen.Initialize(ScreenManager.GraphicsDevice);
                if (screen.LoadContent())
                {
                    if (screen.IsPopup)
                    {
                        ScreenManager.popupCount++;
                    }

                    if (screen.IsModal)
                    {
                        ScreenManager.modalCount++;
                    }

                    ScreenManager.activeScreens.Add(screen);
                    ScreenManager.currentScreen = screen;
                }
                else
                {
                    System.Console.WriteLine("Error while loading screen {0} of type: {1}", screen.Name, screen.GetType().Name);
                }
            }
        }

        public static void RemoveScreen(string screenName)
        {
            if (screenName == SystemScreen.MenuScreen.ToString())
            {
                if (ScreenManager.Quit != null)
                {
                    ScreenManager.Quit(null, null);
                }
            }
            else
            {
                var screen = ScreenManager.activeScreens.FirstOrDefault(sc => sc.Name == screenName);
                if (screen != null)
                {
                    ScreenManager.activeScreens.Remove(screen);

                    if (screen.IsPopup)
                    {
                        ScreenManager.popupCount--;
                    }

                    if (screen.IsModal)
                    {
                        ScreenManager.modalCount--;
                    }
                }

                ScreenManager.CurrentScreen = ScreenManager.activeScreens.Count == 0 ?
                                                ScreenManager.systemScreens[SystemScreen.MenuScreen] :
                                                ScreenManager.activeScreens[ScreenManager.activeScreens.Count - 1];
            }
        }

        public static void PopBackTo(SystemScreen screen)
        {
            switch (screen)
            {
                case SystemScreen.MenuScreen:
                    ScreenManager.UnloadContent();
                    ScreenManager.CurrentScreen = ScreenManager.systemScreens[SystemScreen.MenuScreen];
                    break;
            }
        }

        public static void PopBackTo(string screenName)
        {
            while (ScreenManager.activeScreens[ScreenManager.activeScreens.Count - 1].Name != screenName)
            {
                ScreenManager.RemoveScreen(ScreenManager.activeScreens[ScreenManager.activeScreens.Count - 1].Name);
            }
        }
        
        private static GameScreen AddSystemScreen(SystemScreen screenType)
        {
            GameScreen systemScreen = null;
            GameScreen tempScreen = null;
            try
            {
                switch (screenType)
                {
                    case SystemScreen.MenuScreen:
                        tempScreen = new MenuScreen();
                        break;

                    case SystemScreen.PauseScreen:
                        tempScreen = new PauseScreen();
                        break;
                }

                tempScreen.Initialize(ScreenManager.GraphicsDevice);
                tempScreen.LoadContent();
                systemScreen = tempScreen;
                tempScreen = null;
            }
            finally
            {
                if (tempScreen != null)
                {
                    tempScreen.Dispose();
                }
            }

            return systemScreen;
        }
    }
}
