using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Application.DTOs
{
    public record ChangeTaskStatusDto(TaskItemStatus Status);
    public record TaskDto(int Id, string Title, string? Description, TaskItemStatus Status, int Priority, DateTime CreatedAt, DateTime UpdatedAt);
    public record CreateTaskDto(string Title, string? Description, int? Priority = 0);
    public record UpdateTaskDto(string Title, string? Description, TaskItemStatus Status, int Priority);
    public record TaskFilterDto(TaskItemStatus? Status, int? Priority, int PageNumber = 1);
}
