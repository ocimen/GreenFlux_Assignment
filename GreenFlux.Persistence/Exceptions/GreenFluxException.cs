using System;
using System.Runtime.Serialization;

namespace GreenFlux.Persistence.Exceptions
{
    [Serializable]
    public class GreenFluxException : Exception
    {
        public GreenFluxException()
        {

        }

        public GreenFluxException(string message)
            : base(message)
        {

        }

        protected GreenFluxException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
