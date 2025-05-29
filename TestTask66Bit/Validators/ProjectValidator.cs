using FluentValidation;
using TestTask66Bit.ViewModels.Request;

namespace TestTask66Bit.Validators
{
    public class CreateProjectValidator : AbstractValidator<CreateProjectDto>
    {
        public CreateProjectValidator()
        {
            RuleFor(p => p.Name).NotEmpty();
        }
    }

    public class UpdateProjectValidator : AbstractValidator<UpdateProjectDto>
    {
        public UpdateProjectValidator()
        {
            RuleFor(p => p.Name).NotEmpty();
        }
    }
}
