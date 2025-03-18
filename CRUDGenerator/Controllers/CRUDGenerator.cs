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
        [Route("Tables")]
        public async Task<IActionResult> CreateSampleAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var colunms = await _GeneratorService.GetAllTables();
                return Ok(colunms);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error al crear el elemento", error = ex.Message });
            }
        }
        [HttpPost]
        [Route("CreateCrud")]
        public async Task<IActionResult> CreateCRUD(string request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var colunms = _GeneratorService.CreateCRUD(request);
                return Ok(colunms);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error al crear el elemento", error = ex.Message });
            }
        }


    }
}