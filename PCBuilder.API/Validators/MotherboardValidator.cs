using FluentValidation;
using PCBuilder.Domain.Entities;
using PCBuilder.Domain.Enums;

namespace PCBuilder.API.Validators
{
    public class MotherboardValidator : AbstractValidator<Motherboard>
    {
        public MotherboardValidator()
        {
            RuleFor(m => m.Model).NotEmpty().WithMessage("El modelo de la placa madre es obligatorio.");
            RuleFor(m => m.Price).GreaterThan(0).WithMessage("El precio debe ser mayor a 0.");
            RuleFor(m => m.MaxRamCapacityGb).GreaterThan(0).WithMessage("La capacidad de ram debe ser mayor a 0.");
            RuleFor(m => m.PcieX16Slots).GreaterThan(0).WithMessage("La cantidad de pcie debe ser mayor a 0.");
            // 1. Validar los Enums fundamentales (¡Lo que bien observaste!)
            RuleFor(m => m.Socket)
                .IsInEnum().WithMessage("Debes seleccionar un socket válido (ej. AM4, LGA1700).");

            RuleFor(m => m.SupportedRam) // Aunque lo usás en las reglas de abajo, primero hay que asegurar que exista
                .IsInEnum().WithMessage("Debes seleccionar un tipo de memoria RAM válido.");

            // 2. Proteger las ranuras físicas de números negativos
            RuleFor(m => m.RamSlots)
                .GreaterThan(0).WithMessage("La placa madre debe tener al menos 1 ranura para RAM.");

            RuleFor(m => m.M2Slots)
                .GreaterThanOrEqualTo(0).WithMessage("La cantidad de puertos M.2 no puede ser negativa.");

            RuleFor(m => m.SataSlots)
                .GreaterThanOrEqualTo(0).WithMessage("La cantidad de puertos SATA no puede ser negativa.");
            // Regla: Socket AM4 solo puede tener slots DDR4
            RuleFor(m => m.SupportedRam)
                .Must(ram => ram == RamType.DDR4)
                .When(m => m.Socket == SocketType.AM4)
                .WithMessage("Incompatibilidad física: Una placa AM4 solo puede tener ranuras DDR4.");

            // Regla: Socket AM5 solo puede tener slots DDR5
            RuleFor(m => m.SupportedRam)
                .Must(ram => ram == RamType.DDR5)
                .When(m => m.Socket == SocketType.AM5)
                .WithMessage("Incompatibilidad física: Una placa AM5 requiere ranuras DDR5.");

            // Regla: Sockets antiguos (AM3 / LGA 1150)
            RuleFor(m => m.SupportedRam)
                .Must(ram => ram == RamType.DDR3)
                .When(m => m.Socket == SocketType.AM3 || m.Socket == SocketType.LGA1150)
                .WithMessage("Incompatibilidad física: Esta placa de socket antiguo solo soporta DDR3.");

            // Regla: LGA 1200 (Intel 10ma/11va)
            RuleFor(m => m.SupportedRam)
                .Must(ram => ram == RamType.DDR4)
                .When(m => m.Socket == SocketType.LGA1200)
                .WithMessage("Incompatibilidad física: Las placas LGA 1200 son exclusivas para DDR4.");

            // Regla para LGA 1700 (Soporta DDR4 o DDR5)
            RuleFor(m => m.SupportedRam)
                .Must(ram => ram == RamType.DDR4 || ram == RamType.DDR5)
                .When(m => m.Socket == SocketType.LGA1700)
                .WithMessage("Error de Hardware: Una placa LGA 1700 debe estar diseñada para DDR4 o DDR5.");

            RuleFor(m => m.FormFactor) // Asumiendo que así se llama tu propiedad EATX, ATX, etc.
                .IsInEnum().WithMessage("Debes seleccionar un formato de placa madre válido (ATX, MicroATX, EATX, MiniITX).");

            RuleFor(m => m.PcieGeneration)
    .IsInEnum().WithMessage("Debes seleccionar una generación de PCIe válida (ej. Gen3,Gen4,Gen5).");
        }
    }
}
