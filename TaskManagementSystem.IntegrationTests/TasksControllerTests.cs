using System.Net.Http.Json;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.IntegrationTests.Factories;
using Xunit;

namespace TaskManagementSystem.IntegrationTests
{
    public class TasksControllerTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public TasksControllerTests(TestWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateTask_ShouldReturnCreatedTask()
        {
            var dto = new CreateTaskDto("Integration Test", "Test Task");


            var response = await _client.PostAsJsonAsync("/api/tasks", dto);

            response.EnsureSuccessStatusCode();
            var createdTask = await response.Content.ReadFromJsonAsync<TaskDto>();
            Assert.NotNull(createdTask);
            Assert.Equal(dto.Title, createdTask.Title);
        }
    }
}
