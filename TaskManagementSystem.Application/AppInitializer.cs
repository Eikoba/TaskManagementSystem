using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Application.DTOs.Validators;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Application.Services;

namespace TaskManagementSystem.Application
{
    public static class AppInitializer
    {
        public static void AddApp(this IServiceCollection Services)
        {
            Services.AddScoped<ITaskService, TaskService>();
            Services.AddMemoryCache();

            Services.AddScoped<IValidationService, ValidationService>();

            Services.AddTransient<IValidator, CreateTaskDtoValidator>();
            Services.AddTransient<IValidator, UpdateTaskDtoValidator>();
        }

        public static List<IValidator> GetValidator()
        {
            return new List<IValidator>() {
                new CreateTaskDtoValidator(),
                new UpdateTaskDtoValidator(),
            };
        }
    }
}
