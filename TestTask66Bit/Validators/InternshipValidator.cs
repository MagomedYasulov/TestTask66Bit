using FluentValidation;
using TestTask66Bit.ViewModels.Request;

namespace TestTask66Bit.Validators
{
    public class CreateInternshipValidator : AbstractValidator<CreateInternshipDto>
    {
        public CreateInternshipValidator()
        {
            RuleFor(i => i.Name).NotEmpty();
        }
    }

    public class UpdateInternshipValidator : AbstractValidator<UpdateInternshipDto>
    {
        public UpdateInternshipValidator()
        {
            RuleFor(i => i.Name).NotEmpty();
        }
    }
}
