using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

using BrightBudget.API.Common;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BrightBudget.API.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var apiResponse = ApiResponse<string>.Fail($"An unexpected error occurred.");
            apiResponse.Message += $"[{exception.Message}]";
            var payload = JsonSerializer.Serialize(apiResponse);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(payload);
        }
    }
}