using FluentValidation;
using StudentApp.Models;
namespace StudentApp.Validators
{
    public class PetitionReviewDtoValidator : AbstractValidator<PetitionReviewDto>
    {
        public PetitionReviewDtoValidator()
        {
            RuleFor(x => x.Comment).NotEmpty();
        }
    }
}
