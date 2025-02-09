using Microsoft.AspNetCore.Mvc;
using CRUDGenerator.Interfaces;
using CRUDGenerator.Contracts;

namespace CRUDGenerator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SampleController : ControllerBase
    {
        private readonly IServices _SampleServices;

        public SampleController(IServices sampleServices)
        {
            _SampleServices = sampleServices;
        }
        [HttpPost]
        public async Task<IActionResult> CreateSampleAsync(CreateSampleRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _SampleServices.CreateAsync(request);
                return Ok(new { message = "Elemento creado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error al crear el elemento", error = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var sample = _SampleServices.GetAll();
                if (sample == null || !sample.Any())
                {
                    return Ok(new { message = "No se encontraron elementos" });
                }
                return Ok(new { message = "Elementos recuperados exitosamente", data = sample });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error al recuperar todos los elementos", error = ex.Message });
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            try
            {
                var sample = await _SampleServices.GetByIdAsync(id);
                if (sample == null)
                {
                    return NotFound(new { message = $"No se encontró el elemento con Id {id}." });
                }
                return Ok(new { message = $"Elemento con Id {id} recuperado exitosamente.", data = sample });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ocurrió un error al recuperar el elemento con Id {id}.", error = ex.Message });
            }
        }
        [HttpPut("{id:guid}")]

        public async Task<IActionResult> UpdateSampleAsync(Guid id, UpdateSampleRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var sample = await _SampleServices.GetByIdAsync(id);
                if (sample == null)
                {
                    return NotFound(new { message = $"Elemento con id {id} no encontrado" });
                }
                await _SampleServices.UpdateAsync(id, request);
                return Ok(new { message = $"Elemento con id {id} actualizado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ocurrió un error al actualizar el elemento con id {id}", error = ex.Message });
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteSampleAsync(Guid id)
        {
            try
            {
                await _SampleServices.DeleteAsync(id);
                return Ok(new { message = $"Elemento con id {id} eliminado exitosamente" });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ocurrió un error al eliminar el elemento con id {id}", error = ex.Message });

            }
        }
    }
}