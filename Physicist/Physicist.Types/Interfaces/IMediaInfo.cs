namespace Physicist.Types.Interfaces
{
    using Physicist.Types.Enums;

    public interface IMediaInfo
    {
        MediaFormat Format { get; }

        string Location { get; }

        string Name { get; }
    }
}
