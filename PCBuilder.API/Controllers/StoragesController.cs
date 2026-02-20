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
    public class StoragesController : ControllerBase
    {
        private readonly IRepository<Storage> _repository;

        public StoragesController(IRepository<Storage> repository)
        {
            _repository = repository;
        }

        // GET: api/Storages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Storage>>> GetAll([FromQuery] string? model)
        {
            var Storages = await _repository.GetAllAsync();

            if (!string.IsNullOrEmpty(model))
            {
                Storages = Storages.Where(m => m.Model.ToLower().Contains(model.ToLower()));
            }

            return Ok(Storages);
        }

        // GET: api/Storages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Storage>> GetById(int id)
        {
            var Storage = await _repository.GetByIdAsync(id);
            if (Storage == null) return NotFound();
            return Ok(Storage);
        }

        // POST: api/Storages
        [HttpPost]
        public async Task<ActionResult<Storage>> Create(Storage Storage)
        {
            var createdStorage = await _repository.AddAsync(Storage);
            return CreatedAtAction(nameof(GetById), new { id = createdStorage.Id }, createdStorage);
        }

        // PUT: api/Storages/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Storage Storage)
        {
            if (id != Storage.Id) return BadRequest("El ID no coincide.");

            var existingStorage = await _repository.GetByIdAsync(id);
            if (existingStorage == null) return NotFound();

            // Limpiamos el tracker para evitar el error de EF Core
            // (Si implementaste esto directo en tu Repository.cs, no hace falta ponerlo aquí)
            await _repository.UpdateAsync(Storage);

            return NoContent();
        }

        // DELETE: api/Storages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var Storage = await _repository.GetByIdAsync(id);
            if (Storage == null) return NotFound();

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}