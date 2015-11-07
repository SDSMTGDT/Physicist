namespace Physicist.MainGame.Controls
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Physicist.Types.Enums;
    using Physicist.Types.Interfaces;
    using Physicist.Types.Common;
    
    public interface IActor : IPosition, IName, IUpdate, IDraw, IBody
    {
        bool IsEnabled { get; set; }

        int Health { get; set; }
        
        Visibility VisibleState { get; set; }
        
        bool IsDead { get; }
        
        Dictionary<string, GameSprite> Sprites { get; }
        
        Vector2 MovementSpeed { get; set; }
    }
}
