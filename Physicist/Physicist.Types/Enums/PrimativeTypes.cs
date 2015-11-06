namespace Physicist.Types.Enums
{
    using System;

    public enum PrimitiveType
    {
        @int,
        @float,
        @string,
        @bool,
        @char,
    }

    public sealed class PrimitiveTypesHelper
    {
        private PrimitiveTypesHelper() 
        {
        }

        public static Type ToSystemType(PrimitiveType type)
        {
            Type parsedType = null;
            switch (type)
            {
                case PrimitiveType.@bool:
                    parsedType = typeof(bool);
                    break;

                case PrimitiveType.@float:
                    parsedType = typeof(float);
                    break;

                case PrimitiveType.@int:
                    parsedType = typeof(int);
                    break;

                case PrimitiveType.@char:
                    parsedType = typeof(char);
                    break;

                case PrimitiveType.@string:
                    parsedType = typeof(string);
                    break;
            }

            return parsedType;
        }
    }
}
