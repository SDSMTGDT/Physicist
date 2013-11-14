namespace Physicist.Actors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Physicist;

    public class Door : Actor
    {
        // MEMBERS
        private bool isEndDoor;

        // METHODS
        public Door(bool isEndDoor = false)
        {
            this.isEndDoor = isEndDoor;
        }

        // event when the door is closed, either for ending the level (isEndDoor) or ending the animation (!isEndDoor)
        public event EventHandler DoorClosed;

        // if the player can enter the door on a key press
        public bool IsPlayerValidEntry(Player p)
        {
            return false;
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
