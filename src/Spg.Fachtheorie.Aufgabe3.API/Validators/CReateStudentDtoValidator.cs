using FluentValidation;
using Spg.Fachtheorie.Aufgabe2.DTOs;

namespace Spg.Fachtheorie.Aufgabe3.API.Validators
{
    public class CreateStudentDtoValidator : AbstractValidator<CreateStudentDto>
    {
        public CreateStudentDtoValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        }
    }
}
