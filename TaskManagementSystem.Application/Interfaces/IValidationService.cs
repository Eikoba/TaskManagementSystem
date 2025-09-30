namespace TaskManagementSystem.Application.Interfaces
{
    public interface IValidationService
    {
        void ValidateId(int value);
        void Validate<T>(T dto) where T : class;
    }
}
