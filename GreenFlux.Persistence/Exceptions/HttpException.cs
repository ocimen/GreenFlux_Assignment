using System;
using System.Net;
using System.Runtime.Serialization;

namespace GreenFlux.Persistence.Exceptions
{
    [Serializable]
    public class HttpException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public HttpException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        protected HttpException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
