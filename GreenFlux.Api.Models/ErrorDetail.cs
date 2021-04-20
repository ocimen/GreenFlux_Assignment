using System.Net;

namespace GreenFlux.Api.Models
{
    public class ErrorDetail
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.InternalServerError;

        public string ErrorMessage { get; set; } = "An unexpected error occured";
    }
}
