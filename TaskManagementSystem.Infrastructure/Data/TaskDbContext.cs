using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Infrastructure.Data
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options) { }

        public DbSet<TaskItem> Tasks { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Title)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(t => t.Description)
                      .HasMaxLength(500);

                entity.Property(t => t.Status)
                      .IsRequired();

                entity.HasIndex(e => e.Status)
                      .HasDatabaseName("IX_Tasks_Status");

                entity.Property(t => t.Priority)
                      .IsRequired();

                entity.HasIndex(e => e.Priority)
                      .HasDatabaseName("IX_Tasks_Priority");

                entity.HasIndex(e => new { e.Status, e.Priority })
                      .HasDatabaseName("IX_Tasks_Status_Priority");

                entity.Property(t => t.CreatedAt)
                      .IsRequired();

                entity.Property(t => t.UpdatedAt)
                      .IsRequired();
            });
        }
    }
}
