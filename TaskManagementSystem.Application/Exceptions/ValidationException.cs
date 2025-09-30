namespace TaskManagementSystem.Application.Exceptions
{
    public class ErrorResponse : Exception
    {
        public string error { get; set; } = string.Empty;
        public IEnumerable<object>? details { get; set; }
    }
}