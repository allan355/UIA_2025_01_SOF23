using Microsoft.AspNetCore.Mvc;
using CRUDGenerator.Interfaces;
using CRUDGenerator.Contracts;
using CRUDGenerator.Models;

namespace CRUDGenerator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonaController : ControllerBase
    {
        private readonly IPersonaServices _PersonaServices;

        public PersonaController(IPersonaServices PersonaServices)
        {
            _PersonaServices = PersonaServices;
        }
        [HttpPost]
        public async Task<IActionResult> CreatePersonaAsync(PersonaRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _PersonaServices.CreateAsync(request);
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
                var Persona = (List<Persona>)_PersonaServices.GetAll();
                if (Persona == null || !Persona.Any())
                {
                    return Ok(new { message = "No se encontraron elementos" });
                }
                return Ok(new { message = "Elementos recuperados exitosamente", data = Persona });
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
                var Persona = (Persona)await _PersonaServices.GetByIdAsync(id);
                if (Persona == null)
                {
                    return NotFound(new { message = $"No se encontró el elemento con Id {id}." });
                }
                return Ok(new { message = $"Elemento con Id {id} recuperado exitosamente.", data = Persona });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ocurrió un error al recuperar el elemento con Id {id}.", error = ex.Message });
            }
        }
        [HttpPut("{id:guid}")]

        public async Task<IActionResult> UpdatePersonaAsync(Guid id, PersonaRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var Persona = await _PersonaServices.GetByIdAsync(id);
                if (Persona == null)
                {
                    return NotFound(new { message = $"Elemento con id {id} no encontrado" });
                }
                await _PersonaServices.UpdateAsync(id, request);
                return Ok(new { message = $"Elemento con id {id} actualizado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ocurrió un error al actualizar el elemento con id {id}", error = ex.Message });
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeletePersonaAsync(Guid id)
        {
            try
            {
                await _PersonaServices.DeleteAsync(id);
                return Ok(new { message = $"Elemento con id {id} eliminado exitosamente" });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Ocurrió un error al eliminar el elemento con id {id}", error = ex.Message });

            }
        }
    }
}