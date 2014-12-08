using System;
using System.Runtime.Serialization;

namespace Vortex.Database.Repository
{
    [Serializable]
    public class SessionClosedException : Exception
    {
        public SessionClosedException()
        {
        }

        public SessionClosedException(string message)
            : base(message)
        {
        }

        public SessionClosedException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected SessionClosedException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}
