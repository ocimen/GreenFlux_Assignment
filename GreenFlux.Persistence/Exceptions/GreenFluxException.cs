using System;

namespace GreenFlux.Persistence.Exceptions
{
    public class GreenFluxException : Exception
    {
        public GreenFluxException()
        {

        }

        public GreenFluxException(string message)
            : base(message)
        {

        }
    }
}
