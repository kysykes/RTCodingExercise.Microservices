using Catalog.Application.Dtos;
using Catalog.Application.Interfaces;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlateController : ControllerBase
    {
        private readonly IPlateService _plateService;

        public PlateController(IPlateService plateService)
        {
            _plateService = plateService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPlates()
        {
            var plates = await _plateService.GetAllPlates();
            return Ok(plates);
        }

        [HttpPost]
        public async Task<IActionResult> AddPlate([FromBody] PlateDto plate)
        {
            if (plate.Id == Guid.Empty)
            {
                plate.Id = Guid.NewGuid();
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdPlate = await _plateService.AddPlate(plate);

            return CreatedAtAction(nameof(GetAllPlates), new { id = createdPlate.Id }, createdPlate);
        }
    }
}
