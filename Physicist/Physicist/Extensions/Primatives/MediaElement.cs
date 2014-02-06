namespace Physicist.Extensions.Primitives
{
    using System;
    using Physicist.Enums;

    public class MediaElement : IMediaInfo
    {
        public MediaElement(string assetName, string assetLocation, object asset)
        {
            this.Name = assetName;
            this.Location = assetLocation;
            this.Asset = asset;
        }

        public MediaFormat Format { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public object Asset { get; set; }
    }
}
