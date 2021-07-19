using System;
using System.Runtime.Serialization;

namespace GreenFlux.Persistence.Exceptions
{
    [Serializable]
    public class EntityNotFoundException : GreenFluxException
    {
        public EntityNotFoundException(string message) : base(message)
        {
        }

        protected EntityNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            
        }
    }
}
