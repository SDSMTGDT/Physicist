using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Physicist;

namespace Physicist.Actors
{
    class Door : Actor
    {
        //MEMBERS

        public Boolean isEndDoor;


        //METHODS

        public Door()
        {

        }

        //event when the door is closed, either for ending the level (isEndDoor) or ending the animation (!isEndDoor)
        public event EventHandler doorClosed
        {
            add
            {

            }

            remove
            {

            }
        }

        //if the player can enter the door on a key press
        public Boolean isPlayerValidEntry(Player p)
        {
            return false;
        }

    }
}
