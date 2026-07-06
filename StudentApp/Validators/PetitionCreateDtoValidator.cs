using FluentValidation;
using StudentApp.Models;
namespace StudentApp.Validators
{
    public class PetitionCreateDtoValidator : AbstractValidator<PetitionCreateDto>
    {
        public PetitionCreateDtoValidator()
        {
            RuleFor(x => x.StudentId).GreaterThan(0);
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.PetitionType).IsInEnum();
        }
    }
}
