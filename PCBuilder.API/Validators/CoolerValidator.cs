using FluentValidation;
using PCBuilder.Domain.Entities;

namespace PCBuilder.API.Validators
{
    public class CoolerValidator : AbstractValidator<Cooler>
    {
        public CoolerValidator()
        {
            RuleFor(c => c.Model).NotEmpty().MinimumLength(3);
            RuleFor(c => c.Price).GreaterThan(0);

            RuleFor(c => c.Type) // Tu Enum de tipo de cooler (Air, Liquid, etc.)
                .IsInEnum().WithMessage("Debes seleccionar un tipo de cooler válido (Air, Liquid).");
        }
    }
}
