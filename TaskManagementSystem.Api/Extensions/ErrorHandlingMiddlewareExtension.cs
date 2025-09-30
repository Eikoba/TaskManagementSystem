using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Application.Exceptions;

namespace TaskManagementSystem.Api.Extensions
{
    public static class ErrorHandlingMiddlewareExtension
    {
        public static void OverrideErrorModelStateResponse(this IServiceCollection Services)
        {
            Services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var errors = context.ModelState
                            .Where(ms => ms.Value?.Errors.Count > 0)
                            .SelectMany(ms => ms.Value!.Errors.Select(err => new
                            {
                                PropertyName = ms.Key,
                                err.ErrorMessage
                            }))
                            .ToList();

                        var errorResponse = new ErrorResponse
                        {
                            error = "Validation failed",
                            details = errors
                        };

                        return new BadRequestObjectResult(errorResponse);
                    };
                });
        }
    }
}
