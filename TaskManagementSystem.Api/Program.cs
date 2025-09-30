using TaskManagementSystem.Api.Extensions;
using TaskManagementSystem.Api.Middleware;
using TaskManagementSystem.Application;
using TaskManagementSystem.Infrastructure;

namespace TaskManagementSystem.Api
{
    public class Program
    {
        public static bool useMigration = true;
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddTaskDbContext(connectionString);

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddApp();
            builder.Services.OverrideErrorModelStateResponse();
            

            var app = builder.Build();

            app.UseMiddleware<ErrorHandlingMiddleware>();

            if (useMigration)
                app.Services.MigrateDatabase(context => context.SeedExample());
            

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}