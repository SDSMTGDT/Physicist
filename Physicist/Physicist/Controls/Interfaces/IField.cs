namespace Physicist.Controls
{
    using System.Collections.Generic;
    using FarseerPhysics.Dynamics;
    using Microsoft.Xna.Framework;
    using Physicist.Controls;

    public interface IField : IXmlSerializable, IBody
    {
        Vector2 FieldVector { get; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "dt", Justification = "Farseer Physics uses dt")]
        void ApplyField(float dt, Body controllerBody, Body worldBody);
    }
}
