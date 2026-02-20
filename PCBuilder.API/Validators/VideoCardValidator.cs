using FluentValidation;
using PCBuilder.Domain.Entities;

namespace PCBuilder.API.Validators
{
    public class VideoCardValidator : AbstractValidator<VideoCard>
    {
        public VideoCardValidator()
        {
            RuleFor(v => v.Model).NotEmpty().MinimumLength(3);
            RuleFor(v => v.Price).GreaterThan(0);

            RuleFor(v => v.VramGb)
                .GreaterThan(0).WithMessage("La memoria VRAM debe ser mayor a 0 GB.");

            RuleFor(v => v.LengthMm)
                .GreaterThan(0).WithMessage("El largo de la placa de video debe ser válido (para calcular si entra en el gabinete).");

            RuleFor(v => v.PcieGeneration)
    .IsInEnum().WithMessage("Debes seleccionar una generación de PCIe válida (ej. Gen3,Gen4,Gen5).");
        }
    }
}
