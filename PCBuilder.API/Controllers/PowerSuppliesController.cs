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
    public class PowerSuppliesController : ControllerBase
    {
        private readonly IRepository<PowerSupply> _repository;

        public PowerSuppliesController(IRepository<PowerSupply> repository)
        {
            _repository = repository;
        }

        // GET: api/PowerSupplies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PowerSupply>>> GetAll([FromQuery] string? model)
        {
            var PowerSupplies = await _repository.GetAllAsync();

            if (!string.IsNullOrEmpty(model))
            {
                PowerSupplies = PowerSupplies.Where(m => m.Model.ToLower().Contains(model.ToLower()));
            }

            return Ok(PowerSupplies);
        }

        // GET: api/PowerSupplies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PowerSupply>> GetById(int id)
        {
            var PowerSupply = await _repository.GetByIdAsync(id);
            if (PowerSupply == null) return NotFound();
            return Ok(PowerSupply);
        }

        // POST: api/PowerSupplies
        [HttpPost]
        public async Task<ActionResult<PowerSupply>> Create(PowerSupply PowerSupply)
        {
            var createdPowerSupply = await _repository.AddAsync(PowerSupply);
            return CreatedAtAction(nameof(GetById), new { id = createdPowerSupply.Id }, createdPowerSupply);
        }

        // PUT: api/PowerSupplies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PowerSupply PowerSupply)
        {
            if (id != PowerSupply.Id) return BadRequest("El ID no coincide.");

            var existingPowerSupply = await _repository.GetByIdAsync(id);
            if (existingPowerSupply == null) return NotFound();

            // Limpiamos el tracker para evitar el error de EF Core
            // (Si implementaste esto directo en tu Repository.cs, no hace falta ponerlo aquí)
            await _repository.UpdateAsync(PowerSupply);

            return NoContent();
        }

        // DELETE: api/PowerSupplies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var PowerSupply = await _repository.GetByIdAsync(id);
            if (PowerSupply == null) return NotFound();

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}