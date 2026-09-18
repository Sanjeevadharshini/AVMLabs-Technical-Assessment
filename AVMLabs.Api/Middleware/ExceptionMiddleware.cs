using AVMLabs.Api.DTOs.Common;
using AVMLabs.Api.Exceptions;
using AVMLabs.Api.Logging;
using System.Net;
using System.Text.Json;

namespace AVMLabs.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, ErrorLogWriter errorLogWriter)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.NotFound);
            }
            catch (BusinessException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred.");

                await errorLogWriter.WriteAsync(ex, context.Request.Path);

                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError, "Something went wrong. Please try again later.");
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode statusCode, string? message = null)
        {
            if (context.Response.HasStarted)
                return;

            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            var response = ApiResponse<object>.FailureResponse(message ?? exception.Message);

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }));
        }
    }
}