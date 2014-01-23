namespace Physicist.Actors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Physicist;

    public class Door : Actor
    {
        // METHODS
        public Door()
            : base()
        {
        }

        // event when the door is closed, either for ending the level (isEndDoor) or ending the animation (!isEndDoor)
        public event EventHandler DoorClosed;

        // if the player can enter the door on a key press
        public bool IsPlayerValidEntry(Actor player)
        {
            bool valid = false;
            if (player != null && this.Sprites["Door"].CurrentSprite.Contains(new Point((int)player.Position.X, (int)player.Position.Y)))
            {
                throw new NotImplementedException();
            }

            return valid;
        }

        public override void AddSprite(string name, GameSprite sprite)
        {
            if (sprite != null && sprite.AnimationKeys.Contains("Activate"))
            {
                throw new NotImplementedException();
            }

            base.AddSprite(name, sprite);
        }

        public void ActivateDoor(Actor player)
        {
            if (this.IsPlayerValidEntry(player))
            {
                this.Sprites["Door"].CurrentAnimationString = "Activate";

                this.CloseDoor();

                throw new NotImplementedException();
            }
        }

        // Fires the DoorClosed Event; called only at the end of a closing animation
        private void CloseDoor()
        {
            if (this.DoorClosed != null)
            {
                this.DoorClosed(this, new EventArgs());
            }
        }
    }
}
