namespace Physicist.Actors.NPCs
{
    using System.Xml.Linq;
    using Physicist.Controls;
    using Physicist.Extensions;

    public class Enemy : NPC, IDamage
    {
        public Enemy()
        {
        }

        public Enemy(string name)
            : base(name)
        {
        }

        public int AttackDamage { get; set; }

        public float MaxSpeed { get; set; }

        public override XElement XmlSerialize()
        {
            return new XElement(
                                "Enemy", 
                                new XAttribute("maxSpeed", this.MaxSpeed),
                                new XAttribute("attackDamage", this.AttackDamage),
                                base.XmlSerialize());
        }

        public override void XmlDeserialize(XElement element)
        {
            if (element != null)
            {
                this.AttackDamage = element.GetAttribute("attackDamage", 0);
                this.MaxSpeed = element.GetAttribute("maxSpeed", 10);
                base.XmlDeserialize(element.Element("NPC"));

                this.Body.CollidesWith = PhysicistCategory.All ^ PhysicistCategory.Field;
                this.Body.CollisionCategories = PhysicistCategory.Enemy1;
            }
        }
    }
}