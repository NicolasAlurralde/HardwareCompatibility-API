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
    public class VideoCardsController : ControllerBase
    {
        private readonly IRepository<VideoCard> _repository;

        public VideoCardsController(IRepository<VideoCard> repository)
        {
            _repository = repository;
        }

        // GET: api/VideoCards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VideoCard>>> GetAll([FromQuery] string? model)
        {
            var VideoCards = await _repository.GetAllAsync();

            if (!string.IsNullOrEmpty(model))
            {
                VideoCards = VideoCards.Where(m => m.Model.ToLower().Contains(model.ToLower()));
            }

            return Ok(VideoCards);
        }

        // GET: api/VideoCards/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VideoCard>> GetById(int id)
        {
            var VideoCard = await _repository.GetByIdAsync(id);
            if (VideoCard == null) return NotFound();
            return Ok(VideoCard);
        }

        // POST: api/VideoCards
        [HttpPost]
        public async Task<ActionResult<VideoCard>> Create(VideoCard VideoCard)
        {
            var createdVideoCard = await _repository.AddAsync(VideoCard);
            return CreatedAtAction(nameof(GetById), new { id = createdVideoCard.Id }, createdVideoCard);
        }

        // PUT: api/VideoCards/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, VideoCard VideoCard)
        {
            if (id != VideoCard.Id) return BadRequest("El ID no coincide.");

            var existingVideoCard = await _repository.GetByIdAsync(id);
            if (existingVideoCard == null) return NotFound();

            // Limpiamos el tracker para evitar el error de EF Core
            // (Si implementaste esto directo en tu Repository.cs, no hace falta ponerlo aquí)
            await _repository.UpdateAsync(VideoCard);

            return NoContent();
        }

        // DELETE: api/VideoCards/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var VideoCard = await _repository.GetByIdAsync(id);
            if (VideoCard == null) return NotFound();

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
