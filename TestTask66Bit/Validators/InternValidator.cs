using FluentValidation;
using TestTask66Bit.Extensions;
using TestTask66Bit.ViewModels.Request;

namespace TestTask66Bit.Validators
{
    public class CreateInternValidator : AbstractValidator<CreateInternDto>
    {
        public CreateInternValidator()
        {
            RuleFor(i => i.Name).NotEmpty();
            RuleFor(i => i.Surname).NotEmpty();
            RuleFor(i => i.Email).NotEmpty().EmailAddress();
            RuleFor(i => i.ProjectId).GreaterThan(0);
            RuleFor(i => i.InternshipId).GreaterThan(0);
            RuleFor(i => i.Gender).IsInEnum();
            RuleFor(i => i.BirthDate).NotNull();
            RuleFor(i => i.Phone).Phone().When(i => i.Phone != null).WithMessage("Invalid phone format.");
        }
    }

    public class UpdateInternValidator : AbstractValidator<UpdateInternDto>
    {
        public UpdateInternValidator()
        {
            RuleFor(i => i.Name).NotEmpty();
            RuleFor(i => i.Surname).NotEmpty();
            RuleFor(i => i.Email).NotEmpty().EmailAddress();
            RuleFor(i => i.ProjectId).GreaterThan(0);
            RuleFor(i => i.InternshipId).GreaterThan(0);
            RuleFor(i => i.Gender).IsInEnum();
            RuleFor(i => i.BirthDate).NotNull();
            RuleFor(i => i.Phone).Phone().When(i => i.Phone != null).WithMessage("Invalid phone format.");
        }
    }
}
