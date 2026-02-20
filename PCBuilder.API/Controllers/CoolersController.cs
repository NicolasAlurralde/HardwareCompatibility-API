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

        // GET: api/Coolers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cooler>>> GetAll([FromQuery] string? model)
        {
            var Coolers = await _repository.GetAllAsync();

            if (!string.IsNullOrEmpty(model))
            {
                Coolers = Coolers.Where(m => m.Model.ToLower().Contains(model.ToLower()));
            }

            return Ok(Coolers);
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
