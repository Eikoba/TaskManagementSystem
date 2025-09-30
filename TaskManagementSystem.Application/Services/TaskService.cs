using Microsoft.Extensions.Logging;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Application.Mappers;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Application.Services
{
    public class TaskService : ITaskService
    {
        private static readonly IEnumerable<TaskDto> _noItem = new List<TaskDto>();

        private readonly ITaskRepository _repository;
        private readonly ILogger<TaskService> _logger;
        private readonly IValidationService _validationService;

        public TaskService(ITaskRepository repository, ILogger<TaskService> logger, IValidationService validationService)
        {
            _repository = repository;
            _logger = logger;
            _validationService = validationService;
        }

        public async Task<PartTaskResultDTO<TaskDto>> GetPartTasks(int countItems, TaskFilterDto? filter = null)
        {
            if (filter != null)
                _validationService.Validate(filter);

            var tasks = await _repository.GetPartAsync(countItems, filter);

            return new PartTaskResultDTO<TaskDto>()
            {
                Items = tasks.Items.Select(x => x.ToDto()),
                PageNumber = tasks.PageNumber,
                PageSize = tasks.PageSize,
                TotalCount = tasks.TotalCount,
            };
        }

        public async Task<TaskDto?> GetByIdAsync(int id)
        {
            _validationService.ValidateId(id);

            var task = await _repository.GetByIdAsync(id);


            return task == null ? null : task.ToDto();
        }

        public async Task<TaskDto> CreateAsync(CreateTaskDto dto)
        {
            _validationService.Validate(dto);

            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                Priority = dto.Priority ?? 0,
                Status = TaskItemStatus.Open,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            var created = await _repository.AddAsync(task);
            return task.ToDto();
        }

        public async Task<bool> UpdateAsync(int id, UpdateTaskDto dto)
        {
            _validationService.ValidateId(id);
            _validationService.Validate(dto);

            var task = await _repository.GetByIdAsync(id);
            if (task == null) return false;

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.Priority = dto.Priority;
            task.Status = dto.Status;
            task.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(task);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _validationService.ValidateId(id);

            var task = await _repository.GetByIdAsync(id);
            if (task == null) return false;

            await _repository.DeleteAsync(task);
            return true;
        }

        public async Task<bool> ChangeStatusAsync(int id, TaskItemStatus taskStatus)
        {
            _validationService.ValidateId(id);

            var task = await _repository.GetByIdAsync(id);
            if (task == null) return false;

            task.Status = taskStatus;
            task.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(task);
            return true;
        }
    }
}