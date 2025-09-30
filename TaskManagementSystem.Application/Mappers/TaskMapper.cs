using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Application.Mappers
{
    public static class TaskMapper
    {
        public static TaskDto ToDto(this TaskItem task)
        {
            return new TaskDto(task.Id, task.Title, task.Description, task.Status, task.Priority, task.CreatedAt, task.UpdatedAt);
        }
    }
}