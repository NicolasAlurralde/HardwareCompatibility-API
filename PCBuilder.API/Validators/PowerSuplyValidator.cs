using FluentValidation;
using PCBuilder.Domain.Entities;

namespace PCBuilder.API.Validators
{
    public class PowerSupplyValidator : AbstractValidator<PowerSupply>
    {
        public PowerSupplyValidator()
        {
            // Validamos la propiedad 'Wattage' (fijate que en la entidad es Wattage, no Watts)
            RuleFor(p => p.Wattage)
                .InclusiveBetween(200, 2000).WithMessage("La potencia debe ser razonable (200W a 2000W).");

            // Validamos la propiedad 'Certification' que es la que existe en tu clase
            RuleFor(p => p.Certification)
                .IsInEnum().WithMessage("Debes seleccionar una certificación de eficiencia válida(Generic,PlusWhite, PlusSilver, PlusGold, PlusPlatinum, PlusTitanium)");

            RuleFor(p => p.Price).GreaterThan(0);
            RuleFor(p => p.LengthMm).GreaterThan(0);

            RuleFor(p => p.FormFactor)
    .IsInEnum().WithMessage("Debes seleccionar un formato de fuente válido (ej. ATX, SFX).");

            RuleFor(p => p.Modularity)
                .IsInEnum().WithMessage("Debes seleccionar un tipo de modularidad válido (ej. FullyModular).");
        }
    }
}
