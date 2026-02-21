using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCBuilder.API.DTOs;
using PCBuilder.Domain.Entities;
using PCBuilder.Infrastructure;
using PCBuilder.Infrastructure.Data; // Asegurate de que este sea el namespace de tu DbContext

namespace PCBuilder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PCBuildsController : ControllerBase
    {
        private readonly AppDbContext _context; // Cambiá esto si tu context se llama distinto (Ej: PCBuilderDbContext)
        private readonly IValidator<PCBuild> _validator;

        public PCBuildsController(AppDbContext context, IValidator<PCBuild> validator)
        {
            _context = context;
            _validator = validator;
        }

        [HttpPost("analyze")]
        public async Task<IActionResult> AnalyzeBuild([FromBody] PCBuildRequestDto buildRequest) // <-- ¡Solo cambiamos la clase aquí!
        {
            // 1.OBLIGATORIOS
            var processor = await _context.Processors.FindAsync(buildRequest.ProcessorId);
            if (processor == null) return NotFound(new { Mensaje = $"El Procesador con ID {buildRequest.ProcessorId} no existe." });

            var motherboard = await _context.Motherboards.FindAsync(buildRequest.MotherboardId);
            if (motherboard == null) return NotFound(new { Mensaje = $"La Placa Madre con ID {buildRequest.MotherboardId} no existe." });

            // 2. OPCIONALES (Usando .HasValue estricto)
            var ram = buildRequest.RamId.HasValue ? await _context.Rams.FindAsync(buildRequest.RamId.Value) : null;
            if (buildRequest.RamId.HasValue && ram == null) return NotFound(new { Mensaje = $"La RAM con ID {buildRequest.RamId} no existe." });

            var powerSupply = buildRequest.PowerSupplyId.HasValue ? await _context.PowerSupplies.FindAsync(buildRequest.PowerSupplyId.Value) : null;
            if (buildRequest.PowerSupplyId.HasValue && powerSupply == null) return NotFound(new { Mensaje = $"La Fuente con ID {buildRequest.PowerSupplyId} no existe." });

            var pcCase = buildRequest.PcCaseId.HasValue ? await _context.PcCases.FindAsync(buildRequest.PcCaseId.Value) : null;
            if (buildRequest.PcCaseId.HasValue && pcCase == null) return NotFound(new { Mensaje = $"El Gabinete con ID {buildRequest.PcCaseId} no existe." });

            var storage = buildRequest.StorageId.HasValue ? await _context.Storages.FindAsync(buildRequest.StorageId.Value) : null;
            if (buildRequest.StorageId.HasValue && storage == null) return NotFound(new { Mensaje = $"El Almacenamiento con ID {buildRequest.StorageId} no existe." });

            var secondaryStorage = buildRequest.SecondaryStorageId.HasValue ? await _context.Storages.FindAsync(buildRequest.SecondaryStorageId.Value) : null;
            if (buildRequest.SecondaryStorageId.HasValue && secondaryStorage == null) return NotFound(new { Mensaje = $"El Almacenamiento Secundario con ID {buildRequest.SecondaryStorageId} no existe." });

            var videoCard = buildRequest.VideoCardId.HasValue ? await _context.VideoCards.FindAsync(buildRequest.VideoCardId.Value) : null;
            if (buildRequest.VideoCardId.HasValue && videoCard == null) return NotFound(new { Mensaje = $"La Tarjeta de Video con ID {buildRequest.VideoCardId} no existe." });

            var secondaryVideoCard = buildRequest.SecondaryVideoCardId.HasValue ? await _context.VideoCards.FindAsync(buildRequest.SecondaryVideoCardId.Value) : null;
            if (buildRequest.SecondaryVideoCardId.HasValue && secondaryVideoCard == null) return NotFound(new { Mensaje = $"La GPU Secundaria con ID {buildRequest.SecondaryVideoCardId} no existe." });

            var cooler = buildRequest.CoolerId.HasValue ? await _context.Coolers.FindAsync(buildRequest.CoolerId.Value) : null;
            if (buildRequest.CoolerId.HasValue && cooler == null) return NotFound(new { Mensaje = $"El Cooler con ID {buildRequest.CoolerId} no existe." });



            // 3. Ensamblar el "Carrito" en memoria (¡Actualizado!)
            var pcBuild = new PCBuild
            {
                BuildName = buildRequest.BuildName,
                Processor = processor,
                Motherboard = motherboard,

                Ram = ram,
                // Si RamId tiene un valor, usamos la cantidad del JSON. Si es null, forzamos un 0.
                RamQuantity = buildRequest.RamId.HasValue ? buildRequest.RamQuantity : 0,

                PowerSupply = powerSupply,
                PcCase = pcCase,

                Storage = storage,
                StorageQuantity = buildRequest.StorageId.HasValue ? buildRequest.StorageQuantity : 0,

                SecondaryStorage = secondaryStorage,
                SecondaryStorageQuantity = buildRequest.SecondaryStorageId.HasValue ? buildRequest.SecondaryStorageQuantity : 0,

                VideoCard = videoCard,
                SecondaryVideoCard = secondaryVideoCard,
                Cooler = cooler
            };

            // 4. Pasar la PC armada por el Validador (Tu Jefe Final)
            var validationResult = await _validator.ValidateAsync(pcBuild);

            // Filtramos las alertas por severidad
            var erroresCriticos = validationResult.Errors.Where(e => e.Severity == Severity.Error).ToList();
            var advertencias = validationResult.Errors.Where(e => e.Severity == Severity.Warning).ToList();

            // 5. Si hay ERRORES CRÍTICOS, bloqueamos el ensamble (400 Bad Request)
            if (erroresCriticos.Any())
            {
                return BadRequest(new
                {
                    Titulo = "Incompatibilidad Detectada",
                    Errores = erroresCriticos.Select(e => new { e.PropertyName, Mensaje = e.ErrorMessage })
                });
            }

            // 6. Si no hay errores críticos, ¡el ensamble es válido! (200 OK)
            // Devolvemos el precio y adjuntamos las advertencias amarillas (si es que hay alguna)
            return Ok(new
            {
                Mensaje = advertencias.Any()
                    ? "Ensamble aprobado con advertencias. Revisa los detalles."
                    : "¡Ensamble perfecto! Todas las piezas son compatibles.",
                PrecioTotal = pcBuild.TotalPrice,
                Advertencias = advertencias.Select(a => new { a.PropertyName, Mensaje = a.ErrorMessage })
            });
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveBuild([FromBody] PCBuildRequestDto buildRequest)
        {
            // 1 y 2. BUSCAR PIEZAS (Copiamos la misma lógica perfecta y estricta que armamos recién)
            var processor = await _context.Processors.FindAsync(buildRequest.ProcessorId);
            if (processor == null) return NotFound(new { Mensaje = $"El Procesador con ID {buildRequest.ProcessorId} no existe." });

            var motherboard = await _context.Motherboards.FindAsync(buildRequest.MotherboardId);
            if (motherboard == null) return NotFound(new { Mensaje = $"La Placa Madre con ID {buildRequest.MotherboardId} no existe." });

            var ram = buildRequest.RamId.HasValue ? await _context.Rams.FindAsync(buildRequest.RamId.Value) : null;
            if (buildRequest.RamId.HasValue && ram == null) return NotFound(new { Mensaje = $"La RAM no existe." });

            var powerSupply = buildRequest.PowerSupplyId.HasValue ? await _context.PowerSupplies.FindAsync(buildRequest.PowerSupplyId.Value) : null;
            if (buildRequest.PowerSupplyId.HasValue && powerSupply == null) return NotFound(new { Mensaje = $"La Fuente no existe." });

            var pcCase = buildRequest.PcCaseId.HasValue ? await _context.PcCases.FindAsync(buildRequest.PcCaseId.Value) : null;
            if (buildRequest.PcCaseId.HasValue && pcCase == null) return NotFound(new { Mensaje = $"El Gabinete no existe." });

            var storage = buildRequest.StorageId.HasValue ? await _context.Storages.FindAsync(buildRequest.StorageId.Value) : null;
            if (buildRequest.StorageId.HasValue && storage == null) return NotFound(new { Mensaje = $"El Almacenamiento no existe." });

            var secondaryStorage = buildRequest.SecondaryStorageId.HasValue ? await _context.Storages.FindAsync(buildRequest.SecondaryStorageId.Value) : null;
            if (buildRequest.SecondaryStorageId.HasValue && secondaryStorage == null) return NotFound(new { Mensaje = $"El Almacenamiento Secundario no existe." });

            var videoCard = buildRequest.VideoCardId.HasValue ? await _context.VideoCards.FindAsync(buildRequest.VideoCardId.Value) : null;
            if (buildRequest.VideoCardId.HasValue && videoCard == null) return NotFound(new { Mensaje = $"La Tarjeta de Video no existe." });

            var secondaryVideoCard = buildRequest.SecondaryVideoCardId.HasValue ? await _context.VideoCards.FindAsync(buildRequest.SecondaryVideoCardId.Value) : null;
            if (buildRequest.SecondaryVideoCardId.HasValue && secondaryVideoCard == null) return NotFound(new { Mensaje = $"La GPU Secundaria no existe." });

            var cooler = buildRequest.CoolerId.HasValue ? await _context.Coolers.FindAsync(buildRequest.CoolerId.Value) : null;
            if (buildRequest.CoolerId.HasValue && cooler == null) return NotFound(new { Mensaje = $"El Cooler no existe." });

            // 3. ENSAMBLAR Y SANITIZAR CANTIDADES
            var pcBuild = new PCBuild
            {
                BuildName = buildRequest.BuildName,
                Processor = processor,
                Motherboard = motherboard,
                Ram = ram,
                RamQuantity = buildRequest.RamId.HasValue ? buildRequest.RamQuantity : 0,
                PowerSupply = powerSupply,
                PcCase = pcCase,
                Storage = storage,
                StorageQuantity = buildRequest.StorageId.HasValue ? buildRequest.StorageQuantity : 0,
                SecondaryStorage = secondaryStorage,
                SecondaryStorageQuantity = buildRequest.SecondaryStorageId.HasValue ? buildRequest.SecondaryStorageQuantity : 0,
                VideoCard = videoCard,
                SecondaryVideoCard = secondaryVideoCard,
                Cooler = cooler
            };

            // 4. VALIDAR (El Jefe Final)
            var validationResult = await _validator.ValidateAsync(pcBuild);
            var erroresCriticos = validationResult.Errors.Where(e => e.Severity == Severity.Error).ToList();

            if (erroresCriticos.Any())
            {
                return BadRequest(new
                {
                    Titulo = "Incompatibilidad Detectada - No se puede guardar el ensamble",
                    Errores = erroresCriticos.Select(e => new { e.PropertyName, Mensaje = e.ErrorMessage })
                });
            }

            // 5. ¡LA MAGIA DE ENTITY FRAMEWORK! (Guardar en Base de Datos)
            _context.PCBuilds.Add(pcBuild);
            await _context.SaveChangesAsync();

            // 6. RESPONDER CON ÉXITO (Código 201 Created)
            var advertencias = validationResult.Errors.Where(e => e.Severity == Severity.Warning).ToList();

            return CreatedAtAction(nameof(GetBuildById), new { id = pcBuild.Id }, new
            {
                Mensaje = "¡Ensamble guardado exitosamente en la base de datos!",
                BuildId = pcBuild.Id,
                PrecioTotal = pcBuild.TotalPrice,
                Advertencias = advertencias.Select(a => new { a.PropertyName, Mensaje = a.ErrorMessage })
            });
        }

        // ==========================================
        // ENDPOINT AUXILIAR (Para buscar la PC guardada con todos sus detalles)
        // ==========================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBuildById(int id)
        {
            var build = await _context.PCBuilds
                .Include(x => x.Processor)
                .Include(x => x.Motherboard)
                .Include(x => x.Ram)
                .Include(x => x.PowerSupply)
                .Include(x => x.PcCase)
                .Include(x => x.Storage)
                .Include(x => x.SecondaryStorage)
                .Include(x => x.VideoCard)
                .Include(x => x.SecondaryVideoCard)
                .Include(x => x.Cooler)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (build == null) return NotFound();

            return Ok(build);
        }
    }
    }
