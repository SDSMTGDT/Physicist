namespace Physicist.MainGame.Extensions
{
    using FarseerPhysics.Dynamics;

    public static class PhysicistCategory
    {
        public static Category Player1
        {
            get
            {
                return Category.Cat1;
            }
        }

        public static Category Player2
        {
            get
            {
                return Category.Cat2;
            }
        }
        
        public static Category Map1
        { 
            get
            {
                return Category.Cat3;
            }
        }
        
        public static Category Map2
        {
            get
            {
                return Category.Cat4;
            }
        }
        
        public static Category Field
        {
            get
            {
                return Category.Cat5;
            }
        }
        
        public static Category Environment1
        {
            get
            {
                return Category.Cat6;
            }
        }
        
        public static Category Environment2
        {
            get
            {
                return Category.Cat7;
            }
        }
        
        public static Category Environment3
        {
            get
            {
                return Category.Cat8;
            }
        }
        
        public static Category NPC1
        {
            get
            {
                return Category.Cat9;
            }
        }
        
        public static Category NPC2
        {
            get
            {
                return Category.Cat10;
            }
        }

        public static Category NPC3
        {
            get
            {
                return Category.Cat11;
            }
        }

        public static Category Enemy1
        {
            get
            {
                return Category.Cat12;
            }
        }

        public static Category Enemy2
        {
            get
            {
                return Category.Cat13;
            }
        }

        public static Category Enemy3
        {
            get
            {
                return Category.Cat14;
            }
        }

        public static Category Dynamic
        {
            get
            {
                return PhysicistCategory.Dynamic1 | PhysicistCategory.Dynamic2 | PhysicistCategory.Dynamic3;
            }
        }

        public static Category Dynamic1
        {
            get
            {
                return Category.Cat15;
            }
        }

        public static Category Dynamic2
        {
            get
            {
                return Category.Cat16;
            }
        }

        public static Category Dynamic3
        {
            get
            {
                return Category.Cat17;
            }
        }

        public static Category Item1
        {
            get
            {
                return Category.Cat18;
            }
        }

        public static Category Item2
        {
            get
            {
                return Category.Cat19;
            }
        }

        public static Category Item3
        {
            get
            {
                return Category.Cat20;
            }
        }

        public static Category All
        {
            get
            {
                return Category.All;
            }
        }

        public static Category Player
        {
            get
            {
                return Player1 | Player2;
            }
        }

        public static Category Map
        {
            get
            {
                return Map1 | Map2;
            }
        }

        public static Category Environment
        {
            get
            {
                return Environment1 | Environment2 | Environment3;
            }
        }

        public static Category Item
        {
            get
            {
                return Item1 | Item2 | Item3;
            }
        }

        public static Category NPC
        {
            get
            {
                return NPC1 | NPC2 | NPC3;
            }
        }

        public static Category Enemy
        {
            get
            {
                return Enemy1 | Enemy2 | Enemy3;
            }
        }

        public static Category Physical
        {
            get
            {
                return All ^ Field ^ Environment1 ^ Environment2 ^ Environment3;
            }
        }

        public static Category AllIgnoreFields
        {
            get
            {
                return PhysicistCategory.All ^ PhysicistCategory.Field;
            }
        }

        public static Category None
        {
            get
            {
                return Category.None;
            }
        }
    }
}
