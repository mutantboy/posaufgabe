using FluentValidation;
using Spg.Fachtheorie.Aufgabe2.DTOs;

namespace Spg.Fachtheorie.Aufgabe3.API.Validators
{
    public class EnrollStudentDtoValidator : AbstractValidator<EnrollStudentDto>
    {
        public EnrollStudentDtoValidator()
        {
            RuleFor(x => x.StudentId).GreaterThan(0);
        }
    }
}
