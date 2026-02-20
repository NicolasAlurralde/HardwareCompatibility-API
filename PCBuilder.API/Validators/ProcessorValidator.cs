using FluentValidation;
using PCBuilder.Domain.Entities;
using PCBuilder.Domain.Enums;

using FluentValidation;
using PCBuilder.Domain.Entities;

namespace PCBuilder.API.Validators
{
    public class ProcessorValidator : AbstractValidator<Processor>
    {
        public ProcessorValidator()
        {
            RuleFor(p => p.Model).NotEmpty().MinimumLength(3);
            RuleFor(p => p.Price).GreaterThan(0);

            // --- REGLAS DE MEMORIA RAM vs SOCKET ---

            // 1. DDR3 (Antiguos)
            RuleFor(p => p.SupportedRam)
                .Must(ram => ram == RamType.DDR3)
                .When(p => p.Socket == SocketType.AM3 || p.Socket == SocketType.LGA1150)
                .WithMessage("Error: Los sockets AM3 y LGA 1150 solo soportan DDR3.");

            // 2. DDR4 (El estándar predominante, incluyendo LGA 1200)
            RuleFor(p => p.SupportedRam)
                .Must(ram => ram == RamType.DDR4)
                .When(p => p.Socket == SocketType.AM4 || p.Socket == SocketType.LGA1200) // <-- Agregado aquí
                .WithMessage("Error: Los sockets AM4 y LGA 1200 solo soportan memoria DDR4.");

            // 3. DDR5 (Lo más nuevo)
            RuleFor(p => p.SupportedRam)
                .Must(ram => ram == RamType.DDR5)
                .When(p => p.Socket == SocketType.AM5)
                .WithMessage("Error: El socket AM5 es exclusivo para DDR5.");

            // 4. Híbrido (Intel 12va+ Gen)
            RuleFor(p => p.SupportedRam)
                .Must(ram => ram == RamType.DDR4 || ram == RamType.DDR5)
                .When(p => p.Socket == SocketType.LGA1700)
                .WithMessage("Error: LGA 1700 debe ser DDR4 o DDR5.");


        }
    }
}