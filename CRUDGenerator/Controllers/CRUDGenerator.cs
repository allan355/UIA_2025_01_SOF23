using CRUDGenerator.Contracts;
using CRUDGenerator.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CRUDGenerator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CRUDGenerator : ControllerBase
    {
        private readonly IGeneratorService _GeneratorService;

        public CRUDGenerator(IGeneratorService GeneratorService)
        {
            _GeneratorService = GeneratorService;
        }
        [HttpGet]
        public async Task<IActionResult> CreateSampleAsync(string request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var colunms = await _GeneratorService.GetAllColums(request);
                return Ok(colunms);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error al crear el elemento", error = ex.Message });
            }
        }


    }
}