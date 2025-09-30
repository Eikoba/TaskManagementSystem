using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TaskManagementSystem.Application;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Application.DTOs.Validators;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Application.Services;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Tests
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _taskRepositoryMock;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            var Services = new ServiceCollection();

            var logger = NullLogger<TaskService>.Instance;
            Services.AddTransient<IValidator<CreateTaskDto>, CreateTaskDtoValidator>();
            Services.AddTransient<IValidator<UpdateTaskDto>, UpdateTaskDtoValidator>();
            IValidationService validator = new ValidationService(AppInitializer.GetValidator());
            _taskRepositoryMock = new Mock<ITaskRepository>();


            _taskService = new TaskService(_taskRepositoryMock.Object, logger, validator);
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedTask()
        {
            var dto = new CreateTaskDto("New Task", "Desc", 1);

            _taskRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<TaskItem>()))
                .Callback<TaskItem>(t => t.Id = 123)
                .Returns<TaskItem>(t => Task.FromResult(t));

            var result = await _taskService.CreateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("New Task", result.Title);
            Assert.Equal("Desc", result.Description);
            Assert.Equal(1, result.Priority);
            Assert.Equal(TaskItemStatus.Open, result.Status);
        }

        [Fact]
        public async Task ChangeStatusAsync_ShouldUpdateStatusAndUpdatedAt()
        {
            var date = DateTime.UtcNow;
            var existingTask = new TaskItem
            {
                Id = 1,
                Title = "Test",
                Status = TaskItemStatus.Open,
                Priority = 0,
                CreatedAt = date,
                UpdatedAt = date
            };

            _taskRepositoryMock.Setup(r => r.GetByIdAsync(1))
                               .ReturnsAsync(existingTask);

            _taskRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<TaskItem>()))
                               .Callback<TaskItem>(t =>
                               {
                                   existingTask.Priority = t.Priority;
                                   existingTask.Title = t.Title;
                                   existingTask.Description = t.Description;
                                   existingTask.Status = t.Status;
                                   existingTask.UpdatedAt = t.UpdatedAt;
                               })
                               .Returns(Task.CompletedTask);

            var result = await _taskService.ChangeStatusAsync(1, TaskItemStatus.Done);

            Assert.True(result);
            Assert.Equal(TaskItemStatus.Done, existingTask.Status);
            Assert.True(existingTask.UpdatedAt > date, "updatedAt should be longer than the original time");
        }

        [Fact]
        public async Task UpdateAsync_Should_UpdateTaskAndUpdatedAt()
        {
            var date = DateTime.UtcNow;
            var existingTask = new TaskItem
            {
                Id = 1,
                Title = "Old Title",
                Description = "Old Desc",
                Status = TaskItemStatus.Open,
                Priority = 0,
                CreatedAt = date,
                UpdatedAt = date
            };

            _taskRepositoryMock.Setup(r => r.GetByIdAsync(existingTask.Id))
                    .ReturnsAsync(existingTask);

            _taskRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<TaskItem>()))
                    .Callback<TaskItem>(t =>
                    {
                        existingTask.Title = t.Title;
                        existingTask.Description = t.Description;
                        existingTask.Status = t.Status;
                        existingTask.Priority = t.Priority;
                        existingTask.UpdatedAt = t.UpdatedAt;
                    })
                    .Returns(Task.CompletedTask);

            var service = _taskService;

            var dto = new UpdateTaskDto(
                Title: "Updated Title",
                Description: "Updated Desc",
                Status: TaskItemStatus.InProgress,
                Priority: 1
            );

            var result = await service.UpdateAsync(existingTask.Id, dto);

            Assert.True(result);
            Assert.Equal("Updated Title", existingTask.Title);
            Assert.Equal("Updated Desc", existingTask.Description);
            Assert.Equal(TaskItemStatus.InProgress, existingTask.Status);
            Assert.Equal(1, existingTask.Priority);
            Assert.True(existingTask.UpdatedAt > date, "updatedAt should be longer than the original time");
        }
    }
}