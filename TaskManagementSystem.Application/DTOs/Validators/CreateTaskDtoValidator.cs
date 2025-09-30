using FluentValidation;

namespace TaskManagementSystem.Application.DTOs.Validators
{
    public class CreateTaskDtoValidator : AbstractValidator<CreateTaskDto>
    {
        public CreateTaskDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("The title cannot be empty")
                .MaximumLength(200).WithMessage("The title is too long");

            RuleFor(x => x.Priority)
                .GreaterThanOrEqualTo(0).WithMessage("Priority must be >= 0");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("The description is too long");
        }
    }
}
