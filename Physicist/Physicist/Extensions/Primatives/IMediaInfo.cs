namespace Physicist.Extensions.Primitives
{
    using System;
    using Physicist.Enums;

    public interface IMediaInfo
    {
        MediaFormat Format { get; }

        string Location { get; }

        string Name { get; }
    }
}
