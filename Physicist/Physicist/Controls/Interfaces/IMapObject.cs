namespace Physicist.Controls
{
    public interface IMapObject : IXmlSerializable, IDraw
    {
        string TextureReference { get; }
    }
}
