using Catalog.Application.Dtos;
using Catalog.Application.Interfaces;
using MassTransit;

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
        public async Task<IActionResult> GetAllPlates([FromQuery] string sortOrder = "asc", [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var plates = await _plateService.GetAllPlates();

            // Apply 20% markup to SalePrice
            foreach (var plate in plates)
            {
                plate.SalePrice = plate.SalePrice * 1.20m;
            }

            var totalCount = plates.Count();

            // Apply sorting
            plates = sortOrder == "desc"
                ? plates.OrderByDescending(p => p.SalePrice).ToList()
                : plates.OrderBy(p => p.SalePrice).ToList();

            // Apply pagination
            var pagedPlates = plates.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var result = new PagedResult<PlateDto>
            {
                Items = pagedPlates,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> AddPlate([FromBody] PlateDto plate)
        {
            if (plate.Id == Guid.Empty)
            {
                plate.Id = Guid.NewGuid(); 
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdPlate = await _plateService.AddPlate(plate);

                if (createdPlate != null)
                {
                    return Ok(createdPlate);
                }

                return StatusCode(500, "Failed to add plate");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while adding the plate.");
            }
        }
    }
}
