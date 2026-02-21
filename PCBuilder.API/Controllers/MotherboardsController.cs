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
    public class MotherboardsController : ControllerBase
    {
        private readonly IRepository<Motherboard> _repository;

        public MotherboardsController(IRepository<Motherboard> repository)
        {
            _repository = repository;
        }

        // GET: api/motherboards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Motherboard>>> GetAll([FromQuery] string? model, [FromQuery] string? socket, [FromQuery] string? supportedRam)
        {
            // Traemos todas las placas del repositorio
            var motherboards = await _repository.GetAllAsync();

            // 1. Filtro por Modelo (Optimizado)
            if (!string.IsNullOrEmpty(model))
            {
                motherboards = motherboards.Where(m => m.Model.Contains(model, StringComparison.OrdinalIgnoreCase));
            }

            // 2. Filtro Inteligente por Socket (Convirtiendo el Enum a String)
            if (!string.IsNullOrEmpty(socket))
            {
                motherboards = motherboards.Where(m => string.Equals(m.Socket.ToString(), socket, StringComparison.OrdinalIgnoreCase));
            }

            // 3. Filtro Inteligente por Tipo de RAM (Convirtiendo el Enum a String)
            if (!string.IsNullOrEmpty(supportedRam))
            {
                motherboards = motherboards.Where(m => string.Equals(m.SupportedRam.ToString(), supportedRam, StringComparison.OrdinalIgnoreCase));
            }

            return Ok(motherboards);
        }

        // GET: api/motherboards/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Motherboard>> GetById(int id)
        {
            var motherboard = await _repository.GetByIdAsync(id);
            if (motherboard == null) return NotFound();
            return Ok(motherboard);
        }

        // POST: api/motherboards
        [HttpPost]
        public async Task<ActionResult<Motherboard>> Create(Motherboard motherboard)
        {
            var createdMotherboard = await _repository.AddAsync(motherboard);
            return CreatedAtAction(nameof(GetById), new { id = createdMotherboard.Id }, createdMotherboard);
        }

        // PUT: api/motherboards/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Motherboard motherboard)
        {
            if (id != motherboard.Id) return BadRequest("El ID no coincide.");

            var existingMotherboard = await _repository.GetByIdAsync(id);
            if (existingMotherboard == null) return NotFound();

            // Limpiamos el tracker para evitar el error de EF Core
            // (Si implementaste esto directo en tu Repository.cs, no hace falta ponerlo aquí)
            await _repository.UpdateAsync(motherboard);

            return NoContent();
        }

        // DELETE: api/motherboards/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var motherboard = await _repository.GetByIdAsync(id);
            if (motherboard == null) return NotFound();

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
