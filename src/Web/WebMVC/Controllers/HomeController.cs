using System.Diagnostics;
using System.Net.Http;
using Catalog.Application.Dtos;
using Catalog.WebMVC.Models;
using Newtonsoft.Json;
using RTCodingExercise.Microservices.Models;
using WebMVC;

namespace RTCodingExercise.Microservices.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("PlateApi");
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Plates(string sortOrder = "asc", int page = 1, int pageSize = 20)
        {
            try
            {
                var response = await _httpClient.GetStringAsync($"api/plate?sortOrder={sortOrder}&page={page}&pageSize={pageSize}");

                var result = JsonConvert.DeserializeObject<PaginatedPlatesResponse>(response);

                if (result?.Items == null || !result.Items.Any())
                {
                    _logger.LogWarning("No plates found in the API response.");
                }

                var plateViewModels = result?.Items.Select(plateDto => new PlateViewModel
                {
                    Registration = plateDto.Registration,
                    PurchasePrice = plateDto.PurchasePrice,
                    SalePrice = plateDto.SalePrice
                }).ToList();

                // Handle AJAX requests
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new
                    {
                        items = plateViewModels,
                        page = result.Page,
                        pageSize = result.PageSize,
                        totalCount = result.TotalCount
                    });
                }

                return View(plateViewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while fetching plates: {ex.Message}");
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult AddPlate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPlate([FromBody] PlateViewModel plate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _httpClient.PostAsJsonAsync("api/plate", plate);

            if (response.IsSuccessStatusCode)
            {
                return Ok(new { message = "Plate added successfully" });
            }

            return StatusCode((int)response.StatusCode, "Failed to add plate.");
        }
    }
}