using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Infrastructure.Data;
using TaskManagementSystem.Infrastructure.Repositories;

namespace TaskManagementSystem.Infrastructure
{
    public static class DbInitializer
    {
        public static void AddTaskDbContext(this IServiceCollection Services, string connectionString)
        {
            Services.AddDbContext<TaskDbContext>(options =>
                options.UseSqlServer(connectionString));

            Services.AddScoped<ITaskRepository, EfTaskRepository>();
        }

        public static void MigrateDatabase(this IServiceProvider Services, Action<TaskDbContext>? db = null)
        {
            using (var scope = Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TaskDbContext>();
                
                context.Database.Migrate();
                db?.Invoke(context);
                
            }
        }

        public static void SeedExample(this TaskDbContext context)
        {
            if (context.Tasks.Any()) return;
            var random = new Random();
            
            for (int i = 1; i < 30; i++)
            {    
                context.Add(new TaskItem
                {
                    Title = $"{i}-ый таск",
                    Description = $"Описание {i}-ой задачи",
                    Status = (TaskItemStatus)random.Next(0, 3),
                    Priority = random.Next(0,3),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            };

            context.SaveChanges();
        }
    }
}