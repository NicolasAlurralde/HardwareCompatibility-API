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
    public class RamsController : ControllerBase
    {
        private readonly IRepository<Ram> _repository;

        public RamsController(IRepository<Ram> repository)
        {
            _repository = repository;
        }

        // GET: api/rams
        // Filtros opcionales: ?model=Fury&type=DDR4
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ram>>> GetAll([FromQuery] string? model, [FromQuery] string? type)
        {
            // Traemos todas las memorias del repositorio
            var rams = await _repository.GetAllAsync();

            // 1. Filtro por Modelo (ej: "Fury", "Vengeance")
            if (!string.IsNullOrEmpty(model))
            {
                rams = rams.Where(r => r.Model.Contains(model, StringComparison.OrdinalIgnoreCase));
            }

            // 2. Filtro Inteligente por Tipo (Convirtiendo el Enum DDR3, DDR4, DDR5 a texto)
            if (!string.IsNullOrEmpty(type))
            {
                // ¡Acá usamos tu propiedad r.Type!
                rams = rams.Where(r => string.Equals(r.Type.ToString(), type, StringComparison.OrdinalIgnoreCase));
            }

            return Ok(rams);
        }

        // GET: api/Rams/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ram>> GetById(int id)
        {
            var Ram = await _repository.GetByIdAsync(id);
            if (Ram == null) return NotFound();
            return Ok(Ram);
        }

        // POST: api/Rams
        [HttpPost]
        public async Task<ActionResult<Ram>> Create(Ram Ram)
        {
            var createdRam = await _repository.AddAsync(Ram);
            return CreatedAtAction(nameof(GetById), new { id = createdRam.Id }, createdRam);
        }

        // PUT: api/Rams/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Ram Ram)
        {
            if (id != Ram.Id) return BadRequest("El ID no coincide.");

            var existingRam = await _repository.GetByIdAsync(id);
            if (existingRam == null) return NotFound();

            // Limpiamos el tracker para evitar el error de EF Core
            // (Si implementaste esto directo en tu Repository.cs, no hace falta ponerlo aquí)
            await _repository.UpdateAsync(Ram);

            return NoContent();
        }

        // DELETE: api/Rams/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var Ram = await _repository.GetByIdAsync(id);
            if (Ram == null) return NotFound();

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}