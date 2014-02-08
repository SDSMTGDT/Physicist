namespace Physicist.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Physicist.Extensions;

    public class Viewport
    {
        // for now, just holds the width and height of the viewport.
        // may in the future interact with the graphcis device or attempt to get viewport information based on the game or the screen
        public Viewport(Size viewportSize)
        {
            this.ViewportSize = viewportSize;
        }

        public Size ViewportSize { get; set; }
    }
}
