namespace Physicist.MainGame.Controls
{
    using Physicist.Types.Interfaces;

    public interface IMapObject : IXmlSerializable, IDraw, IBody
    {
        string TextureReference { get; }
    }
}
