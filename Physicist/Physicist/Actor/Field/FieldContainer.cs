namespace Physicist.Actors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Physicist.Actors.Fields;

    public class FieldContainer : Actor
    {
        public Field ContainedField { get; set; }
    }
}
