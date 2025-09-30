using FluentValidation;
using System.Text.Json;
using TaskManagementSystem.Application.Exceptions;

namespace TaskManagementSystem.Api.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException vex)
            {
                _logger.LogWarning(vex, "Validation failed");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

                var validationErrors = vex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });

                var result = JsonSerializer.Serialize(new ErrorResponse()
                {
                    error = vex.Message,
                    details = validationErrors
                });

                await context.Response.WriteAsync(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                string result;
                if (_env.IsDevelopment())
                {

                    result = JsonSerializer.Serialize(new ErrorResponse()
                    {
                        error = ex.Message,
                        details = new List<object>(){ new
                        {
                            type = ex.GetType().Name,
                            stackTrace = ex.StackTrace
                        } }

                    });
                }
                else
                {

                    result = JsonSerializer.Serialize(new ErrorResponse()
                    {
                        error = "An unexpected error occurred."
                    });

                }
                await context.Response.WriteAsync(result);
            }
        }
    }
}
