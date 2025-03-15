using Microsoft.AspNetCore.Mvc;
using CRUDGenerator.Interfaces;
using CRUDGenerator.Contracts;
using CRUDGenerator.Models;

namespace CRUDGenerator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AllanTestController : ControllerBase
    {
        private readonly IAllanTestServices _AllanTestServices;

        public AllanTestController(IAllanTestServices AllanTestServices)
        {
            _AllanTestServices = AllanTestServices;
        }
        [HttpPost]
        public async Task<IActionResult> CreateAllanTestAsync(AllanTestRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _AllanTestServices.CreateAsync(request);
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
                var AllanTest = (List<AllanTest>)_AllanTestServices.GetAll();
                if (AllanTest == null || !AllanTest.Any())
                {
                    return Ok(new { message = "No se encontraron elementos" });
                }
                return Ok(new { message = "Elementos recuperados exitosamente", data = AllanTest });
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
                var AllanTest = (AllanTest)await _AllanTestServices.GetByIdAsync(id);
                if (AllanTest == null)
                {
                    return NotFound(new { message = $"No se encontró el elemento con Id {id}." });
                }
                return Ok(new { message = $"Elemento con Id {id} recuperado exitosamente.", data = AllanTest });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ocurrió un error al recuperar el elemento con Id {id}.", error = ex.Message });
            }
        }
        [HttpPut("{id:guid}")]

        public async Task<IActionResult> UpdateAllanTestAsync(Guid id, AllanTestRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var AllanTest = await _AllanTestServices.GetByIdAsync(id);
                if (AllanTest == null)
                {
                    return NotFound(new { message = $"Elemento con id {id} no encontrado" });
                }
                await _AllanTestServices.UpdateAsync(id, request);
                return Ok(new { message = $"Elemento con id {id} actualizado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ocurrió un error al actualizar el elemento con id {id}", error = ex.Message });
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAllanTestAsync(Guid id)
        {
            try
            {
                await _AllanTestServices.DeleteAsync(id);
                return Ok(new { message = $"Elemento con id {id} eliminado exitosamente" });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ocurrió un error al eliminar el elemento con id {id}", error = ex.Message });

            }
        }
    }
}