using FluentValidation;
using Spg.Fachtheorie.Aufgabe2.DTOs;

namespace Spg.Fachtheorie.Aufgabe3.API.Validators
{
    public class CreateTopicDtoValidator : AbstractValidator<CreateTopicDto>
    {
        public CreateTopicDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Seats).GreaterThan(0);
        }
    }
}
