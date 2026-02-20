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

        // GET: api/PcCases
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PcCase>>> GetAll([FromQuery] string? model)
        {
            var PcCases = await _repository.GetAllAsync();

            if (!string.IsNullOrEmpty(model))
            {
                PcCases = PcCases.Where(m => m.Model.ToLower().Contains(model.ToLower()));
            }

            return Ok(PcCases);
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
