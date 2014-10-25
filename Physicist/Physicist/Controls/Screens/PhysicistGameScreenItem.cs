namespace Physicist.Controls
{
    using System.Xml.Linq;
    using FarseerPhysics.Dynamics;

    public abstract class PhysicistGameScreenItem : IPhysicistGameScreenItem, IXmlSerializable
    {
        private PhysicistGameScreen screen;

        public PhysicistGameScreen Screen
        {
            get
            {
                return this.screen;
            }

            set
            {
                this.screen = value;
                if (this.Screen != null)
                {
                    this.World = this.screen.World;
                    this.Map = this.screen.Map;
                }
            }
        }

        public World World { get; set; }

        public Map Map { get; set; }

        public abstract XElement XmlSerialize();

        public abstract void XmlDeserialize(XElement element);
    }
}
