using Microsoft.AspNetCore.Mvc;
using PCBuilder.Domain.Entities;
using PCBuilder.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PCBuilder.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PcCasesController : ControllerBase
    {
        private readonly IRepository<PcCase> _repository;

        public PcCasesController(IRepository<PcCase> repository)
        {
            _repository = repository;
        }

        // GET: api/pccases
        // Filtros opcionales físicos, de tamaño y refrigeración
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PcCase>>> GetAll(
            [FromQuery] string? model,
            [FromQuery] string? motherboardSize,
            [FromQuery] string? psuFormFactor,
            [FromQuery] int? coolerHeightMm,
            [FromQuery] int? gpuLengthMm,
            [FromQuery] bool? requiresLiquidCooling, // <-- NUEVO: ¿Eligió un Watercooler?
            [FromQuery] int? radiatorSizeMm)         // <-- NUEVO: ¿De qué tamaño es el radiador?
        {
            // Traemos todos los gabinetes del repositorio
            var pcCases = await _repository.GetAllAsync();

            // 1. Filtro por Modelo
            if (!string.IsNullOrEmpty(model))
            {
                pcCases = pcCases.Where(c => c.Model.Contains(model, StringComparison.OrdinalIgnoreCase));
            }

            // 2. Filtro Jerárquico por Tamaño de Placa Madre
            if (!string.IsNullOrEmpty(motherboardSize))
            {
                var targetSize = motherboardSize.ToLower();
                var allowedSizes = new List<string>();

                if (targetSize == "miniitx") allowedSizes.AddRange(new[] { "miniitx", "microatx", "atx", "eatx" });
                else if (targetSize == "microatx") allowedSizes.AddRange(new[] { "microatx", "atx", "eatx" });
                else if (targetSize == "atx") allowedSizes.AddRange(new[] { "atx", "eatx" });
                else if (targetSize == "eatx") allowedSizes.Add("eatx");
                else allowedSizes.Add(targetSize);

                pcCases = pcCases.Where(c => allowedSizes.Contains(c.MaxMotherboardSize.ToString().ToLower()));
            }

            // 3. Filtro por Formato de la Fuente
            if (!string.IsNullOrEmpty(psuFormFactor))
            {
                pcCases = pcCases.Where(c => string.Equals(c.SupportedPsuFormFactor.ToString(), psuFormFactor, StringComparison.OrdinalIgnoreCase));
            }

            // 4. Filtro Matemático: Altura del Cooler (Para coolers de aire)
            if (coolerHeightMm.HasValue)
            {
                pcCases = pcCases.Where(c => c.MaxCpuCoolerHeightMm >= coolerHeightMm.Value);
            }

            // 5. Filtro Matemático: Largo de la Placa de Video
            if (gpuLengthMm.HasValue)
            {
                pcCases = pcCases.Where(c => c.MaxGpuLengthMm >= gpuLengthMm.Value);
            }

            // ==========================================
            // 6 y 7. LOS NUEVOS FILTROS DE LÍQUIDA
            // ==========================================

            // Si el frontend nos avisa que el usuario eligió refrigeración líquida...
            if (requiresLiquidCooling.HasValue && requiresLiquidCooling.Value)
            {
                // Excluimos los gabinetes de oficina o muy chicos que no soportan líquida
                pcCases = pcCases.Where(c => c.SupportsLiquidCooling);
            }

            // Y si nos dice de qué tamaño es el radiador (ej: 240)...
            if (radiatorSizeMm.HasValue && radiatorSizeMm.Value > 0)
            {
                // Dejamos solo los gabinetes cuyo límite sea 240 o más (ej: 240, 280, 360, 420)
                pcCases = pcCases.Where(c => c.MaxRadiatorSizeMm >= radiatorSizeMm.Value);
            }

            return Ok(pcCases);
        }

        // GET: api/PcCases/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PcCase>> GetById(int id)
        {
            var PcCase = await _repository.GetByIdAsync(id);
            if (PcCase == null) return NotFound();
            return Ok(PcCase);
        }

        // POST: api/PcCases
        [HttpPost]
        public async Task<ActionResult<PcCase>> Create(PcCase PcCase)
        {
            var createdPcCase = await _repository.AddAsync(PcCase);
            return CreatedAtAction(nameof(GetById), new { id = createdPcCase.Id }, createdPcCase);
        }

        // PUT: api/PcCases/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PcCase PcCase)
        {
            if (id != PcCase.Id) return BadRequest("El ID no coincide.");

            var existingPcCase = await _repository.GetByIdAsync(id);
            if (existingPcCase == null) return NotFound();

            // Limpiamos el tracker para evitar el error de EF Core
            // (Si implementaste esto directo en tu Repository.cs, no hace falta ponerlo aquí)
            await _repository.UpdateAsync(PcCase);

            return NoContent();
        }

        // DELETE: api/PcCases/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var PcCase = await _repository.GetByIdAsync(id);
            if (PcCase == null) return NotFound();

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
