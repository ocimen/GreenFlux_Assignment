using System;
using System.Net;
using System.Threading.Tasks;
using GreenFlux.Api.Models;
using GreenFlux.Persistence.Exceptions;
using GreenFlux.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GreenFlux.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly ISuggestionService _suggestionService;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, ISuggestionService suggestionService)
        {
            _next = next;
            _logger = logger;
            _suggestionService = suggestionService;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (GroupCapacityExceedsException exception)
            {
                _logger.LogError("Group Capacity exceeds");
                await HandleGroupCapacityExceedsException(httpContext, exception);
            }
            catch (EntityNotFoundException exception)
            {
                _logger.LogError("Group Capacity exceeds");
                await HandleEntityNotFoundException(httpContext, exception);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex.Message}");
                await HandleGlobalExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleGlobalExceptionAsync(HttpContext context, Exception exception)
        {
            var errorDetail = new ErrorDetail();
            if (exception is HttpException httpException)
            {
                errorDetail.StatusCode = httpException.StatusCode;
                errorDetail.ErrorMessage = httpException.Message;
            }

            if (exception is GreenFluxException greenFluxException)
            {
                errorDetail.StatusCode = HttpStatusCode.InternalServerError;
                errorDetail.ErrorMessage = greenFluxException.Message;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var json = JsonConvert.SerializeObject(errorDetail);
            await context.Response.WriteAsync(json);
        }

        private async Task HandleGroupCapacityExceedsException(HttpContext context, GroupCapacityExceedsException exception)
        {
            var message = $"Group Capacity Exceeds. You can remove these connectors or increase the group capacity";
            var suggestions = await _suggestionService.SuggestRemovalConnectors(exception.GroupId, exception.Connector, exception.Diff);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var json = JsonConvert.SerializeObject(new { message, suggestions});
            await context.Response.WriteAsync(json);
        }

        private async Task HandleEntityNotFoundException(HttpContext context, EntityNotFoundException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
        }
    }
}