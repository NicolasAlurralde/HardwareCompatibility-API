using FluentValidation;
using PCBuilder.Domain.Entities;

namespace PCBuilder.API.Validators
{
    public class RamValidator : AbstractValidator<Ram>
    {
        public RamValidator()
        {
            RuleFor(r => r.CapacityGb)
                .InclusiveBetween(1, 128).WithMessage("La capacidad debe estar entre 1 y 128 GB.");

            RuleFor(r => r.SpeedMhz)
                .GreaterThan(800).WithMessage("La frecuencia mínima soportada es 800 MHz.");

            RuleFor(r => r.ModulesCount)
                .GreaterThan(0).WithMessage("Debe haber 1 o mas modulos de ram");

            RuleFor(r => r.Price).GreaterThan(0);
           
            RuleFor(r => r.Type) // Cambiá 'Type' por el nombre real de tu propiedad en la clase Ram
                .IsInEnum().WithMessage("Debes ingresar un tipo de memoria válido (ej. DDR3, DDR4, DDR5).");
        }
    }
}
