using System;
using System.Runtime.Serialization;

namespace GreenFlux.Persistence.Exceptions
{
    [Serializable]
    public class MaxConnectorExceedsException : GreenFluxException
    {
        public MaxConnectorExceedsException(string message) : base(message)
        {
            
        }

        protected MaxConnectorExceedsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
