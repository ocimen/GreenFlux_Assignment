using GreenFlux.Api.Middleware;
using Microsoft.AspNetCore.Builder;

namespace GreenFlux.Api.Extensions
{
    public static class ExceptionMiddlewareExtension
    {
        public static void UseGlobalExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
