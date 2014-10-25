namespace Physicist.Controls
{
    using Physicist.Enums;

    public interface IMediaInfo
    {
        MediaFormat Format { get; }

        string Location { get; }

        string Name { get; }
    }
}
