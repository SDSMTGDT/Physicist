using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Physicist.Actors.Fields
{
    class AccelerationField : IField, Actor
    {
        public float dampenRate; //the rate at which to dampen player velocity when they enter the field. Negative number instantly removes all velocity
    }
}
