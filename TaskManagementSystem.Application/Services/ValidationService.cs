using FluentValidation;
using TaskManagementSystem.Application.Interfaces;

namespace TaskManagementSystem.Application.Services
{
    public class ValidationService : IValidationService
    {

        public void ValidateId(int id)
        {
            if (id < 0)
                throw new ValidationException("The id cannot be less than zero");
        }

        private readonly IEnumerable<IValidator> _validators;

        public ValidationService(IEnumerable<IValidator> validators)
        {
            _validators = validators;
        }

        public void Validate<T>(T instance) where T : class
        {
            var validators = _validators.OfType<IValidator<T>>().ToList();

            if (!validators.Any())
                return;

            foreach (var validator in validators)
            {
                var result = validator.Validate(instance);
                if (!result.IsValid)
                {

                    var errors = result.Errors;

                    throw new ValidationException(result.Errors);
                }
            }
        }
    }
}
