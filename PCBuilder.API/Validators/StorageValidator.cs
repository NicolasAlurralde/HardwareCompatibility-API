using FluentValidation;
using PCBuilder.Domain.Entities;

namespace PCBuilder.API.Validators
{
    public class StorageValidator : AbstractValidator<Storage>
    {
        public StorageValidator()
        {
            RuleFor(s => s.Model).NotEmpty().MinimumLength(3);
            RuleFor(s => s.Price).GreaterThan(0);

            RuleFor(s => s.CapacityGb)
                .GreaterThan(0).WithMessage("La capacidad de almacenamiento debe ser mayor a 0 GB.");

            RuleFor(s => s.ReadSpeedMb)
                .GreaterThan(0).WithMessage("La velocidad de lectura debe ser mayor a 0 MB.");

            RuleFor(s => s.InterfaceType) // Tu Enum de tipo de disco (HDD, SSD, NVMe, etc.)
                .IsInEnum().WithMessage("Debes seleccionar un tipo de almacenamiento válido.");

         
        }
    }
}
