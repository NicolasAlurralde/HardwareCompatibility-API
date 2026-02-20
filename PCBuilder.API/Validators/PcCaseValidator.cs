using FluentValidation;
using PCBuilder.Domain.Entities;

namespace PCBuilder.API.Validators
{
    public class PcCaseValidator : AbstractValidator<PcCase>
    {
        public PcCaseValidator()
        {
            // 1. Reglas base del Componente
            RuleFor(c => c.Model).NotEmpty().WithMessage("El modelo del gabinete es obligatorio.").MinimumLength(3);
            RuleFor(c => c.Price).GreaterThan(0).WithMessage("El precio debe ser mayor a 0.");

            // 2. Validaciones de Enums (Formatos soportados)
            RuleFor(c => c.MaxMotherboardSize)
                .IsInEnum().WithMessage("Debes seleccionar un formato de placa madre válido (ej. ATX, MicroATX).");

            RuleFor(c => c.SupportedPsuFormFactor)
                .IsInEnum().WithMessage("Debes seleccionar un formato de fuente soportado válido (ej. ATX, SFX).");

            // 3. Medidas físicas (Espacios internos)
            RuleFor(c => c.MaxGpuLengthMm)
                .GreaterThan(0).WithMessage("El espacio máximo para la tarjeta de video debe ser mayor a 0 mm.");

            RuleFor(c => c.MaxCpuCoolerHeightMm)
                .GreaterThan(0).WithMessage("El espacio máximo para el disipador del procesador debe ser mayor a 0 mm.");

            RuleFor(c => c.MaxPsuLengthMm)
                .GreaterThan(0).WithMessage("El espacio máximo para la fuente de poder debe ser mayor a 0 mm.");

            // 4. Lógica Condicional: Radiadores
            RuleFor(c => c.MaxRadiatorSizeMm)
                .GreaterThan(0)
                .When(c => c.SupportsLiquidCooling) // Solo se exige si la propiedad es 'true'
                .WithMessage("Si el gabinete soporta refrigeración líquida, debes especificar un tamaño de radiador mayor a 0 mm (ej. 240, 360).");
        }
    }
}
