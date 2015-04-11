namespace Physicist.Events
{
    using System;

    public struct ActivationData
    {
        private object data;
        private string type;

        public ActivationData(object data, string type)
        {
            this.data = data;
            this.type = type;
        }

        public object Data 
        {
            get
            {
                return this.data;
            }
        }

        public string ActivationType
        {
            get
            {
                return this.type;
            }
        }

        public static bool operator ==(ActivationData data1, ActivationData data2)
        {
            return data1.type == data2.type && data1.data == data2.data;
        }

        public static bool operator !=(ActivationData data1, ActivationData data2)
        {
            return !(data1 == data2);
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
