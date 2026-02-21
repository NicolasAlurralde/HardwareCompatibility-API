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
    public class CoolersController : ControllerBase
    {
        private readonly IRepository<Cooler> _repository;

        public CoolersController(IRepository<Cooler> repository)
        {
            _repository = repository;
        }

        // GET: api/coolers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cooler>>> GetAll(
            [FromQuery] string? model,
            [FromQuery] string? type,
            [FromQuery] string? socket) // <-- El socket del procesador que ya eligió el cliente
        {
            var coolers = await _repository.GetAllAsync();

            // 1. Filtro por Modelo
            if (!string.IsNullOrEmpty(model))
            {
                coolers = coolers.Where(c => c.Model.Contains(model, StringComparison.OrdinalIgnoreCase));
            }

            // 2. Filtro por Tipo (Air, Liquid)
            if (!string.IsNullOrEmpty(type))
            {
                coolers = coolers.Where(c => string.Equals(c.Type.ToString(), type, StringComparison.OrdinalIgnoreCase));
            }

            // 3. Filtro Inteligente: Buscar dentro de la lista de Sockets Soportados
            if (!string.IsNullOrEmpty(socket))
            {
                // Verificamos si la lista de anclajes del cooler contiene el socket que nos piden
                coolers = coolers.Where(c => c.SupportedSockets.Any(s => string.Equals(s.ToString(), socket, StringComparison.OrdinalIgnoreCase)));
            }

            return Ok(coolers);
        }

        // GET: api/Coolers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cooler>> GetById(int id)
        {
            var Cooler = await _repository.GetByIdAsync(id);
            if (Cooler == null) return NotFound();
            return Ok(Cooler);
        }

        // POST: api/Coolers
        [HttpPost]
        public async Task<ActionResult<Cooler>> Create(Cooler Cooler)
        {
            var createdCooler = await _repository.AddAsync(Cooler);
            return CreatedAtAction(nameof(GetById), new { id = createdCooler.Id }, createdCooler);
        }

        // PUT: api/Coolers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Cooler Cooler)
        {
            if (id != Cooler.Id) return BadRequest("El ID no coincide.");

            var existingCooler = await _repository.GetByIdAsync(id);
            if (existingCooler == null) return NotFound();

            // Limpiamos el tracker para evitar el error de EF Core
            // (Si implementaste esto directo en tu Repository.cs, no hace falta ponerlo aquí)
            await _repository.UpdateAsync(Cooler);

            return NoContent();
        }

        // DELETE: api/Coolers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var Cooler = await _repository.GetByIdAsync(id);
            if (Cooler == null) return NotFound();

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
