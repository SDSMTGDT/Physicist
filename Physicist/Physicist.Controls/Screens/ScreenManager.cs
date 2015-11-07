namespace Physicist.Controls.Screens
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Physicist.Types.Interfaces;
    using Physicist.Types.Enums;
    using Physicist.Types.Util;
    using Physicist.Types.Xna;

    public static class ScreenManager
    {
        private static SystemScreenKeyedCollection systemScreens = new SystemScreenKeyedCollection();
        private static List<IGameScreen> activeScreens = new List<IGameScreen>();

        private static bool isInitialized = false;
        private static int popupCount = 0;
        private static int modalCount = 0;
        private static IGameScreen currentScreen;

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

        private static IGameScreen CurrentScreen
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

        public static void Initialize(GraphicsDevice graphicsDev, Lazy<IEnumerable<ISystemScreen>> screens)
        {
            ScreenManager.GraphicsDevice = graphicsDev;
            ScreenManager.IsInitialized = true;

            screens.Value.ForEach(s =>
            {
                s.InitializeScreen(ScreenManager.GraphicsDevice);
                s.LoadScreenContent();

                systemScreens.Add(s);
            });

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

                    ScreenManager.systemScreens[SystemScreen.MenuScreen].DrawScreen(sb);
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

                            screen.DrawScreen(sb);
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
                    screen.UpdateScreen(gameTime);
                }
            }

            ScreenManager.CurrentScreen.UpdateScreen(gameTime);
        }

        public static void UnloadContent()
        {
            foreach (var screen in ScreenManager.activeScreens)
            {
                if (!(screen is ISystemScreen))
                {
                    screen.Dispose();
                }
            }

            foreach (var screen in ScreenManager.systemScreens)
            {
                screen.Dispose();
            }

            ScreenManager.activeScreens.Clear();
        }

        public static void AddScreen(SystemScreen systemScreen)
        {
            if(ScreenManager.systemScreens.Contains(systemScreen))
            {
                ScreenManager.AddScreenInternal(ScreenManager.systemScreens[systemScreen]);
            }
        }

        public static void AddScreen(IGameScreen screen)
        {
            if (ScreenManager.IsInitialized && screen != null && !ScreenManager.activeScreens.Contains(screen))
            {
                screen.InitializeScreen(ScreenManager.GraphicsDevice);
                if (screen.LoadScreenContent())
                {
                    ScreenManager.AddScreenInternal(screen);
                }
                else
                {
                    Console.WriteLine("Error while loading screen {0} of type: {1}", screen.Name, screen.GetType().Name);
                }
            }
        }

        private static void AddScreenInternal(IGameScreen screen)
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
                    screen.UnloadContent();

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
                                                (IGameScreen)ScreenManager.activeScreens[ScreenManager.activeScreens.Count - 1];
            }
        }

        public static void PopBackTo(SystemScreen screen)
        {
            switch (screen)
            {
                case SystemScreen.MenuScreen:
                    for (int i = ScreenManager.activeScreens.Count - 1; i >= 0; i--)
                    {
                        ScreenManager.activeScreens[i].UnloadContent();
                        ScreenManager.activeScreens.RemoveAt(i);
                    }

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
    }
}
