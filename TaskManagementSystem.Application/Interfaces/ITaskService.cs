using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Application.Interfaces
{
    public interface ITaskService
    {
        Task<PartTaskResultDTO<TaskDto>> GetPartTasks(int countItems, TaskFilterDto? filter = null);
        Task<TaskDto?> GetByIdAsync(int id);
        Task<TaskDto> CreateAsync(CreateTaskDto dto);
        Task<bool> UpdateAsync(int id, UpdateTaskDto dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ChangeStatusAsync(int id, TaskItemStatus taskStatus);
    }
}