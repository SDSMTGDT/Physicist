namespace Physicist.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;

    [Serializable]
    public class PhysicistSoundControllerException : Exception
    {
        public PhysicistSoundControllerException()
            : base()
        {
        }

        public PhysicistSoundControllerException(string message)
            : base(message)
        {
        }

        public PhysicistSoundControllerException(string message, Exception exception)
            : base(message, exception)
        {
        }

        protected PhysicistSoundControllerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
