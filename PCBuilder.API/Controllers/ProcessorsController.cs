using Microsoft.AspNetCore.Mvc;
using PCBuilder.Domain.Entities;
using PCBuilder.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PCBuilder.API.Controllers
{
    // Esta ruta significa que accederemos a esta puerta yendo a: http://localhost:puerto/api/processors
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessorsController : ControllerBase
    {
        private readonly IRepository<Processor> _repository;

        // ¡Aquí ocurre la inyección de dependencias!
        public ProcessorsController(IRepository<Processor> repository)
        {
            _repository = repository;
        }

        // GET: api/processors
        // Filtros: ?model=Ryzen&socket=AM4&hasIntegratedGraphics=true&includesStockCooler=true
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Processor>>> GetAll(
            [FromQuery] string? model,
            [FromQuery] string? socket,
            [FromQuery] bool? hasIntegratedGraphics,
            [FromQuery] bool? includesStockCooler) // <-- NUEVO: El filtro del cooler de fábrica
        {
            var processors = await _repository.GetAllAsync();

            // 1. Filtro por Modelo
            if (!string.IsNullOrEmpty(model))
            {
                processors = processors.Where(p => p.Model.Contains(model, StringComparison.OrdinalIgnoreCase));
            }

            // 2. Filtro Inteligente: Socket
            if (!string.IsNullOrEmpty(socket))
            {
                processors = processors.Where(p => string.Equals(p.Socket.ToString(), socket, StringComparison.OrdinalIgnoreCase));
            }

            // 3. Filtro Lógico: Gráficos integrados
            if (hasIntegratedGraphics.HasValue)
            {
                processors = processors.Where(p => p.HasIntegratedGraphics == hasIntegratedGraphics.Value);
            }

            // 4. Filtro Económico: ¿Trae Cooler de Stock?
            if (includesStockCooler.HasValue)
            {
                processors = processors.Where(p => p.IncludesStockCooler == includesStockCooler.Value); // Cambiá esto por el nombre exacto de tu propiedad
            }

            return Ok(processors);
        }

        // GET: api/processors/5
        // Endpoint para buscar un procesador específico por su ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Processor>> GetById(int id)
        {
            var processor = await _repository.GetByIdAsync(id);
            if (processor == null)
            {
                return NotFound(); // Devuelve un código 404 si no existe
            }
            return Ok(processor);
        }

        // PUT: api/processors/5
        // Endpoint para ACTUALIZAR un procesador existente
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Processor processor)
        {
            // Medida de seguridad: el ID de la URL debe coincidir con el del objeto
            if (id != processor.Id)
            {
                return BadRequest("El ID de la URL no coincide con el ID del procesador.");
            }

            // Verificamos si realmente existe en la base de datos
            var existingProcessor = await _repository.GetByIdAsync(id);
            if (existingProcessor == null)
            {
                return NotFound("El procesador que intentas actualizar no existe.");
            }

            // Si todo está bien, lo actualizamos
            // OJO: Entity Framework rastrea el objeto, así que debemos "desconectarlo" primero 
            // o simplemente actualizar sus propiedades. Para mantenerlo simple con tu repositorio actual:
            await _repository.UpdateAsync(processor);

            return NoContent(); // Código 204: Significa "Se actualizó con éxito y no tengo nada más que decirte"
        }

        // DELETE: api/processors/5
        // Endpoint para ELIMINAR un procesador
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var processor = await _repository.GetByIdAsync(id);
            if (processor == null)
            {
                return NotFound("El procesador que intentas eliminar no existe.");
            }

            await _repository.DeleteAsync(id);

            return NoContent(); // Código 204: "Lo borré y no hay más nada que mostrar"
        }

        // POST: api/processors
        // Endpoint para guardar un procesador NUEVO en la base de datos
        [HttpPost]
        public async Task<ActionResult<Processor>> Create(Processor processor)
        {
            var createdProcessor = await _repository.AddAsync(processor);

            // Devuelve un código 201 Created y la ruta exacta para ver el nuevo registro
            return CreatedAtAction(nameof(GetById), new { id = createdProcessor.Id }, createdProcessor);
        }
    }
}
