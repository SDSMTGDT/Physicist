namespace Physicist.MainGame.Controls
{
    using FarseerPhysics.Dynamics;
    using Microsoft.Xna.Framework;
    using Physicist.Types.Interfaces;

    public interface IField : IXmlSerializable, IBody
    {
        Vector2 FieldVector { get; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dt", Justification = "Farseer Physics uses dt")]
        void ApplyField(float dt, Body controllerBody, Body worldBody);
    }
}
