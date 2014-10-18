namespace Physicist.Extensions
{
    using Microsoft.Xna.Framework;

    public struct Size 
    {
        private int width;
        private int height;

        public Size(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public int Width
        {
            get
            {
                return this.width;
            }

            set
            {
                this.width = value;
            }
        }

        public int Height
        {
            get
            {
                return this.height;
            }

            set
            {
                this.height = value;
            }
        }

        public static bool operator ==(Size size1, Size size2)
        {
            return size1.Width == size2.Width && size1.Height == size2.Height;
        }

        public static bool operator !=(Size size1, Size size2)
        {
            return !(size1 == size2);
        }

        public static Vector2 ToVector2(Size size)
        {
            return (Vector2)size;
        }

        public static implicit operator Vector2(Size size)
        {
            return new Vector2(size.Width, size.Height);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
