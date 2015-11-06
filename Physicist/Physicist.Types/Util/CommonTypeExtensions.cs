namespace Physicist.Types.Util
{
    using System.Xml.Linq;
    using System;
    using System.Globalization;
    using Microsoft.Xna.Framework;
    using Physicist.Types.Common;

    public static class CommonTypeExtensions
    {
        public static XElement XmlSerialize(this Size size, string tag)
        {
            return new XElement(
                tag,
                new XAttribute("width", size.Width),
                new XAttribute("height", size.Height));
        }

        public static XElement XmlSerialize(this SpriteAnimation animation, string tag)
        {
            return new XElement(
                tag,
                new XAttribute("rowIndex", animation.RowIndex),
                new XAttribute("frameCount", animation.FrameCount),
                new XAttribute("defaultFrameRate", animation.DefaultFrameRate),
                new XAttribute("playInReverse", animation.PlayInReverse),
                new XAttribute("flipVertical", animation.FlipVertical),
                new XAttribute("flipHorizontal", animation.FlipHorizontal),
                new XAttribute("loopAnimation", animation.LoopAnimation));
        }
    }
}
