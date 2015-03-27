namespace Physicist.Controls
{
    using FarseerPhysics.Dynamics;

    public interface IMapObject : IXmlSerializable, IDraw, IBody
    {
        string TextureReference { get; }
    }
}
