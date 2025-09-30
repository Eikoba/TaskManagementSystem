namespace TaskManagementSystem.Application.DTOs
{
    public class ResponseExceptionDto
    {
        public string Message { get; set; } = string.Empty;
        public IEnumerable<object>? Details { get; set; }
    }
}